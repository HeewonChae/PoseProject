import enums
import joblib
import os
from sklearn.base import BaseEstimator, TransformerMixin
import numpy as np


# Display K-Fold cross-validation Scores
def display_scores(scores):
    print("점수:", scores)
    print("평균:", scores.mean())
    print("표준 편차:", scores.std())


# 모델 저장
def save_sklearn_model(model, save_dir, model_name):
    abs_path = os.path.abspath(save_dir)

    if not (os.path.isdir(abs_path)):
        os.makedirs(abs_path)

    file_path = os.path.join(save_dir, model_name)
    joblib.dump(model, file_path)


# 모델 로드
def load_sklearn_model(model_name):
    root_dir = os.path.dirname(os.path.abspath(__file__))
    model_dir = os.path.join(root_dir, '../models/sklearn/')

    return joblib.load(os.path.join(model_dir, model_name))


# 각 팀의 휴식기간을 비닝(binning) 처리
class BinningRestPeriod(BaseEstimator, TransformerMixin):
    def __init__(self):  # *args 또는 **kargs 없음
        pass

    def fit(self, X, y=None):
        return self  # 아무것도 하지 않습니다

    def transform(self, X):
        X.loc[X.my_rest_period < 3, 'my_rest_period_cat'] = 0
        X.loc[(X.my_rest_period >= 3) & (X.my_rest_period < 7), 'my_rest_period_cat'] = 1
        X.loc[(X.my_rest_period >= 7) & (X.my_rest_period < 11), 'my_rest_period_cat'] = 2
        X.loc[(X.my_rest_period >= 11), 'my_rest_period_cat'] = 3

        X.loc[X.op_rest_period < 3, 'op_rest_period_cat'] = 0
        X.loc[(X.op_rest_period >= 3) & (X.op_rest_period < 7), 'op_rest_period_cat'] = 1
        X.loc[(X.op_rest_period >= 7) & (X.op_rest_period < 11), 'op_rest_period_cat'] = 2
        X.loc[(X.op_rest_period >= 11), 'op_rest_period_cat'] = 3
        return X


# 리그 타입 카테고라이즈
class LeagueTypeCategorize(BaseEstimator, TransformerMixin):
    def __init__(self):  # *args 또는 **kargs 없음
        pass

    def fit(self, X, y=None):
        return self  # 아무것도 하지 않습니다

    def transform(self, X):
        X.loc[X.league_type == 0, 'league_type_0'] = 1
        X.loc[X.league_type == 0, 'league_type_1'] = 0

        X.loc[X.league_type == 1, 'league_type_0'] = 0
        X.loc[X.league_type == 1, 'league_type_1'] = 1
        return np.c_[X.iloc[:, -2], X.iloc[:, -1]]
