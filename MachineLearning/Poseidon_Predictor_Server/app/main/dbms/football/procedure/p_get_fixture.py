from app.main.dbms.football.football_db import DB as FOOTBALL_DB
import app.main.model.enums as enums


def by_match_time(start_time, end_time):
    connection = FOOTBALL_DB.connect()

    try:
        with connection.cursor() as cursor:
            sql = "SELECT `id` as fixture_id, `league_id`, `home_team_id`, `away_team_id`, " \
                  "`home_score`, `away_score`, `match_time` " \
                  "FROM `fixture` " \
                  "WHERE `is_completed` = 1 AND `match_time` BETWEEN %s AND %s"
            cursor.execute(sql, (start_time, end_time))
            results = cursor.fetchall()

    finally:
        connection.close()

    return results


def by_fixture_id(fixture_id):
    connection = FOOTBALL_DB.connect()

    try:
        with connection.cursor() as cursor:
            sql = "SELECT `id` as fixture_id, `league_id`, `home_team_id`, `away_team_id`, " \
                  "`home_score`, `away_score`, `match_time` " \
                  "FROM `fixture` " \
                  "WHERE `id`=%s"
            cursor.execute(sql, fixture_id)
            results = cursor.fetchone()

    finally:
        connection.close()

    return results


def last_team_fixtures(league_id, team, camp_type, time, limit):
    connection = FOOTBALL_DB.connect()

    try:
        with connection.cursor() as cursor:
            if camp_type is enums.CampType.Home:
                sql = "SELECT `id` as fixture_id, `league_id`, `home_team_id`, `away_team_id`, " \
                      "`home_score`, `away_score`, `match_time` " \
                      "FROM `fixture` " \
                      "WHERE `league_id` = %s AND `home_team_id` = %s AND `match_time` < %s AND `is_completed` = 1 " \
                      "ORDER BY `match_time` DESC " \
                      "LIMIT %s"
                cursor.execute(sql, (league_id, team, time, limit))

            elif camp_type is enums.CampType.Away:
                sql = "SELECT `id` as fixture_id, `league_id`, `home_team_id`, `away_team_id`, " \
                      "`home_score`, `away_score`, `match_time` " \
                      "FROM `fixture` " \
                      "WHERE `league_id` = %s AND `away_team_id` = %s AND `match_time` < %s AND `is_completed` = 1 " \
                      "ORDER BY `match_time` DESC " \
                      "LIMIT %s"
                cursor.execute(sql, (league_id, team, time, limit))

            elif camp_type is enums.CampType.Any:
                sql = "SELECT `id` as fixture_id, `league_id`, `home_team_id`, `away_team_id`, " \
                      "`home_score`, `away_score`, `match_time` " \
                      "FROM `fixture` " \
                      "WHERE `league_id` = %s AND (`home_team_id` = %s OR `away_team_id` = %s) " \
                      "AND `match_time` < %s AND `is_completed` = 1 " \
                      "ORDER BY `match_time` DESC " \
                      "LIMIT %s"
                cursor.execute(sql, (league_id, team, team, time, limit))

            results = cursor.fetchall()

    finally:
        connection.close()

    return results


def last_h2h_fixtures(team_id1, team_id2, time, limit):
    connection = FOOTBALL_DB.connect()

    try:
        with connection.cursor() as cursor:
            sql = "SELECT `id` as fixture_id, `league_id`, `home_team_id`, `away_team_id`, " \
                  "`home_score`, `away_score`, `match_time` " \
                  "FROM `fixture` " \
                  "WHERE ((`home_team_id` = %s AND `away_team_id` = %s) OR  " \
                  "(`home_team_id` = %s AND `away_team_id` = %s)) " \
                  "AND `match_time` < %s " \
                  "ORDER BY `match_time` DESC " \
                  "LIMIT %s"
            cursor.execute(sql, (team_id1, team_id2, team_id2, team_id1, time, limit))
            results = cursor.fetchall()

    finally:
        connection.close()

    return results
