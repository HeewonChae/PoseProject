import configparser
import os
from app.main import basedir


class Database:
    DB_NAME = "na"

    def __init__(self):
        pass

    @staticmethod
    def read_config(section):
        config = configparser.ConfigParser()
        config_path = os.path.join(basedir, './dbms/mysql_config.ini')  # requires `import os`
        config.read(config_path)
        return config[section]
