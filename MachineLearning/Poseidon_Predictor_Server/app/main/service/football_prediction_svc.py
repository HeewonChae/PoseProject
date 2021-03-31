import machine_learning.utility as ml
from app.main.model.score import Score
from app.main.model.proba_winner import ProbaWinner
from app.main.model.proba_yn import ProbaYN
from app.main.model.football.football_uner_over import FootballUnderOver
from app.main.model.football.football_clf import FootballCLF
import numpy as np
import math


# 두 값의 조화 평균
def get_2_items_harmonic_mean(value1, value2):
    denominator = value1 + value2
    if denominator == 0:
        return 0

    return_value = 2 * (value1 * value2) / denominator

    if return_value == 0:
        return value1 + value2 / 2

    return return_value


# 세 값의 조화 평균
def get_3_items_harmonic_mean(value1, value2, value3):
    denominator = (value1*value2) + (value1*value3) + (value2*value3)
    if denominator == 0:
        return 0

    return_value = 3 * (value1 * value2 * value3) / denominator

    if return_value == 0:
        return value1 + value2 + value3 / 3

    return return_value


# 두 값의 기하 평균
def get_geometric_mean(value1, value2):
    return math.sqrt(value1 + value2)


# 두 확률 리스트의 조화 평균
def get_2_items_proba_harmonic_mean(proba1, proba2):
    if len(proba1) != len(proba2):
        raise ValueError("proba list not equal lenth")
    
    mean_list = []
    for idx in range(len(proba1)):
        harmonic_mean = get_2_items_harmonic_mean(proba1[idx], proba2[idx])
        mean_list.append(harmonic_mean)

    return mean_list


# 세 확률 리스트의 조화 평균
def get_3_items_proba_harmonic_mean(proba1, proba2, proba3):
    if len(proba1) != len(proba2) or len(proba1) != len(proba3):
        raise ValueError("proba list not equal lenth")
    
    mean_list = []
    for idx in range(len(proba1)):
        harmonic_mean = get_3_items_harmonic_mean(proba1[idx], proba2[idx], proba3[idx])
        mean_list.append(harmonic_mean)

    return mean_list


# 두 확률 리스트의 산술 평균
def get_2_items_proba_arithmetic_mean(proba1, proba2):
    if len(proba1) != len(proba2):
        raise ValueError("proba list not equal lenth")

    sum_list = []
    for idx in range(len(proba1)):
        sum_list.append(proba1[idx] + proba2[idx])

    sum_proba = sum(sum_list)
    return sum_list / sum_proba


# 세 확률 리스트의 산술 평균
def get_3_items_proba_arithmetic_mean(proba1, proba2, proba3):
    if len(proba1) != len(proba2):
        raise ValueError("proba list not equal lenth")

    sum_list = []
    for idx in range(len(proba1)):
        sum_list.append(proba1[idx] + proba2[idx] + proba3[idx])

    sum_proba = sum(sum_list)
    return sum_list / sum_proba


# 홈,원정 스코어 예측
def predict_score(home_data, away_data):
    knn_score_home = ml.knn_score_any.predict(home_data)
    knn_score_away = ml.knn_score_any.predict(away_data)
    lin_score_home = ml.lin_score_any.predict(home_data)
    lin_score_away = ml.lin_score_any.predict(away_data)

    home_camp_features = np.c_[np.ones(1), np.zeros(1),
                               knn_score_home, lin_score_home]
    away_camp_features = np.c_[np.zeros(1), np.ones(1),
                               knn_score_away, lin_score_away]

    # fin
    fin_lin_home_score = ml.fin_lin_score.predict(home_camp_features).flatten()
    fin_lin_away_score = ml.fin_lin_score.predict(away_camp_features).flatten()

    football_score = Score()
    football_score.SetLinHomeScore(fin_lin_home_score[0])
    football_score.SetLinAwayScore(fin_lin_away_score[0])

    return football_score


def predict_match_winner(home_data, away_data):
    knn_result_home = ml.knn_result_any.predict_proba(home_data)
    knn_result_away = ml.knn_result_any.predict_proba(away_data)
    rft_result_home = ml.rft_result_any.predict_proba(home_data)
    rft_result_away = ml.rft_result_any.predict_proba(away_data)
    sgd_result_home = ml.sgd_result_any.predict_proba(home_data)
    sgd_result_away = ml.sgd_result_any.predict_proba(away_data)

    home_camp_features = np.c_[np.ones(1), np.zeros(1),
                               knn_result_home, rft_result_home, sgd_result_home]
    away_camp_features = np.c_[np.zeros(1), np.ones(1),
                               knn_result_away, rft_result_away, sgd_result_away]

    # fin result
    fin_knn_result_home = ml.fin_knn_result.predict_proba(home_camp_features)
    fin_knn_result_away = ml.fin_knn_result.predict_proba(away_camp_features)
    fin_knn_result = get_2_items_proba_arithmetic_mean(fin_knn_result_home.flatten(),
                                                     np.flip(fin_knn_result_away.flatten()))

    # fin_sgd_result_home = ml.fin_sgd_result.predict_proba(home_camp_features)
    # fin_sgd_result_away = ml.fin_sgd_result.predict_proba(away_camp_features)
    # fin_sgd_result = get_2_items_proba_arithmetic_mean(fin_sgd_result_home.flatten(),
    #                                                  np.flip(fin_sgd_result_away.flatten()))

    # Sub
    knn_result = get_2_items_proba_harmonic_mean(knn_result_home.flatten(), np.flip(knn_result_away.flatten()))
    rft_result = get_2_items_proba_harmonic_mean(rft_result_home.flatten(), np.flip(rft_result_away.flatten()))
    sgd_result = get_2_items_proba_harmonic_mean(sgd_result_home.flatten(), np.flip(sgd_result_away.flatten()))
    sub_result_proba = get_3_items_proba_harmonic_mean(knn_result, rft_result, sgd_result)

    knn_proba = ProbaWinner()
    knn_proba.SetWinProba(fin_knn_result[0])
    knn_proba.SetDrawProba(fin_knn_result[1])
    knn_proba.SetLoseProba(fin_knn_result[2])

    # sgd_proba = ProbaWinner()
    # sgd_proba.SetWinProba(fin_sgd_result[0])
    # sgd_proba.SetDrawProba(fin_sgd_result[1])
    # sgd_proba.SetLoseProba(fin_sgd_result[2])

    sub_proba = ProbaWinner()
    sub_proba.SetWinProba(sub_result_proba[0])
    sub_proba.SetDrawProba(sub_result_proba[1])
    sub_proba.SetLoseProba(sub_result_proba[2])

    match_winner = FootballCLF()
    match_winner.setKNN(knn_proba)
    # match_winner.setSGD(sgd_proba)
    match_winner.setSub(sub_proba)

    return match_winner


def predict_both_to_score(home_data, away_data):
    knn_bts_home = ml.knn_bts_any.predict_proba(home_data)
    knn_bts_away = ml.knn_bts_any.predict_proba(away_data)
    rft_bts_home = ml.rft_bts_any.predict_proba(home_data)
    rft_bts_away = ml.rft_bts_any.predict_proba(away_data)
    sgd_bts_home = ml.sgd_bts_any.predict_proba(home_data)
    sgd_bts_away = ml.sgd_bts_any.predict_proba(away_data)

    home_camp_features = np.c_[np.ones(1), np.zeros(1),
                               knn_bts_home, rft_bts_home, sgd_bts_home]
    away_camp_features = np.c_[np.zeros(1), np.ones(1),
                               knn_bts_away, rft_bts_away, sgd_bts_away]

    # fin both teams to score
    fin_knn_bts_home = ml.fin_knn_bts.predict_proba(home_camp_features)
    fin_knn_bts_away = ml.fin_knn_bts.predict_proba(away_camp_features)
    fin_knn_bts = get_2_items_proba_arithmetic_mean(fin_knn_bts_home.flatten(), fin_knn_bts_away.flatten())
    # fin_sgd_bts_home = ml.fin_sgd_bts.predict_proba(home_camp_features)
    # fin_sgd_bts_away = ml.fin_sgd_bts.predict_proba(away_camp_features)
    # fin_sgd_bts = get_2_items_proba_arithmetic_mean(fin_sgd_bts_home.flatten(), fin_sgd_bts_away.flatten())

    # Sub
    knn_bts = get_2_items_proba_harmonic_mean(knn_bts_home.flatten(), knn_bts_away.flatten())
    rft_bts = get_2_items_proba_harmonic_mean(rft_bts_home.flatten(), rft_bts_away.flatten())
    sgd_bts = get_2_items_proba_harmonic_mean(sgd_bts_home.flatten(), sgd_bts_away.flatten())
    sub_bts_proba = get_3_items_proba_harmonic_mean(knn_bts, rft_bts, sgd_bts)

    knn_proba = ProbaYN()
    knn_proba.SetNo(fin_knn_bts[0])
    knn_proba.SetYes(fin_knn_bts[1])

    # sgd_proba = ProbaYN()
    # sgd_proba.SetNo(fin_sgd_bts[0])
    # sgd_proba.SetYes(fin_sgd_bts[1])

    sub_proba = ProbaYN()
    sub_proba.SetNo(sub_bts_proba[0])
    sub_proba.SetYes(sub_bts_proba[1])

    both_to_score = FootballCLF()
    both_to_score.setKNN(knn_proba)
    # both_to_score.setSGD(sgd_proba)
    both_to_score.setSub(sub_proba)

    return both_to_score


def predict_under_over(home_data, away_data):
    # 1.5
    knn_uo_home = ml.knn_1_5_any.predict_proba(home_data)
    knn_uo_away = ml.knn_1_5_any.predict_proba(away_data)
    rft_uo_home = ml.rft_1_5_any.predict_proba(home_data)
    rft_uo_away = ml.rft_1_5_any.predict_proba(away_data)
    sgd_uo_home = ml.sgd_1_5_any.predict_proba(home_data)
    sgd_uo_away = ml.sgd_1_5_any.predict_proba(away_data)

    home_camp_features = np.c_[np.ones(1), np.zeros(1),
                               knn_uo_home, rft_uo_home, sgd_uo_home]
    away_camp_features = np.c_[np.zeros(1), np.ones(1),
                               knn_uo_away, rft_uo_away, sgd_uo_away]

    # fin 1.5
    fin_knn_uo_home = ml.fin_knn_1_5.predict_proba(home_camp_features)
    fin_knn_uo_away = ml.fin_knn_1_5.predict_proba(away_camp_features)
    fin_knn_uo = get_2_items_proba_arithmetic_mean(fin_knn_uo_home.flatten(), fin_knn_uo_away.flatten())

    # fin_sgd_uo_home = ml.fin_sgd_1_5.predict_proba(home_camp_features)
    # fin_sgd_uo_away = ml.fin_sgd_1_5.predict_proba(away_camp_features)
    # fin_sgd_uo = get_2_items_proba_arithmetic_mean(fin_sgd_uo_home.flatten(), fin_sgd_uo_away.flatten())

    # sub
    knn_uo = get_2_items_proba_harmonic_mean(knn_uo_home.flatten(), knn_uo_away.flatten())
    rft_uo = get_2_items_proba_harmonic_mean(rft_uo_home.flatten(), rft_uo_away.flatten())
    sgd_uo = get_2_items_proba_harmonic_mean(sgd_uo_home.flatten(), sgd_uo_away.flatten())
    sub_uo_proba = get_3_items_proba_harmonic_mean(knn_uo, rft_uo, sgd_uo)

    knn_proba = ProbaYN()
    knn_proba.SetNo(fin_knn_uo[0])
    knn_proba.SetYes(fin_knn_uo[1])

    # sgd_proba = ProbaYN()
    # sgd_proba.SetNo(fin_sgd_uo[0])
    # sgd_proba.SetYes(fin_sgd_uo[1])

    sub_proba = ProbaYN()
    sub_proba.SetNo(sub_uo_proba[0])
    sub_proba.SetYes(sub_uo_proba[1])

    under_over_1_5 = FootballCLF()
    under_over_1_5.setKNN(knn_proba)
    # under_over_1_5.setSGD(sgd_proba)
    under_over_1_5.setSub(sub_proba)

    # 2.5
    knn_uo_home = ml.knn_2_5_any.predict_proba(home_data)
    knn_uo_away = ml.knn_2_5_any.predict_proba(away_data)
    rft_uo_home = ml.rft_2_5_any.predict_proba(home_data)
    rft_uo_away = ml.rft_2_5_any.predict_proba(away_data)
    sgd_uo_home = ml.sgd_2_5_any.predict_proba(home_data)
    sgd_uo_away = ml.sgd_2_5_any.predict_proba(away_data)

    home_camp_features = np.c_[np.ones(1), np.zeros(1),
                               knn_uo_home, rft_uo_home, sgd_uo_home]
    away_camp_features = np.c_[np.zeros(1), np.ones(1),
                               knn_uo_away, rft_uo_away, sgd_uo_away]

    # fin 2.5
    fin_knn_uo_home = ml.fin_knn_2_5.predict_proba(home_camp_features)
    fin_knn_uo_away = ml.fin_knn_2_5.predict_proba(away_camp_features)
    fin_knn_uo = get_2_items_proba_arithmetic_mean(fin_knn_uo_home.flatten(), fin_knn_uo_away.flatten())

    # fin_sgd_uo_home = ml.fin_sgd_2_5.predict_proba(home_camp_features)
    # fin_sgd_uo_away = ml.fin_sgd_2_5.predict_proba(away_camp_features)
    # fin_sgd_uo = get_2_items_proba_arithmetic_mean(fin_sgd_uo_home.flatten(), fin_sgd_uo_away.flatten())

    # sub
    knn_uo = get_2_items_proba_harmonic_mean(knn_uo_home.flatten(), knn_uo_away.flatten())
    rft_uo = get_2_items_proba_harmonic_mean(rft_uo_home.flatten(), rft_uo_away.flatten())
    sgd_uo = get_2_items_proba_harmonic_mean(sgd_uo_home.flatten(), sgd_uo_away.flatten())
    sub_uo_proba = get_3_items_proba_harmonic_mean(knn_uo, rft_uo, sgd_uo)

    knn_proba = ProbaYN()
    knn_proba.SetNo(fin_knn_uo[0])
    knn_proba.SetYes(fin_knn_uo[1])

    # sgd_proba = ProbaYN()
    # sgd_proba.SetNo(fin_sgd_uo[0])
    # sgd_proba.SetYes(fin_sgd_uo[1])

    sub_proba = ProbaYN()
    sub_proba.SetNo(sub_uo_proba[0])
    sub_proba.SetYes(sub_uo_proba[1])

    under_over_2_5 = FootballCLF()
    under_over_2_5.setKNN(knn_proba)
    # under_over_2_5.setSGD(sgd_proba)
    under_over_2_5.setSub(sub_proba)

    # 3.5
    knn_uo_home = ml.knn_3_5_any.predict_proba(home_data)
    knn_uo_away = ml.knn_3_5_any.predict_proba(away_data)
    rft_uo_home = ml.rft_3_5_any.predict_proba(home_data)
    rft_uo_away = ml.rft_3_5_any.predict_proba(away_data)
    sgd_uo_home = ml.sgd_3_5_any.predict_proba(home_data)
    sgd_uo_away = ml.sgd_3_5_any.predict_proba(away_data)

    home_camp_features = np.c_[np.ones(1), np.zeros(1),
                               knn_uo_home, rft_uo_home, sgd_uo_home]
    away_camp_features = np.c_[np.zeros(1), np.ones(1),
                               knn_uo_away, rft_uo_away, sgd_uo_away]

    # fin 3.5
    fin_knn_uo_home = ml.fin_knn_3_5.predict_proba(home_camp_features)
    fin_knn_uo_away = ml.fin_knn_3_5.predict_proba(away_camp_features)
    fin_knn_uo = get_2_items_proba_arithmetic_mean(fin_knn_uo_home.flatten(), fin_knn_uo_away.flatten())

    # fin_sgd_uo_home = ml.fin_sgd_3_5.predict_proba(home_camp_features)
    # fin_sgd_uo_away = ml.fin_sgd_3_5.predict_proba(away_camp_features)
    # fin_sgd_uo = get_2_items_proba_arithmetic_mean(fin_sgd_uo_home.flatten(), fin_sgd_uo_away.flatten())

    # sub
    knn_uo = get_2_items_proba_harmonic_mean(knn_uo_home.flatten(), knn_uo_away.flatten())
    rft_uo = get_2_items_proba_harmonic_mean(rft_uo_home.flatten(), rft_uo_away.flatten())
    sgd_uo = get_2_items_proba_harmonic_mean(sgd_uo_home.flatten(), sgd_uo_away.flatten())
    sub_uo_proba = get_3_items_proba_harmonic_mean(knn_uo, rft_uo, sgd_uo)

    knn_proba = ProbaYN()
    knn_proba.SetNo(fin_knn_uo[0])
    knn_proba.SetYes(fin_knn_uo[1])

    # sgd_proba = ProbaYN()
    # sgd_proba.SetNo(fin_sgd_uo[0])
    # sgd_proba.SetYes(fin_sgd_uo[1])

    sub_proba = ProbaYN()
    sub_proba.SetNo(sub_uo_proba[0])
    sub_proba.SetYes(sub_uo_proba[1])

    under_over_3_5 = FootballCLF()
    under_over_3_5.setKNN(knn_proba)
    # under_over_3_5.setSGD(sgd_proba)
    under_over_3_5.setSub(sub_proba)

    # 4.5
    knn_uo_home = ml.knn_4_5_any.predict_proba(home_data)
    knn_uo_away = ml.knn_4_5_any.predict_proba(away_data)
    rft_uo_home = ml.rft_4_5_any.predict_proba(home_data)
    rft_uo_away = ml.rft_4_5_any.predict_proba(away_data)
    sgd_uo_home = ml.sgd_4_5_any.predict_proba(home_data)
    sgd_uo_away = ml.sgd_4_5_any.predict_proba(away_data)

    home_camp_features = np.c_[np.ones(1), np.zeros(1),
                               knn_uo_home, rft_uo_home, sgd_uo_home]
    away_camp_features = np.c_[np.zeros(1), np.ones(1),
                               knn_uo_away, rft_uo_away, sgd_uo_away]

    # fin 4.5
    fin_knn_uo_home = ml.fin_knn_4_5.predict_proba(home_camp_features)
    fin_knn_uo_away = ml.fin_knn_4_5.predict_proba(away_camp_features)
    fin_knn_uo = get_2_items_proba_arithmetic_mean(fin_knn_uo_home.flatten(), fin_knn_uo_away.flatten())

    # fin_sgd_uo_home = ml.fin_sgd_4_5.predict_proba(home_camp_features)
    # fin_sgd_uo_away = ml.fin_sgd_4_5.predict_proba(away_camp_features)
    # fin_sgd_uo = get_2_items_proba_arithmetic_mean(fin_sgd_uo_home.flatten(), fin_sgd_uo_away.flatten())

    # sub
    knn_uo = get_2_items_proba_harmonic_mean(knn_uo_home.flatten(), knn_uo_away.flatten())
    rft_uo = get_2_items_proba_harmonic_mean(rft_uo_home.flatten(), rft_uo_away.flatten())
    sgd_uo = get_2_items_proba_harmonic_mean(sgd_uo_home.flatten(), sgd_uo_away.flatten())
    sub_uo_proba = get_3_items_proba_harmonic_mean(knn_uo, rft_uo, sgd_uo)

    knn_proba = ProbaYN()
    knn_proba.SetNo(fin_knn_uo[0])
    knn_proba.SetYes(fin_knn_uo[1])

    # sgd_proba = ProbaYN()
    # sgd_proba.SetNo(fin_sgd_uo[0])
    # sgd_proba.SetYes(fin_sgd_uo[1])

    sub_proba = ProbaYN()
    sub_proba.SetNo(sub_uo_proba[0])
    sub_proba.SetYes(sub_uo_proba[1])

    under_over_4_5 = FootballCLF()
    under_over_4_5.setKNN(knn_proba)
    # under_over_4_5.setSGD(sgd_proba)
    under_over_4_5.setSub(sub_proba)

    # final
    football_uo = FootballUnderOver()
    football_uo.SetOver_1_5(under_over_1_5)
    football_uo.SetOver_2_5(under_over_2_5)
    football_uo.SetOver_3_5(under_over_3_5)
    football_uo.SetOver_4_5(under_over_4_5)

    return football_uo
