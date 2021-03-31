import os


class Config:
	SECRET_KEY = os.getenv('SECRET_KEY', '_poseidon_server_')
	DEBUG = False
	DB_CONFIG = ''


class DevelopmentConfig(Config):
	DEBUG = True
	DB_CONFIG = 'FOOTBALL_DB_CONNECTION_DEV'


class ProductionConfig(Config):
	DEBUG = False
	DB_CONFIG = 'FOOTBALL_DB_CONNECTION_PROD'


config_by_name = dict(
	dev=DevelopmentConfig,
	prod=ProductionConfig
)

key = Config.SECRET_KEY

