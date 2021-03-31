from load_data import load_data, num_attribs, cat_attribs_with_camp
from export_football_data import label_columns
from sklearn.pipeline import Pipeline
from sklearn.compose import ColumnTransformer
from sklearn.preprocessing import OneHotEncoder, StandardScaler
from sklearn.multiclass import OneVsRestClassifier
from sklearn.ensemble import RandomForestClassifier
import machine_learning.utility as utility
import enums
import time
from datetime import datetime


# 로드 데이터
any_football_data = load_data(enums.SportsType.Football, enums.CampType.Any)

# 학습 데이터셋
train_X = any_football_data.drop(label_columns, axis=1)
train_Y = any_football_data['result_type']  # 경기 결과 (승=0, 무=1, 패=2)

# 특성 처리
feature_pipeline = ColumnTransformer([
        ("num", StandardScaler(), num_attribs),
        ("cat", OneHotEncoder(), cat_attribs_with_camp),
    ])

# 모델 훈련
prepared_train = feature_pipeline.fit_transform(train_X)
print(f'prepared_train data_set shape: {prepared_train.shape}')

start_prepared = time.time()

rtf_clf = OneVsRestClassifier(RandomForestClassifier(n_estimators=122, random_state=42, min_samples_split=4, class_weight='balanced'))
rtf_clf.fit(prepared_train, train_Y)

end_prepared = time.time()

print(f'elapsed time: {end_prepared - start_prepared} sec \n')

full_pipeline = Pipeline([
        ("preparation", feature_pipeline),
        ("predictor", rtf_clf)
    ])

# 저장
dir_name = datetime.today().strftime("rft_%Y%m%d-%H%M%S")
utility.save_sklearn_model(full_pipeline, dir_name, "rft_clf_result_any.pkl")
