import json
from json import JSONEncoder


class FootballPrediction:
    def __init__(self):
        self.score = None
        self.match_winner = None
        self.both_to_score = None
        self.under_over = None
        self.home_stat = None
        self.away_stat = None

    def set_score(self, score):
        self.score = score

    def set_match_winner(self, match_winner):
        self.match_winner = match_winner

    def set_both_to_score(self, both_to_score):
        self.both_to_score = both_to_score

    def set_under_over(self, under_over):
        self.under_over = under_over

    def set_home_stat(self, home_stat):
        self.home_stat = home_stat

    def set_away_stat(self, away_stat):
        self.away_stat = away_stat

    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__)


# # subclass JSONEncoder
# class FootballPredictionEncoder(JSONEncoder):
#         def default(self, o):
#             return o.__dict__
