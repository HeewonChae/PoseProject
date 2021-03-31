from dbms.football.procedure import *
from datetime import datetime, timedelta
from dateutil.relativedelta import relativedelta
import numpy as np
import pandas as pd
import enums
import time
import configparser
import os
import json


# 점수에 따른 결과 타입 반환 (승, 무, 패)
def get_result_type(home_score, away_score):
    return enums.MatchResultType.Win if home_score > away_score \
        else home_score == away_score and enums.MatchResultType.Draw or enums.MatchResultType.Lose


# 양팀 득점 여부 (1 이면 양팀 득, 아니면 0)
def get_both_to_score(score1, score2):
    return 1 if score1 > 0 and score2 > 0 else 0


# 두 점수에 따른 각 언더 오버 결과 참일 경우 1
def get_under_over(score1, score2):
    score_sum = score1 + score2
    ret = [int(score_sum < 1.5), int(score_sum > 1.5),
           int(score_sum < 2.5), int(score_sum > 2.5),
           int(score_sum < 3.5), int(score_sum > 3.5),
           int(score_sum < 4.5), int(score_sum > 4.5),
           int(score_sum < 5.5), int(score_sum > 5.5)]
    return ret


# 휴식 기간
def get_rest_period(cur_match_time, last_fixture):
    last_match_time = last_fixture['match_time']
    date_diff = cur_match_time - last_match_time
    seconds_in_day = 24 * 60 * 60
    return min(14, (date_diff.total_seconds()/seconds_in_day))


# 6골 초과인 점수를 6로 변경
def convert_max_score(data_frame):
    if len(data_frame) == 0:
        return data_frame

    data_frame['home_score'] = np.where(data_frame['home_score'] > 5, 5, data_frame['home_score'])
    data_frame['away_score'] = np.where(data_frame['away_score'] > 5, 5, data_frame['away_score'])
    return data_frame


# 데이터베이스에서 매개변수로 받은 시간내에 이루어진 경기들 모두 가져옴
def get_fixtures(start, end):
    # 프로시저 실행
    fixtures = p_get_fixture.by_match_time(start_time=start, end_time=end)
    return convert_max_score(pd.DataFrame(fixtures))


# 해당 팀의 최근 경기 정보
def get_last_fixtures(league_id, team_id, camp_type, match_time, count):
    last_fixtures = p_get_fixture.last_team_fixtures(league_id, team_id, camp_type, str(match_time), count)
    last_fixtures = convert_max_score(pd.DataFrame(last_fixtures))

    if len(last_fixtures) == 0:
        return last_fixtures

    min_match_time = match_time - timedelta(days=90)
    last_fixtures = last_fixtures[last_fixtures['match_time'] > min_match_time]

    return last_fixtures


# 해당 두팀이 맞붙은 경기 정보
def get_last_h2h_fixtures(team_id1, team_id2, match_time, count):
    last_h2h_fixtures = p_get_fixture.last_h2h_fixtures(team_id1, team_id2, str(match_time), count)
    return convert_max_score(pd.DataFrame(last_h2h_fixtures))


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


# 해당 팀의 최근 폼 트렌드
def get_form_trend(team_id, camp_type, match_time, fixtures):
    match_cnt = len(fixtures)
    if match_cnt < 3:
        return [0, 0]

    sum_att_trend = 0.0
    sum_def_trend = 0.0
    my_standings = get_score_avg(team_id, fixtures)

    for fixture in (fixtures.iloc[0:5]).to_dict('records'):
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
            sum_att_trend += my_score - \
                             ((2 * my_standings[0] * op_standings[1]) / (my_standings[0] + op_standings[1]))
        else:
            sum_att_trend += 0

        # def_trend = ((2 * 상평득 * 내평실) / (상평득 + 내평실)) - 실점
        if (my_standings[1] + op_standings[0]) != 0:
            sum_def_trend += ((2 * my_standings[1] * op_standings[0]) / (my_standings[1] + op_standings[0])) \
                             - op_score
        else:
            sum_def_trend += 0

    return [sum_att_trend / 3, sum_def_trend / 3]


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


# 매개변수로 받은 fixture_id의 경기 데이터를 머신러닝에서 사용할 수 있도록 가공한 데이터 프레임을 반환
def get_prepared_data_frame(fixture_id, league_id, home_team_id, away_team_id, home_score, away_score, match_time):
    df = pd.DataFrame(columns=ignore_columns + data_columns + label_columns)

    # 리그정보 가져오기
    league_info = p_get_league.by_league_id(league_id)
    league_type = enums.LeagueType[league_info['type']]
    # country_name = league_info['country_name']
    # league_name = league_info['name']

    # 커버 가능한 리그, 컵인지 체크
    # key = f"{country_name}:{league_type.name}:{league_name}"

    # if key not in football_coverage_leagues.index:
    #     return df

    home_team_last_9matches = get_last_fixtures(league_id, home_team_id, enums.CampType.Any, match_time, 9)
    away_team_last_9matches = get_last_fixtures(league_id, away_team_id, enums.CampType.Any, match_time, 9)

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

    home_result = get_result_type(home_score, away_score)
    away_result = get_result_type(away_score, home_score)
    both_to_score = get_both_to_score(home_score, away_score)
    under_over = get_under_over(home_score, away_score)

    # 홈팀
    df.loc[0] = [match_time, enums.CampType.Home.value, league_type.value,
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
                 home_rest_period, away_rest_period,
                 home_score, home_result.value, both_to_score] + under_over

    # 원정팀
    df.loc[1] = [match_time, enums.CampType.Away.value, league_type.value,
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
                 away_rest_period, home_rest_period,
                 away_score, away_result.value, both_to_score] + under_over
    return df


def setting_football_data_dir():
    config = configparser.ConfigParser()
    root_dir = os.path.dirname(os.path.abspath(__file__))  # This is your Project Root
    config_path = os.path.join(root_dir, 'config.ini')  # requires `import os`
    config.read(config_path)
    save_dir_path = config['DATA_DIR_PATH']['FOOTBALL_DATA_PATH_V2']

    abs_path = os.path.abspath(save_dir_path)
    if not (os.path.isdir(abs_path)):
        os.makedirs(abs_path)

    return abs_path


def get_league_coverage():
    config = configparser.ConfigParser()
    root_dir = os.path.dirname(os.path.abspath(__file__))  # This is your Project Root
    config_path = os.path.join(root_dir, 'config.ini')  # requires `import os`
    config.read(config_path)
    res_dir_path = os.path.join(root_dir, config['DATA_DIR_PATH']['FOOTBALL_RES_PATH'])
    file_path = os.path.join(res_dir_path, 'CoverageLeagues.json')

    with open(file_path, encoding='utf-8') as json_file:
        json_data = json.load(json_file)

        indices = []
        for data in json_data:
            indices.append(f"{data['CountryName']}:{data['LeagueType']}:{data['LeagueName']}")

    return pd.DataFrame(json_data, index=indices)


ignore_columns = ['match_time']

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

label_columns = ['score', 'result_type', 'both_to_score',
                 '1.5_under', '1.5_over',
                 '2.5_under', '2.5_over',
                 '3.5_under', '3.5_over',
                 '4.5_under', '4.5_over',
                 '5.5_under', '5.5_over']

if __name__ == "__main__":
    # football_coverage_leagues = get_league_coverage()

    # 10년전 ~ 현재
    start_time = datetime(year=2020, month=10, day=1)
    end_time = start_time + relativedelta(months=1) - timedelta(seconds=1)

    while start_time < datetime.utcnow():
        fixture_df = get_fixtures(start=start_time, end=end_time)

        data_cnt = fixture_df.shape[0]
        print(f'data count: {data_cnt}, time:{start_time} - {end_time} \n')

        # 머신러닝에 사용할 데이터 프레임 생성
        prepared_df = pd.DataFrame(columns=ignore_columns + data_columns + label_columns)

        start_prepared = time.time()
        for index, row in fixture_df.iterrows():
            prepared_df = prepared_df.append(get_prepared_data_frame(**row.to_dict()), ignore_index=True)
            print(f'processing football {start_time.strftime("%Y-%m")} data ({index + 1}/{data_cnt})')

        # Save .csv format
        football_data_dir = setting_football_data_dir()
        fin_path = os.path.join(football_data_dir, f'{start_time.strftime("%Y%m")}.csv')
        prepared_df.to_csv(fin_path, sep=',', na_rep=np.nan, index=False, float_format='%.3f')

        end_prepared = time.time()

        print(f'Save {start_time.strftime("%Y%m")}.csv (elapsed time: {end_prepared - start_prepared} sec)\n')

        start_time = start_time + relativedelta(months=1)
        end_time = start_time + relativedelta(months=1) - timedelta(seconds=1)
