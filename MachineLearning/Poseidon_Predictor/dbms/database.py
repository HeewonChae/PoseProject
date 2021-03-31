import configparser
import os


class Database:
    DB_NAME = "na"

    def __init__(self):
        pass

    @staticmethod
    def read_config(section):
        config = configparser.ConfigParser()
        root_dir = os.path.dirname(os.path.abspath(__file__))  # This is your Project Root
        config_path = os.path.join(root_dir, '../config.ini')  # requires `import os`
        config.read(config_path)
        return config[section]
