from dbms.database import Database
import pymysql.cursors


class FootballDB(Database):
    DB_NAME = "footballdb"

    def __init__(self):
        self.con_config = Database.read_config("FOOTBALL_DB_CONNECTION")

    def connect(self):
        return pymysql.connect(
            host=self.con_config['HOST'],
            user=self.con_config['USER'],
            password=self.con_config['PASSWORD'],
            db=FootballDB.DB_NAME,
            charset=self.con_config['CHARSET'],
            cursorclass=pymysql.cursors.DictCursor)


DB = FootballDB()
