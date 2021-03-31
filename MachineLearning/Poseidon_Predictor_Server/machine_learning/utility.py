import joblib
import os
from sklearn.base import BaseEstimator, TransformerMixin
import numpy as np


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


def load_sklearn_model(model_name):
    root_dir = os.path.dirname(os.path.abspath(__file__))
    model_dir = os.path.join(root_dir, 'football')

    return joblib.load(os.path.join(model_dir, model_name))


# score
knn_score_any = load_sklearn_model("score/knn_reg_score_any.pkl")
lin_score_any = load_sklearn_model("score/lin_reg_score_any.pkl")
# score final
fin_lin_score = load_sklearn_model("score/final/fin_lin_reg_score.pkl")

# result
knn_result_any = load_sklearn_model("result/knn_clf_result_any.pkl")
rft_result_any = load_sklearn_model("result/rft_clf_result_any.pkl")
sgd_result_any = load_sklearn_model("result/sgd_clf_result_any.pkl")
# result final
fin_knn_result = load_sklearn_model("result/final/fin_knn_clf_result.pkl")
# fin_sgd_result = load_sklearn_model("result/final/fin_sgd_clf_result.pkl")


# # both to score
knn_bts_any = load_sklearn_model("both_to_score/knn_clf_bts_any.pkl")
rft_bts_any = load_sklearn_model("both_to_score/rft_clf_bts_any.pkl")
sgd_bts_any = load_sklearn_model("both_to_score/sgd_clf_bts_any.pkl")
# both to score final
# fin_sgd_bts = load_sklearn_model("both_to_score/final/fin_sgd_clf_bts.pkl")
fin_knn_bts = load_sklearn_model("both_to_score/final/fin_knn_clf_bts.pkl")


# under over
knn_1_5_any = load_sklearn_model("under_over/knn_clf_1_5_any.pkl")
knn_2_5_any = load_sklearn_model("under_over/knn_clf_2_5_any.pkl")
knn_3_5_any = load_sklearn_model("under_over/knn_clf_3_5_any.pkl")
knn_4_5_any = load_sklearn_model("under_over/knn_clf_4_5_any.pkl")

rft_1_5_any = load_sklearn_model("under_over/rft_clf_1_5_any.pkl")
rft_2_5_any = load_sklearn_model("under_over/rft_clf_2_5_any.pkl")
rft_3_5_any = load_sklearn_model("under_over/rft_clf_3_5_any.pkl")
rft_4_5_any = load_sklearn_model("under_over/rft_clf_4_5_any.pkl")

sgd_1_5_any = load_sklearn_model("under_over/sgd_clf_1_5_any.pkl")
sgd_2_5_any = load_sklearn_model("under_over/sgd_clf_2_5_any.pkl")
sgd_3_5_any = load_sklearn_model("under_over/sgd_clf_3_5_any.pkl")
sgd_4_5_any = load_sklearn_model("under_over/sgd_clf_4_5_any.pkl")
# under over final
# fin_sgd_1_5 = load_sklearn_model("under_over/final/fin_sgd_clf_1_5.pkl")
# fin_sgd_2_5 = load_sklearn_model("under_over/final/fin_sgd_clf_2_5.pkl")
# fin_sgd_3_5 = load_sklearn_model("under_over/final/fin_sgd_clf_3_5.pkl")
# fin_sgd_4_5 = load_sklearn_model("under_over/final/fin_sgd_clf_4_5.pkl")

fin_knn_1_5 = load_sklearn_model("under_over/final/fin_knn_clf_1_5.pkl")
fin_knn_2_5 = load_sklearn_model("under_over/final/fin_knn_clf_2_5.pkl")
fin_knn_3_5 = load_sklearn_model("under_over/final/fin_knn_clf_3_5.pkl")
fin_knn_4_5 = load_sklearn_model("under_over/final/fin_knn_clf_4_5.pkl")
