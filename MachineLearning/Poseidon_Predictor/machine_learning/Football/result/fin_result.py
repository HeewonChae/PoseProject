import load_data
import enums
from load_data import get_camp_fixtures
from export_football_data import label_columns
from machine_learning.utility import load_sklearn_model
import numpy as np
from sklearn.linear_model import SGDClassifier
from sklearn.neighbors import KNeighborsClassifier
import time
from datetime import datetime
import machine_learning.utility as utility


football_data = load_data.load_data(enums.SportsType.Football, enums.CampType.Any)
football_data.head()

home_football_data = get_camp_fixtures(enums.CampType.Home, football_data)
away_football_data = get_camp_fixtures(enums.CampType.Away, football_data)

home_train = home_football_data.drop(label_columns, axis=1)
away_train = away_football_data.drop(label_columns, axis=1)

home_result_Y = home_football_data['result_type']  # 경기 결과 (승=0, 무=1, 패=2)
away_result_Y = away_football_data['result_type']

# load models
knn_clf_any = load_sklearn_model('result/knn_clf_result_any.pkl')
rft_clf_any = load_sklearn_model('result/rft_clf_result_any.pkl')
sgd_clf_any = load_sklearn_model('result/sgd_clf_result_any.pkl')

# predict
knn_home_pred = np.round(knn_clf_any.predict_proba(home_train), decimals=3)
knn_away_pred = np.round(knn_clf_any.predict_proba(away_train), decimals=3)

rft_home_pred = np.round(rft_clf_any.predict_proba(home_train), decimals=3)
rft_away_pred = np.round(rft_clf_any.predict_proba(away_train), decimals=3)

sgd_home_pred = np.round(sgd_clf_any.predict_proba(home_train), decimals=3)
sgd_away_pred = np.round(sgd_clf_any.predict_proba(away_train), decimals=3)

home_prepared_train = np.c_[np.ones(len(home_train)), np.zeros(len(home_train)),
                            knn_home_pred, rft_home_pred, sgd_home_pred]

away_prepared_train = np.c_[np.zeros(len(away_train)), np.ones(len(away_train)),
                            knn_away_pred, rft_away_pred, sgd_away_pred]


train_X = np.r_[home_prepared_train, away_prepared_train]
train_Y = np.r_[home_result_Y, away_result_Y]

# SGDClassifier 학습
# start_prepared = time.time()
#
# sgd_clf = SGDClassifier(max_iter=3000, tol=1e-3, random_state=42, alpha=0.01, loss='log')
# sgd_clf.fit(train_X, train_Y)
#
# end_prepared = time.time()
#
# print(f'sgd result predictor elapsed time: {end_prepared - start_prepared} sec \n')


# KNeighborsClassifier 학습
start_prepared = time.time()

knn_clf = KNeighborsClassifier(weights='uniform', n_neighbors=100)
knn_clf.fit(train_X, train_Y)

end_prepared = time.time()

print(f'knn result predictor elapsed time: {end_prepared - start_prepared} sec \n')

# Save
dir_name = datetime.today().strftime("fin_%Y%m%d-%H%M%S")
# utility.save_sklearn_model(sgd_clf, dir_name, "fin_sgd_clf_result.pkl")
utility.save_sklearn_model(knn_clf, dir_name, "fin_knn_clf_result.pkl")
