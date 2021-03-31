class FootballCLF():
    def __init__(self):
        self.knn = None
        self.sgd = None
        self.sub = None

    def setKNN(self, knn):
        self.knn = knn

    def setSGD(self, sgd):
        self.sgd = sgd  

    def setSub(self, sub):
        self.sub = sub
        