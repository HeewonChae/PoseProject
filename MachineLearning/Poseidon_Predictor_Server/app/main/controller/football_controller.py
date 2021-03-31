from flask import request
from flask_restplus import Resource, Namespace
from app.main.service.football_svc import get_fixture_from_id, get_predict


api = Namespace('football', description='football related operations')


@api.route('/Predictor')
class Predictor(Resource):
    @api.param('fixture_id', 'The Fixture identifier')
    @api.response(1001, 'fixture_id is none')
    @api.response(1002, 'fixture not found.')
    def get(self):
        """Predict Football Fixture From fixture_id"""

        fixture_id = request.args.get('fixture_id')
        if fixture_id is None:
            return None

        fixture = get_fixture_from_id(fixture_id)
        if fixture is None:
            return None

        return get_predict(fixture)
