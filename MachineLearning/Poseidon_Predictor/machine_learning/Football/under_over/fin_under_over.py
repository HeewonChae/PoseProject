import load_data
import enums
from load_data import get_camp_fixtures
from export_football_data import label_columns
from machine_learning.utility import load_sklearn_model
import numpy as np
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

home_result_1_5_over_Y = home_football_data['1.5_over']
home_result_2_5_over_Y = home_football_data['2.5_over']
home_result_3_5_over_Y = home_football_data['3.5_over']
home_result_4_5_over_Y = home_football_data['4.5_over']

away_result_1_5_over_Y = away_football_data['1.5_over']
away_result_2_5_over_Y = away_football_data['2.5_over']
away_result_3_5_over_Y = away_football_data['3.5_over']
away_result_4_5_over_Y = away_football_data['4.5_over']

# load models
knn_clf_1_5_any = load_sklearn_model('under_over/knn_clf_1_5_any.pkl')
rft_clf_1_5_any = load_sklearn_model('under_over/rft_clf_1_5_any.pkl')
sgd_clf_1_5_any = load_sklearn_model('under_over/sgd_clf_1_5_any.pkl')

knn_clf_2_5_any = load_sklearn_model('under_over/knn_clf_2_5_any.pkl')
rft_clf_2_5_any = load_sklearn_model('under_over/rft_clf_2_5_any.pkl')
sgd_clf_2_5_any = load_sklearn_model('under_over/sgd_clf_2_5_any.pkl')

knn_clf_3_5_any = load_sklearn_model('under_over/knn_clf_3_5_any.pkl')
rft_clf_3_5_any = load_sklearn_model('under_over/rft_clf_3_5_any.pkl')
sgd_clf_3_5_any = load_sklearn_model('under_over/sgd_clf_3_5_any.pkl')

knn_clf_4_5_any = load_sklearn_model('under_over/knn_clf_4_5_any.pkl')
rft_clf_4_5_any = load_sklearn_model('under_over/rft_clf_4_5_any.pkl')
sgd_clf_4_5_any = load_sklearn_model('under_over/sgd_clf_4_5_any.pkl')


# 1.5 under over
# predict
knn_home_pred = np.round(knn_clf_1_5_any.predict_proba(home_train), decimals=3)
knn_away_pred = np.round(knn_clf_1_5_any.predict_proba(away_train), decimals=3)

rft_home_pred = np.round(rft_clf_1_5_any.predict_proba(home_train), decimals=3)
rft_away_pred = np.round(rft_clf_1_5_any.predict_proba(away_train), decimals=3)

sgd_home_pred = np.round(sgd_clf_1_5_any.predict_proba(home_train), decimals=3)
sgd_away_pred = np.round(sgd_clf_1_5_any.predict_proba(away_train), decimals=3)

home_prepared_train = np.c_[np.ones(len(home_train)), np.zeros(len(home_train)),
                            knn_home_pred, rft_home_pred, sgd_home_pred]
away_prepared_train = np.c_[np.zeros(len(away_train)), np.ones(len(away_train)),
                            knn_away_pred, rft_away_pred, sgd_away_pred]

train_X = np.r_[home_prepared_train, away_prepared_train]
train_Y = np.r_[home_result_1_5_over_Y, away_result_1_5_over_Y]

# SGDClassifier 학습
start_prepared = time.time()

knn_clf_1_5 = KNeighborsClassifier(weights='uniform', n_neighbors=100)
knn_clf_1_5.fit(train_X, train_Y)

end_prepared = time.time()

print(f'1.5 knn predictor elapsed time: {end_prepared - start_prepared} sec \n')


# 2.5 under over
# predict
knn_home_pred = np.round(knn_clf_2_5_any.predict_proba(home_train), decimals=3)
knn_away_pred = np.round(knn_clf_2_5_any.predict_proba(away_train), decimals=3)

rft_home_pred = np.round(rft_clf_2_5_any.predict_proba(home_train), decimals=3)
rft_away_pred = np.round(rft_clf_2_5_any.predict_proba(away_train), decimals=3)

sgd_home_pred = np.round(sgd_clf_2_5_any.predict_proba(home_train), decimals=3)
sgd_away_pred = np.round(sgd_clf_2_5_any.predict_proba(away_train), decimals=3)

home_prepared_train = np.c_[np.ones(len(home_train)), np.zeros(len(home_train)),
                            knn_home_pred, rft_home_pred, sgd_home_pred]
away_prepared_train = np.c_[np.zeros(len(away_train)), np.ones(len(away_train)),
                            knn_away_pred, rft_away_pred, sgd_away_pred]

train_X = np.r_[home_prepared_train, away_prepared_train]
train_Y = np.r_[home_result_2_5_over_Y, away_result_2_5_over_Y]

# SGDClassifier 학습
start_prepared = time.time()

knn_clf_2_5 = KNeighborsClassifier(weights='uniform', n_neighbors=100)
knn_clf_2_5.fit(train_X, train_Y)

end_prepared = time.time()

print(f'2.5 knn predictor elapsed time: {end_prepared - start_prepared} sec \n')


# 3.5 under over
# predict
knn_home_pred = np.round(knn_clf_3_5_any.predict_proba(home_train), decimals=3)
knn_away_pred = np.round(knn_clf_3_5_any.predict_proba(away_train), decimals=3)

rft_home_pred = np.round(rft_clf_3_5_any.predict_proba(home_train), decimals=3)
rft_away_pred = np.round(rft_clf_3_5_any.predict_proba(away_train), decimals=3)

sgd_home_pred = np.round(sgd_clf_3_5_any.predict_proba(home_train), decimals=3)
sgd_away_pred = np.round(sgd_clf_3_5_any.predict_proba(away_train), decimals=3)

home_prepared_train = np.c_[np.ones(len(home_train)), np.zeros(len(home_train)),
                            knn_home_pred, rft_home_pred, sgd_home_pred]
away_prepared_train = np.c_[np.zeros(len(away_train)), np.ones(len(away_train)),
                            knn_away_pred, rft_away_pred, sgd_away_pred]

train_X = np.r_[home_prepared_train, away_prepared_train]
train_Y = np.r_[home_result_3_5_over_Y, away_result_3_5_over_Y]

# SGDClassifier 학습
start_prepared = time.time()

knn_clf_3_5 = KNeighborsClassifier(weights='uniform', n_neighbors=100)
knn_clf_3_5.fit(train_X, train_Y)

end_prepared = time.time()

print(f'3.5 knn predictor elapsed time: {end_prepared - start_prepared} sec \n')


# 4.5 under over
# predict
knn_home_pred = np.round(knn_clf_4_5_any.predict_proba(home_train), decimals=3)
knn_away_pred = np.round(knn_clf_4_5_any.predict_proba(away_train), decimals=3)

rft_home_pred = np.round(rft_clf_4_5_any.predict_proba(home_train), decimals=3)
rft_away_pred = np.round(rft_clf_4_5_any.predict_proba(away_train), decimals=3)

sgd_home_pred = np.round(sgd_clf_4_5_any.predict_proba(home_train), decimals=3)
sgd_away_pred = np.round(sgd_clf_4_5_any.predict_proba(away_train), decimals=3)

home_prepared_train = np.c_[np.ones(len(home_train)), np.zeros(len(home_train)),
                            knn_home_pred, rft_home_pred, sgd_home_pred]
away_prepared_train = np.c_[np.zeros(len(away_train)), np.ones(len(away_train)),
                            knn_away_pred, rft_away_pred, sgd_away_pred]

train_X = np.r_[home_prepared_train, away_prepared_train]
train_Y = np.r_[home_result_4_5_over_Y, away_result_4_5_over_Y]

# SGDClassifier 학습
start_prepared = time.time()

knn_clf_4_5 = KNeighborsClassifier(weights='uniform', n_neighbors=100)
knn_clf_4_5.fit(train_X, train_Y)

end_prepared = time.time()

print(f'4.5 knn predictor elapsed time: {end_prepared - start_prepared} sec \n')

# Save
dir_name = datetime.today().strftime("fin_%Y%m%d-%H%M%S")
utility.save_sklearn_model(knn_clf_1_5, dir_name, "fin_knn_clf_1_5.pkl")
utility.save_sklearn_model(knn_clf_2_5, dir_name, "fin_knn_clf_2_5.pkl")
utility.save_sklearn_model(knn_clf_3_5, dir_name, "fin_knn_clf_3_5.pkl")
utility.save_sklearn_model(knn_clf_4_5, dir_name, "fin_knn_clf_4_5.pkl")

