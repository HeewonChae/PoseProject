class FootballTeamStat:
    def __init__(self):
        self.avg_gf = None
        self.avg_ga = None
        self.att_trend = None
        self.def_trend = None
        self.rest_date = None

    def set_avg_gf(self, avg_gf):
        self.avg_gf = avg_gf

    def set_avg_ga(self, avg_ga):
        self.avg_ga = avg_ga

    def set_att_trend(self, att_trend):
        self.att_trend = att_trend

    def set_def_trend(self, def_trend):
        self.def_trend = def_trend

    def set_rest_date(self, rest_date):
        self.rest_date = rest_date
