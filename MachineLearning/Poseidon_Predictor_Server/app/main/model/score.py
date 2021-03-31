class Score():
    def __init__(self):
        self.lin_home_socre = None
        self.lin_away_score = None
        self.knn_home_score = None
        self.knn_away_score = None

    def SetLinHomeScore(self, lin_home_socre):
        self.lin_home_socre = lin_home_socre
    
    def SetLinAwayScore(self, lin_away_score):
        self.lin_away_score = lin_away_score

    def SetKnnHomeScore(self, knn_home_score):
        self.knn_home_score = knn_home_score

    def SetKnnAwayScore(self, knn_away_score):
        self.knn_away_score = knn_away_score
