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


# 로드 데이터
any_football_data = load_data(enums.SportsType.Football, enums.CampType.Any)

# 학습 데이터셋
train_X = any_football_data.drop(label_columns, axis=1)
train_Y = any_football_data['both_to_score']  # 경기 결과 (승=0, 무=1, 패=2)

# 특성 처리
number_pipeline_home = Pipeline([
        ("poly", PolynomialFeatures(degree=2, include_bias=False)),
        ("scaler", StandardScaler()),
    ])

columns_pipeline_home = ColumnTransformer([
        ("num", number_pipeline_home, num_attribs),
        ("cat", OneHotEncoder(), cat_attribs_with_camp),
    ])

feature_pipeline = Pipeline([
        ('columns', columns_pipeline_home),
    ])

# 모델 훈련
prepared_train = feature_pipeline.fit_transform(train_X)
print(f'prepared_train data_set shape: {prepared_train.shape}')

start_prepared = time.time()

sgd_clf = SGDClassifier(max_iter=2000, tol=1e-3, random_state=42, alpha=0.01, loss='log')
sgd_clf.fit(prepared_train, train_Y)

end_prepared = time.time()

print(f'elapsed time: {end_prepared - start_prepared} sec \n')

full_pipeline = Pipeline([
        ("preparation", feature_pipeline),
        ("predictor", sgd_clf)
    ])

# 저장
dir_name = datetime.today().strftime("sgd_%Y%m%d-%H%M%S")
utility.save_sklearn_model(full_pipeline, dir_name, "sgd_clf_bts_any.pkl")
