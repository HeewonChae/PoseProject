import load_data
import enums
from load_data import get_camp_fixtures
from export_football_data import label_columns
from machine_learning.utility import load_sklearn_model
import numpy as np
from sklearn.linear_model import LinearRegression
import machine_learning.utility as utility
from datetime import datetime
import time

football_data = load_data.load_data(enums.SportsType.Football, enums.CampType.Any)
football_data.head()

home_football_data = get_camp_fixtures(enums.CampType.Home, football_data)
away_football_data = get_camp_fixtures(enums.CampType.Away, football_data)

home_train = home_football_data.drop(label_columns, axis=1)
away_train = away_football_data.drop(label_columns, axis=1)

home_score_Y = home_football_data['score']  # 점수
away_score_Y = away_football_data['score']  # 점수

# load models
knn_reg_any = load_sklearn_model('score/knn_reg_score_any.pkl')
lin_reg_any = load_sklearn_model('score/lin_reg_score_any.pkl')

# predict
knn_home_pred = np.round(knn_reg_any.predict(home_train), decimals=3)
knn_away_pred = np.round(knn_reg_any.predict(away_train), decimals=3)

lin_home_pred = np.round(lin_reg_any.predict(home_train), decimals=3)
lin_away_pred = np.round(lin_reg_any.predict(away_train), decimals=3)

# prepare data
# league_cat = utility.LeagueTypeCategorize()
# leagues = league_cat.transform(home_train.copy())
home_prepared_train = np.c_[np.ones(len(home_train)), np.zeros(len(home_train)),
                            knn_home_pred, lin_home_pred]
away_prepared_train = np.c_[np.zeros(len(away_train)), np.ones(len(away_train)),
                            knn_away_pred, lin_away_pred]

train_X = np.r_[home_prepared_train, away_prepared_train]
train_Y = np.r_[home_score_Y, away_score_Y]

# LinearRegression 학습
start_prepared = time.time()

fin_lin_reg = LinearRegression()
fin_lin_reg.fit(train_X, train_Y)  # 훈련

end_prepared = time.time()

print(f'lin score predictor elapsed time: {end_prepared - start_prepared} sec \n')

# Save
dir_name = datetime.today().strftime("fin_%Y%m%d-%H%M%S")
utility.save_sklearn_model(fin_lin_reg, dir_name, "fin_lin_reg_score.pkl")

