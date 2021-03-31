import os
from flask import Flask
from app.main.config import config_by_name

app_config = config_by_name[os.getenv('BOILERPLATE_ENV') if os.getenv('BOILERPLATE_ENV') else 'dev']
basedir = os.path.abspath(os.path.dirname(__file__))


def create_app():
    app = Flask(__name__)
    app.config.from_object(app_config)

    return app
