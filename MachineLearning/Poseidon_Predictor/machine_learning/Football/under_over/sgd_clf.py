from load_data import load_data, num_attribs, cat_attribs_with_camp
from export_football_data import label_columns
from sklearn.pipeline import Pipeline
from sklearn.compose import ColumnTransformer
from sklearn.preprocessing import OneHotEncoder, StandardScaler, PolynomialFeatures
from sklearn.linear_model import SGDClassifier
import machine_learning.utility as utility
import enums
import time
from datetime import datetime


any_football_data = load_data(enums.SportsType.Football, enums.CampType.Any)

# 학습 데이터셋
train_X = any_football_data.drop(label_columns, axis=1)
train_1_5_Y = any_football_data['1.5_over']
train_2_5_Y = any_football_data['2.5_over']
train_3_5_Y = any_football_data['3.5_over']
train_4_5_Y = any_football_data['4.5_over']

number_pipeline_home = Pipeline([
        ("poly", PolynomialFeatures(degree=2, include_bias=False)),
        ("scaler", StandardScaler()),
    ])

feature_pipeline = ColumnTransformer([
        ("num", number_pipeline_home, num_attribs),
        ("cat", OneHotEncoder(), cat_attribs_with_camp),
    ])

prepared_train = feature_pipeline.fit_transform(train_X)
print(f'prepared_train data_set shape: {prepared_train.shape}')

# 모델 훈련 (1.5)
start_prepared = time.time()

sgd_clf_1_5 = SGDClassifier(max_iter=2000, tol=1e-3, random_state=42, alpha=0.01, loss='log')
sgd_clf_1_5.fit(prepared_train, train_1_5_Y)

end_prepared = time.time()

print(f'1.5 predictor elapsed time: {end_prepared - start_prepared} sec \n')

full_pipeline_1_5 = Pipeline([
        ("preparation", feature_pipeline),
        ("predictor", sgd_clf_1_5)
    ])

# 모델 훈련 (2.5)
start_prepared = time.time()

sgd_clf_2_5 = SGDClassifier(max_iter=2000, tol=1e-3, random_state=42, alpha=0.01, loss='log')
sgd_clf_2_5.fit(prepared_train, train_2_5_Y)

end_prepared = time.time()

print(f'2.5 predictor elapsed time: {end_prepared - start_prepared} sec \n')

full_pipeline_2_5 = Pipeline([
        ("preparation", feature_pipeline),
        ("predictor", sgd_clf_2_5)
    ])

# 모델 훈련 (3.5)
start_prepared = time.time()

sgd_clf_3_5 = SGDClassifier(max_iter=2000, tol=1e-3, random_state=42, alpha=0.01, loss='log')
sgd_clf_3_5.fit(prepared_train, train_3_5_Y)

end_prepared = time.time()

print(f'3.5 predictor elapsed time: {end_prepared - start_prepared} sec \n')

full_pipeline_3_5 = Pipeline([
        ("preparation", feature_pipeline),
        ("predictor", sgd_clf_3_5)
    ])

# 모델 훈련 (4.5)
start_prepared = time.time()

sgd_clf_4_5 = SGDClassifier(max_iter=2000, tol=1e-3, random_state=42, alpha=0.01, loss='log')
sgd_clf_4_5.fit(prepared_train, train_4_5_Y)

end_prepared = time.time()

print(f'4.5 predictor elapsed time: {end_prepared - start_prepared} sec \n')

full_pipeline_4_5 = Pipeline([
        ("preparation", feature_pipeline),
        ("predictor", sgd_clf_4_5)
    ])

# 저장
dir_name = datetime.today().strftime("sgd_%Y%m%d-%H%M%S")
utility.save_sklearn_model(full_pipeline_1_5, dir_name, "sgd_clf_1_5_any.pkl")
utility.save_sklearn_model(full_pipeline_2_5, dir_name, "sgd_clf_2_5_any.pkl")
utility.save_sklearn_model(full_pipeline_3_5, dir_name, "sgd_clf_3_5_any.pkl")
utility.save_sklearn_model(full_pipeline_4_5, dir_name, "sgd_clf_4_5_any.pkl")
