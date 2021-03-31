class ProbaWinner():
    def __init__(self):
        self.win_proba = None
        self.draw_proba = None
        self.lose_proba = None
    
    def SetWinProba(self, win_proba):
        self.win_proba = win_proba

    def SetDrawProba(self, draw_proba):
        self.draw_proba = draw_proba

    def SetLoseProba(self, lose_proba):
        self.lose_proba = lose_proba