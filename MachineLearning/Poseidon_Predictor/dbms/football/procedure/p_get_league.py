from dbms.football.footballdb import DB as FOOTBALL_DB


def by_league_id(league_id):
    connection = FOOTBALL_DB.connect()

    try:
        with connection.cursor() as cursor:
            sql = "SELECT * " \
                  "FROM `league` " \
                  "WHERE `id` = %s "
            cursor.execute(sql, league_id)
            result = cursor.fetchone()

    finally:
        connection.close()

    return result
