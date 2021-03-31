import pandas as pd
import numpy as np
import enums
import configparser
import os
import glob


def get_football_data_dir():
    config = configparser.ConfigParser()
    root_dir = os.path.dirname(os.path.abspath(__file__))  # This is your Project Root
    config_path = os.path.join(root_dir, 'config.ini')  # requires `import os`
    config.read(config_path)
    save_dir_path = config['DATA_DIR_PATH']['FOOTBALL_DATA_PATH_V2']

    abs_path = os.path.join(root_dir, save_dir_path)
    if not (os.path.isdir(abs_path)):
        raise ValueError("Football data 디렉토리가 없습니다.")

    return abs_path


# football data 전부 합친 데이터 프레임 반환
def get_football_data():
    football_data_dir = get_football_data_dir()

    all_files = glob.glob(os.path.join(football_data_dir, '20*'))
    print(f"football file count: {len(all_files)}")

    all_csv_data = []
    for file in all_files:
        df = pd.read_csv(file, sep=',', na_values=np.nan)
        all_csv_data.append(df)

    # concat all football data
    football_data = pd.concat(all_csv_data, axis=0, ignore_index=True)
    print(f"football data shape: {football_data.shape}")

    return football_data


# null 값 체크
def is_exist_null_row(data):
    incomplete_rows = data[data.isnull().any(axis=1)]
    return len(incomplete_rows) != 0


# 해당 스포츠에 맞는 데이터 가져오기
def load_data(sports_type, camp_type):
    # 축구
    if sports_type is enums.SportsType.Football:
        data = get_football_data()
        if camp_type != enums.CampType.Any:
            data = data[data['camp_type'] == camp_type.value]

        data.drop('match_time', axis=1, inplace=True)

    if is_exist_null_row(data):
        raise ValueError("exist null value in data")

    return data


# 해당 캠프에 맞는 데이터만 가져오기
def get_camp_fixtures(camp_type, fixtures):
    data = fixtures[fixtures['camp_type'] == camp_type.value]

    return data


num_attribs = ['my_camp_3matches_gf_avg', 'my_camp_3matches_ga_avg',
               'my_6matches_gf_avg', 'my_6matches_ga_avg',
               'my_camp_3matches_pts', 'my_6matches_pts',
               'op_camp_3matches_gf_avg', 'op_camp_3matches_ga_avg',
               'op_6matches_gf_avg', 'op_6matches_ga_avg',
               'op_camp_3matches_pts', 'op_6matches_pts',
               'my_att_trend', 'my_def_trend',
               'op_att_trend', 'op_def_trend',
               'h2h_4matches_gf_avg', 'h2h_4matches_ga_avg',
               'h2h_4matches_pts',
               'my_rest_period', 'op_rest_period']  # 21컬럼

cat_attribs = ['league_type']

cat_attribs_with_camp = ['league_type', 'camp_type']  # 4컬럼

cat_attribs_with_rest = ['league_type', 'my_rest_period_cat', 'op_rest_period_cat']

cat_attribs_with_rest_camp = ['league_type', 'my_rest_period_cat', 'op_rest_period_cat', 'camp_type']  # 4컬럼
