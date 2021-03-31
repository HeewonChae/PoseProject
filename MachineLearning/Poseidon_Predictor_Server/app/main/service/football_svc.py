from app.main.dbms.football.procedure import *
import pandas as pd
import app.main.model.enums as enums
import numpy as np
from datetime import datetime, timedelta
from .football_prediction_svc import predict_score, predict_match_winner, predict_both_to_score, get_2_items_harmonic_mean, predict_under_over
from app.main.model.football.football_prediction import FootballPrediction
from app.main.model.football.football_team_stat import FootballTeamStat


data_columns = ['camp_type', 'league_type',
                'my_camp_3matches_gf_avg', 'my_camp_3matches_ga_avg',
                'my_6matches_gf_avg', 'my_6matches_ga_avg',
                'my_camp_3matches_pts', 'my_6matches_pts',
                'op_camp_3matches_gf_avg', 'op_camp_3matches_ga_avg',
                'op_6matches_gf_avg', 'op_6matches_ga_avg',
                'op_camp_3matches_pts', 'op_6matches_pts',
                'my_att_trend', 'my_def_trend',
                'op_att_trend', 'op_def_trend',
                'h2h_4matches_gf_avg', 'h2h_4matches_ga_avg',
                'h2h_4matches_pts',
                'my_rest_period', 'op_rest_period']


# 해당 캠프에 맞는 데이터만 가져오기
def get_camp_fixtures(camp_type, fixtures):
    data = fixtures[fixtures['camp_type'] == camp_type.value]

    return data


# 6골 초과인 점수를 6로 변경
def convert_max_score(data_frame):
    if len(data_frame) == 0:
        return data_frame

    data_frame['home_score'] = np.where(data_frame['home_score'] > 5, 5, data_frame['home_score'])
    data_frame['away_score'] = np.where(data_frame['away_score'] > 5, 5, data_frame['away_score'])
    return data_frame


# 해당 팀의 최근 경기 정보
def get_last_fixtures(league_id, team_id, camp_type, match_time, count):
    last_fixtures = p_get_fixture.last_team_fixtures(league_id, team_id, camp_type, str(match_time), count)
    last_fixtures = convert_max_score(pd.DataFrame(last_fixtures))

    if len(last_fixtures) == 0:
        return last_fixtures

    min_match_time = match_time - timedelta(days=90)
    last_fixtures = last_fixtures[last_fixtures['match_time'] > min_match_time]

    return last_fixtures


# 해당 팀의 평균 [득점, 실점]
def get_score_avg_from_db(league_id, team_id, camp_type, match_time, match_cnt):
    last_fixtures = get_last_fixtures(league_id, team_id, camp_type, match_time, match_cnt)
    fixture_cnt = len(last_fixtures)

    if fixture_cnt == 0:
        return [0, 0]

    sum_gf = 0
    sum_ga = 0
    for fixture in last_fixtures.to_dict('records'):
        if fixture['home_team_id'] == team_id:
            sum_gf += fixture['home_score']
            sum_ga += fixture['away_score']
        else:
            sum_gf += fixture['away_score']
            sum_ga += fixture['home_score']

    return [sum_gf / fixture_cnt, sum_ga / fixture_cnt]


# 해당 팀의 평균 [득점, 실점]
def get_score_avg(team_id, fixtures):
    fixture_cnt = len(fixtures)

    if fixture_cnt == 0:
        raise ValueError("get_score_avg - fixtures count is zero")

    sum_gf = 0
    sum_ga = 0
    for fixture in fixtures.to_dict('records'):
        if fixture['home_team_id'] == team_id:
            sum_gf += fixture['home_score']
            sum_ga += fixture['away_score']
        else:
            sum_gf += fixture['away_score']
            sum_ga += fixture['home_score']

    return [sum_gf / fixture_cnt, sum_ga / fixture_cnt]


# 승점 계산
def get_win_point(team_id, fixtures):
    match_cnt = len(fixtures)
    if match_cnt == 0:
        return 0

    sum_pts = 0
    for fixture in fixtures.to_dict('records'):
        if fixture['home_team_id'] == team_id:
            match_result = get_result_type(fixture['home_score'], fixture['away_score'])
        else:
            match_result = get_result_type(fixture['away_score'], fixture['home_score'])

        if match_result is enums.MatchResultType.Win:
            sum_pts += 3
        elif match_result is enums.MatchResultType.Draw:
            sum_pts += 1

    return sum_pts


# 점수에 따른 결과 타입 반환 (승, 무, 패)
def get_result_type(home_score, away_score):
    return enums.MatchResultType.Win if home_score > away_score \
        else home_score == away_score and enums.MatchResultType.Draw or enums.MatchResultType.Lose


# 휴식 기간
def get_rest_period(cur_match_time, last_fixture):
    last_match_time = last_fixture['match_time']
    date_diff = cur_match_time - last_match_time
    seconds_in_day = 24 * 60 * 60
    return min(14, (date_diff.total_seconds()/seconds_in_day))


# 해당 팀의 최근 폼 트렌드
def get_form_trend(team_id, camp_type, match_time, fixtures):
    match_cnt = len(fixtures)
    if match_cnt == 0:
        return [0, 0]

    sum_att_trend = 0.0
    sum_def_trend = 0.0
    my_standings = get_score_avg(team_id, fixtures)

    for fixture in fixtures.to_dict('records'):
        if camp_type is enums.CampType.Home:
            op_standings = get_score_avg_from_db(fixture['league_id'], fixture['away_team_id'],
                                                 enums.CampType.Away, match_time, match_cnt)
            my_score = fixture['home_score']
            op_score = fixture['away_score']
        elif camp_type is enums.CampType.Away:
            op_standings = get_score_avg_from_db(fixture['league_id'], fixture['home_team_id'],
                                                 enums.CampType.Home, match_time, match_cnt)
            my_score = fixture['away_score']
            op_score = fixture['home_score']
        else:
            if fixture['home_team_id'] == team_id:
                op_standings = get_score_avg_from_db(fixture['league_id'], fixture['away_team_id'],
                                                     enums.CampType.Any, match_time, match_cnt)
                my_score = fixture['home_score']
                op_score = fixture['away_score']
            else:
                op_standings = get_score_avg_from_db(fixture['league_id'], fixture['home_team_id'],
                                                     enums.CampType.Any, match_time, match_cnt)
                my_score = fixture['away_score']
                op_score = fixture['home_score']

        # 평균 득실점 조화 평균
        # att_trend = 득점 - ((2 * 내평득 * 상평실) / (내평득 + 상평실))
        if (my_standings[0] + op_standings[1]) != 0:
            sum_att_trend += my_score - get_2_items_harmonic_mean(my_standings[0], op_standings[1])
        else:
            sum_att_trend += 0

        # def_trend = ((2 * 상평득 * 내평실) / (상평득 + 내평실)) - 실점
        if (my_standings[1] + op_standings[0]) != 0:
            sum_def_trend += get_2_items_harmonic_mean(my_standings[1], op_standings[0]) - op_score
        else:
            sum_def_trend += 0

    return [sum_att_trend / match_cnt, sum_def_trend / match_cnt]


# H2H 평균 득 실점
def get_h2h_score_avg(team_id1, team_id2, fixtures):
    fixture_cnt = len(fixtures)

    if fixture_cnt == 0:
        return {team_id1: (0, 0), team_id2: (0, 0)}

    sum_gf_team1 = 0
    sum_gf_team2 = 0
    for fixture in fixtures.to_dict('records'):
        if fixture['home_team_id'] == team_id1:
            sum_gf_team1 += fixture['home_score']
            sum_gf_team2 += fixture['away_score']
        else:
            sum_gf_team1 += fixture['away_score']
            sum_gf_team2 += fixture['home_score']

    avg_gf = [sum_gf_team1 / fixture_cnt, sum_gf_team2 / fixture_cnt]

    return {team_id1: (avg_gf[0], avg_gf[1]), team_id2: (avg_gf[1], avg_gf[0])}


# 해당 두팀이 맞붙은 경기 정보
def get_last_h2h_fixtures(team_id1, team_id2, match_time, count):
    last_h2h_fixtures = p_get_fixture.last_h2h_fixtures(team_id1, team_id2, str(match_time), count)
    return convert_max_score(pd.DataFrame(last_h2h_fixtures))


# 매개변수로 받은 fixture_id의 경기 데이터를 머신러닝에서 사용할 수 있도록 가공한 데이터 프레임을 반환
def get_prepared_data_frame(fixture_id, league_id, home_team_id, away_team_id, home_score, away_score, match_time):
    df = pd.DataFrame(columns=data_columns)

    # 리그정보 가져오기
    league_info = p_get_league.by_league_id(league_id)
    league_type = enums.LeagueType[league_info['type']]

    home_team_last_9matches = get_last_fixtures(league_id, home_team_id, enums.CampType.Any, match_time, 12)
    away_team_last_9matches = get_last_fixtures(league_id, away_team_id, enums.CampType.Any, match_time, 12)

    if len(home_team_last_9matches) < 6 or len(away_team_last_9matches) < 6:
        return df

    home_camp_last_3matches = home_team_last_9matches[home_team_last_9matches['home_team_id'] == home_team_id]
    away_camp_last_3matches = away_team_last_9matches[away_team_last_9matches['away_team_id'] == away_team_id]

    if len(home_camp_last_3matches) < 3 or len(away_camp_last_3matches) < 3:
        return df

    home_team_last_6matches = home_team_last_9matches.iloc[0:6]
    home_camp_last_3matches = home_camp_last_3matches.iloc[0:3]

    away_team_last_6matches = away_team_last_9matches.iloc[0:6]
    away_camp_last_3matches = away_camp_last_3matches.iloc[0:3]

    home_camp_last_3matches_avg = get_score_avg(home_team_id, home_camp_last_3matches)
    home_last_6matches_avg = get_score_avg(home_team_id, home_team_last_6matches)
    home_camp_pts = get_win_point(home_team_id, home_camp_last_3matches)
    home_pts = get_win_point(home_team_id, home_team_last_6matches)

    away_camp_last_3matches_avg = get_score_avg(away_team_id, away_camp_last_3matches)
    away_last_6matches_avg = get_score_avg(away_team_id, away_team_last_6matches)
    away_camp_pts = get_win_point(away_team_id, away_camp_last_3matches)
    away_pts = get_win_point(away_team_id, away_team_last_6matches)

    home_trend = get_form_trend(home_team_id, enums.CampType.Any, match_time, home_team_last_6matches)
    away_trend = get_form_trend(away_team_id, enums.CampType.Any, match_time, away_team_last_6matches)

    h2h_fixtures = get_last_h2h_fixtures(home_team_id, away_team_id, match_time, 4)
    h2h_last_4matches_avg = get_h2h_score_avg(home_team_id, away_team_id, h2h_fixtures)
    h2h_home_pts = get_win_point(home_team_id, h2h_fixtures)
    h2h_away_pts = get_win_point(away_team_id, h2h_fixtures)

    home_rest_period = get_rest_period(match_time, home_team_last_6matches.iloc[0])
    away_rest_period = get_rest_period(match_time, away_team_last_6matches.iloc[0])

    # 홈팀
    df.loc[0] = [enums.CampType.Home.value, league_type.value,
                 home_camp_last_3matches_avg[0], home_camp_last_3matches_avg[1],
                 home_last_6matches_avg[0], home_last_6matches_avg[1],
                 home_camp_pts, home_pts,
                 away_camp_last_3matches_avg[0], away_camp_last_3matches_avg[1],
                 away_last_6matches_avg[0], away_last_6matches_avg[1],
                 away_camp_pts, away_pts,
                 home_trend[0], home_trend[1],
                 away_trend[0], away_trend[1],
                 h2h_last_4matches_avg[home_team_id][0], h2h_last_4matches_avg[home_team_id][1],
                 h2h_home_pts,
                 home_rest_period, away_rest_period]

    # 원정팀
    df.loc[1] = [enums.CampType.Away.value, league_type.value,
                 away_camp_last_3matches_avg[0], away_camp_last_3matches_avg[1],
                 away_last_6matches_avg[0], away_last_6matches_avg[1],
                 away_camp_pts, away_pts,
                 home_camp_last_3matches_avg[0], home_camp_last_3matches_avg[1],
                 home_last_6matches_avg[0], home_last_6matches_avg[1],
                 home_camp_pts, home_pts,
                 away_trend[0], away_trend[1],
                 home_trend[0], home_trend[1],
                 h2h_last_4matches_avg[away_team_id][0], h2h_last_4matches_avg[away_team_id][1],
                 h2h_away_pts,
                 away_rest_period, home_rest_period]
    return df


def get_fixture_from_id(fixture_id):
    is_digit = fixture_id.isdigit()

    if not is_digit:
        return None

    found_fixture = p_get_fixture.by_fixture_id(fixture_id)
    return found_fixture


def get_predict(fixture):
    # 머신러닝에 사용할 데이터 프레임 생성
    prepared_df = pd.DataFrame(columns=data_columns)
    prepared_df = prepared_df.append(get_prepared_data_frame(**fixture), ignore_index=True)
    if len(prepared_df) == 0:
        return None

    football_prediction = FootballPrediction()

    home_data = get_camp_fixtures(enums.CampType.Home, prepared_df)
    away_data = get_camp_fixtures(enums.CampType.Away, prepared_df)

    home_team_stat = FootballTeamStat()
    home_team_stat.set_avg_gf(home_data['my_6matches_gf_avg'].values[0])
    home_team_stat.set_avg_ga(home_data['my_6matches_ga_avg'].values[0])
    home_team_stat.set_att_trend(home_data['my_att_trend'].values[0])
    home_team_stat.set_def_trend(home_data['my_def_trend'].values[0])
    home_team_stat.set_rest_date(home_data['my_rest_period'].values[0])

    away_team_stat = FootballTeamStat()
    away_team_stat.set_avg_gf(away_data['my_6matches_gf_avg'].values[0])
    away_team_stat.set_avg_ga(away_data['my_6matches_ga_avg'].values[0])
    away_team_stat.set_att_trend(away_data['my_att_trend'].values[0])
    away_team_stat.set_def_trend(away_data['my_def_trend'].values[0])
    away_team_stat.set_rest_date(away_data['my_rest_period'].values[0])

    football_prediction.set_home_stat(home_team_stat)
    football_prediction.set_away_stat(away_team_stat)

    # score
    pred_score = predict_score(home_data, away_data)
    football_prediction.set_score(pred_score)

    # result
    pred_match_winner = predict_match_winner(home_data, away_data)
    football_prediction.set_match_winner(pred_match_winner)

    # both to score
    pred_both_to_score = predict_both_to_score(home_data, away_data)
    football_prediction.set_both_to_score(pred_both_to_score)

    # under over
    pred_uo = predict_under_over(home_data, away_data)
    football_prediction.set_under_over(pred_uo)

    return football_prediction.toJSON()
