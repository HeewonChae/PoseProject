# 예측 데이터 조회
set @start_time = '20200811';
set @end_time = '20200820';

SELECT f.id, l.country_name, l.name as league, ht.name as home_team, at.name as away_team, f.match_time, f.home_score, f.away_score,
p.main_label, p.sub_label, p.value1, p.value2, p.value3, p.grade, p.is_recommended, p.is_hit FROM footballdb.fixture as f
inner join footballdb.league as l on f.league_id = l.id
inner join footballdb.team as ht on f.home_team_id = ht.id
inner join footballdb.team as at on f.away_team_id = at.id
left join footballdb.prediction as p on f.id = p.fixture_id
where f.is_predicted = 1 
and  f.match_time between @start_time and @end_time;

-------------------------------------------------------------------------

# 추천 예측 데이터 적중 확률
set @start_time = '20200811';
set @end_time = '20200820';

SELECT f.id, l.country_name, l.name as league, ht.name as home_team, at.name as away_team, f.match_time, f.home_score, f.away_score,
p.main_label, p.sub_label, p.value1, p.value2, p.value3, p.grade, p.is_recommended, p.is_hit FROM footballdb.fixture as f
inner join footballdb.league as l on f.league_id = l.id
inner join footballdb.team as ht on f.home_team_id = ht.id
inner join footballdb.team as at on f.away_team_id = at.id
left join footballdb.prediction as p on f.id = p.fixture_id
where f.is_predicted = 1 
and  f.match_time between @start_time and @end_time
and f.is_completed = 1
and p.is_recommended = 1;

SELECT f.id, l.country_name, l.name as league, ht.name as home_team, at.name as away_team, f.match_time, f.home_score, f.away_score,
p.main_label, p.sub_label, p.value1, p.value2, p.value3, p.grade, p.is_recommended, p.is_hit FROM footballdb.fixture as f
inner join footballdb.league as l on f.league_id = l.id
inner join footballdb.team as ht on f.home_team_id = ht.id
inner join footballdb.team as at on f.away_team_id = at.id
left join footballdb.prediction as p on f.id = p.fixture_id
where f.is_predicted = 1 
and  f.match_time between @start_time and @end_time
and f.is_completed = 1
and p.is_recommended = 1
and p.is_hit = 1;

-------------------------------------------------------------------------

# 각 레이블 추천경기중 미적중 경기
set @start_time = '20200811';
set @end_time = '20200820';

SELECT f.id, l.country_name, l.name as league, ht.name as home_team, at.name as away_team, f.match_time, f.home_score, f.away_score,
p.main_label, p.sub_label, p.value1, p.value2, p.value3, p.grade, p.is_recommended, p.is_hit FROM footballdb.fixture as f
inner join footballdb.league as l on f.league_id = l.id
inner join footballdb.team as ht on f.home_team_id = ht.id
inner join footballdb.team as at on f.away_team_id = at.id
left join footballdb.prediction as p on f.id = p.fixture_id
where f.is_predicted = 1 
and  f.match_time between @start_time and @end_time
and f.is_completed = 1
and p.is_recommended = 1
and p.is_hit = 0
and main_label = 1
order by main_label, sub_label;

SELECT f.id, l.country_name, l.name as league, ht.name as home_team, at.name as away_team, f.match_time, f.home_score, f.away_score,
p.main_label, p.sub_label, p.value1, p.value2, p.value3, p.grade, p.is_recommended, p.is_hit FROM footballdb.fixture as f
inner join footballdb.league as l on f.league_id = l.id
inner join footballdb.team as ht on f.home_team_id = ht.id
inner join footballdb.team as at on f.away_team_id = at.id
left join footballdb.prediction as p on f.id = p.fixture_id
where f.is_predicted = 1 
and  f.match_time between @start_time and @end_time
and f.is_completed = 1
and p.is_recommended = 1
and p.is_hit = 0
and main_label = 2
order by main_label, sub_label;

SELECT f.id, l.country_name, l.name as league, ht.name as home_team, at.name as away_team, f.match_time, f.home_score, f.away_score,
p.main_label, p.sub_label, p.value1, p.value2, p.value3, p.grade, p.is_recommended, p.is_hit FROM footballdb.fixture as f
inner join footballdb.league as l on f.league_id = l.id
inner join footballdb.team as ht on f.home_team_id = ht.id
inner join footballdb.team as at on f.away_team_id = at.id
left join footballdb.prediction as p on f.id = p.fixture_id
where f.is_predicted = 1 
and  f.match_time between @start_time and @end_time
and f.is_completed = 1
and p.is_recommended = 1
and p.is_hit = 0
and main_label = 3
order by main_label, sub_label;

SELECT f.id, l.country_name, l.name as league, ht.name as home_team, at.name as away_team, f.match_time, f.home_score, f.away_score,
p.main_label, p.sub_label, p.value1, p.value2, p.value3, p.grade, p.is_recommended, p.is_hit FROM footballdb.fixture as f
inner join footballdb.league as l on f.league_id = l.id
inner join footballdb.team as ht on f.home_team_id = ht.id
inner join footballdb.team as at on f.away_team_id = at.id
left join footballdb.prediction as p on f.id = p.fixture_id
where f.is_predicted = 1 
and  f.match_time between @start_time and @end_time
and f.is_completed = 1
and p.is_recommended = 1
and p.is_hit = 0
and main_label = 4
order by main_label, sub_label;