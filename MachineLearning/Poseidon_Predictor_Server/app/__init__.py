from flask_restplus import Api
from flask import Blueprint
from app.main.controller.football_controller import api as football_ns

blueprint = Blueprint('api', __name__)

api = Api(blueprint,
          title='POSEIDON PREDICTOR SERVER',
          version='1.0',
          description='a boilerplate for flask restplus web service'
          )

api.add_namespace(football_ns, path='/football')
