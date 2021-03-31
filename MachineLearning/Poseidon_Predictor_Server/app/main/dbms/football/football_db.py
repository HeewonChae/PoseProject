from app.main.dbms.database import Database
from app.main import app_config
import pymysql.cursors


class FootballDB(Database):
    DB_NAME = "football_db"

    def __init__(self):
        self.config_name = app_config.DB_CONFIG
        self.con_config = Database.read_config(self.config_name)
        print(f"db host: {self.con_config['HOST']}")

    def connect(self):
        return pymysql.connect(
            host=self.con_config['HOST'],
            user=self.con_config['USER'],
            password=self.con_config['PASSWORD'],
            db=self.con_config['DATABASE'],
            charset=self.con_config['CHARSET'],
            cursorclass=pymysql.cursors.DictCursor)


DB = FootballDB()
