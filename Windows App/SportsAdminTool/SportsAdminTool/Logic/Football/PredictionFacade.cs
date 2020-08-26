using LogicCore.Converter;
using PosePacket.Service.Enum;
using PosePacket.Service.Football.Models.Enums;
using PredictorAPI.Models;
using PredictorAPI.Models.Football;
using SportsAdminTool.Model.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballTable = Repository.Mysql.FootballDB.Tables;

namespace SportsAdminTool.Logic.Football
{
    public static class PredictionFacade
    {
        private static readonly double Score_std_dev = 0.6;

        private static readonly int Winner_Proba_Criteria = 55;
        private static readonly int YN_Proba_Criteria = 50;

        public static List<FootballTable.Prediction> PredictFinalScore(int fixtureId, FootballPrediction data)
        {
            var homeScoreSet = GetAvailableScoreSet(data.MatchScore.HomeScore, data.HomeStat.AttTrend, data.AwayStat.DefTrend);
            var awayScoreSet = GetAvailableScoreSet(data.MatchScore.AwayScore, data.AwayStat.AttTrend, data.HomeStat.DefTrend);
            var matchWinnerType = GetMatchWinner(data.MatchWinner, out double proba);

            if (matchWinnerType == MatchResultType.Win)
                homeScoreSet.Reverse();

            if (matchWinnerType == MatchResultType.Lose)
                awayScoreSet.Reverse();

            var homeScores = homeScoreSet.Take(2).ToArray();
            Array.Sort(homeScores);

            var awayScores = awayScoreSet.Take(2).ToArray();
            Array.Sort(awayScores);

            // 예측
            List<FootballTable.Prediction> predictions = new List<FootballTable.Prediction>();
            predictions.Add(new FootballTable.Prediction
            {
                fixture_id = fixtureId,
                pred_seq = 1,
                main_label = (short)FootballPredictionType.Final_Score,
                sub_label = (short)TeamCampType.Home,
                value1 = homeScores[0],
                value2 = homeScores.Length > 1 ? homeScores[1] : 0,
            });

            predictions.Add(new FootballTable.Prediction
            {
                fixture_id = fixtureId,
                pred_seq = 2,
                main_label = (short)FootballPredictionType.Final_Score,
                sub_label = (short)TeamCampType.Away,
                value1 = awayScores[0],
                value2 = awayScores.Length > 1 ? awayScores[1] : 0,
            });

            return predictions;
        }

        public static List<FootballTable.Prediction> PredictMatchWinner(int fixtureId, FootballPrediction data)
        {
            int[] meanProbas = GetWinnerMeanProbas(data.MatchWinner.KNN, data.MatchWinner.SGD, data.MatchWinner.Sub);
#if DEBUG
            var homeScoreSet = GetAvailableScoreSet(data.MatchScore.HomeScore, data.HomeStat.AttTrend, data.AwayStat.DefTrend);
            var awayScoreSet = GetAvailableScoreSet(data.MatchScore.AwayScore, data.AwayStat.AttTrend, data.HomeStat.DefTrend);
            double meanHomeScore = homeScoreSet.Average();
            double meanAwayScore = awayScoreSet.Average();
#endif

            List<FootballTable.Prediction> predictions = new List<FootballTable.Prediction>();

            var win_grade = GetMatchWinnerGrade(data, FootballMatchWinnerType.Win, out bool isRecommend_win);
            var lose_grade = GetMatchWinnerGrade(data, FootballMatchWinnerType.Lose, out bool isRecommend_lose);

            if (win_grade >= lose_grade)
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 3,
                    main_label = (short)FootballPredictionType.Match_Winner,
                    sub_label = (short)FootballMatchWinnerType.Win,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    value3 = meanProbas[2],
                    grade = (short)win_grade,
                    is_recommended = isRecommend_win,
                });
            }
            else
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 3,
                    main_label = (short)FootballPredictionType.Match_Winner,
                    sub_label = (short)FootballMatchWinnerType.Lose,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    value3 = meanProbas[2],
                    grade = (short)lose_grade,
                    is_recommended = isRecommend_lose,
                });
            }

            var grade_wOd = GetMatchWinnerGrade(data, FootballMatchWinnerType.WinOrDraw, out bool isRecommend_wOd);
            var grade_dOl = GetMatchWinnerGrade(data, FootballMatchWinnerType.DrawOrLose, out bool isRecommend_dOl);
            //var grade_wOl = GetMatchWinnerGrade(data, FootballMatchWinnerType.WinOrLose, out bool isRecommend_wOl);

            if (grade_wOd >= grade_dOl)
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 4,
                    main_label = (short)FootballPredictionType.Match_Winner,
                    sub_label = (short)FootballMatchWinnerType.WinOrDraw,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    value3 = meanProbas[2],
                    grade = (short)grade_wOd,
                    is_recommended = isRecommend_wOd,
                });
            }
            else //if (grade_dOl >= grade_wOd && grade_dOl >= grade_wOl)
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 4,
                    main_label = (short)FootballPredictionType.Match_Winner,
                    sub_label = (short)FootballMatchWinnerType.DrawOrLose,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    value3 = meanProbas[2],
                    grade = (short)grade_dOl,
                    is_recommended = isRecommend_dOl,
                });
            }
            //else
            //{
            //    predictions.Add(new FootballTable.Prediction()
            //    {
            //        fixture_id = fixtureId,
            //        pred_seq = 4,
            //        main_label = (short)FootballPredictionType.Match_Winner,
            //        sub_label = (short)FootballMatchWinnerType.WinOrLose,
            //        value1 = meanProbas[0],
            //        value2 = meanProbas[1],
            //        value3 = meanProbas[2],
            //        grade = (short)grade_wOl,
            //        is_recommended = isRecommend_wOl,
            //    });
            //}

            return predictions;
        }

        public static List<FootballTable.Prediction> PredictBothToScore(int fixtureId, FootballPrediction data)
        {
            int[] meanProbas = GetYNMeanProbas(data.BothToScore.SGD, data.BothToScore.Sub);

            List<FootballTable.Prediction> predictions = new List<FootballTable.Prediction>();

            var yes_grade = GetBothToScoreGrade(data, YesNoType.Yes, out bool isRecommend_yes);
            var no_grade = GetBothToScoreGrade(data, YesNoType.No, out bool isRecommend_no);

            if (yes_grade >= no_grade)
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 5,
                    main_label = (short)FootballPredictionType.Both_Teams_to_Score,
                    sub_label = (short)YesNoType.Yes,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)yes_grade,
                    is_recommended = isRecommend_yes,
                });
            }
            else
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 5,
                    main_label = (short)FootballPredictionType.Both_Teams_to_Score,
                    sub_label = (short)YesNoType.No,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)no_grade,
                    is_recommended = isRecommend_no,
                });
            }

            return predictions;
        }

        public static List<FootballTable.Prediction> PredictUnderOver(int fixtureId, FootballPrediction data)
        {
            List<FootballTable.Prediction> predictions = new List<FootballTable.Prediction>();

            // 1.5 under over
            int[] meanProbas = GetYNMeanProbas(data.UnderOver.UO_1_5.SGD, data.UnderOver.UO_1_5.Sub);
            var under_grade = GetUnderOverGrade(data, FootballUnderOverType.UNDER_1_5, out bool isRecommend_under);
            var over_grade = GetUnderOverGrade(data, FootballUnderOverType.OVER_1_5, out bool isRecommend_over);

            if (under_grade > over_grade)
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 6,
                    main_label = (short)FootballPredictionType.Under_Over,
                    sub_label = (short)FootballUnderOverType.UNDER_1_5,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)under_grade,
                    is_recommended = isRecommend_under,
                });
            }
            else
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 6,
                    main_label = (short)FootballPredictionType.Under_Over,
                    sub_label = (short)FootballUnderOverType.OVER_1_5,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)over_grade,
                    is_recommended = isRecommend_over,
                });
            }

            // 2.5 under over
            meanProbas = GetYNMeanProbas(data.UnderOver.UO_2_5.SGD, data.UnderOver.UO_2_5.Sub);
            under_grade = GetUnderOverGrade(data, FootballUnderOverType.UNDER_2_5, out isRecommend_under);
            over_grade = GetUnderOverGrade(data, FootballUnderOverType.OVER_2_5, out isRecommend_over);

            if (under_grade > over_grade)
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 7,
                    main_label = (short)FootballPredictionType.Under_Over,
                    sub_label = (short)FootballUnderOverType.UNDER_2_5,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)under_grade,
                    is_recommended = isRecommend_under,
                });
            }
            else
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 7,
                    main_label = (short)FootballPredictionType.Under_Over,
                    sub_label = (short)FootballUnderOverType.OVER_2_5,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)over_grade,
                    is_recommended = isRecommend_over,
                });
            }

            // 3.5 under over
            meanProbas = GetYNMeanProbas(data.UnderOver.UO_3_5.SGD, data.UnderOver.UO_3_5.Sub);
            under_grade = GetUnderOverGrade(data, FootballUnderOverType.UNDER_3_5, out isRecommend_under);
            over_grade = GetUnderOverGrade(data, FootballUnderOverType.OVER_3_5, out isRecommend_over);

            if (under_grade >= over_grade)
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 8,
                    main_label = (short)FootballPredictionType.Under_Over,
                    sub_label = (short)FootballUnderOverType.UNDER_3_5,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)under_grade,
                    is_recommended = isRecommend_under,
                });
            }
            else
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 8,
                    main_label = (short)FootballPredictionType.Under_Over,
                    sub_label = (short)FootballUnderOverType.OVER_3_5,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)over_grade,
                    is_recommended = isRecommend_over,
                });
            }

            // 4.5 under over
            meanProbas = GetYNMeanProbas(data.UnderOver.UO_4_5.SGD, data.UnderOver.UO_4_5.Sub);
            under_grade = GetUnderOverGrade(data, FootballUnderOverType.UNDER_4_5, out isRecommend_under);
            over_grade = GetUnderOverGrade(data, FootballUnderOverType.OVER_4_5, out isRecommend_over);

            if (under_grade >= over_grade)
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 9,
                    main_label = (short)FootballPredictionType.Under_Over,
                    sub_label = (short)FootballUnderOverType.UNDER_4_5,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)under_grade,
                    is_recommended = isRecommend_under,
                });
            }
            else
            {
                predictions.Add(new FootballTable.Prediction()
                {
                    fixture_id = fixtureId,
                    pred_seq = 9,
                    main_label = (short)FootballPredictionType.Under_Over,
                    sub_label = (short)FootballUnderOverType.OVER_4_5,
                    value1 = meanProbas[0],
                    value2 = meanProbas[1],
                    grade = (short)over_grade,
                    is_recommended = isRecommend_over,
                });
            }

            return predictions;
        }

        public static bool DiscernMatchWinner(FootballMatchWinnerType subType, int homeScore, int awayScore)
        {
            switch (subType)
            {
                case FootballMatchWinnerType.Win:
                    return homeScore > awayScore;

                case FootballMatchWinnerType.Lose:
                    return awayScore > homeScore;

                case FootballMatchWinnerType.WinOrDraw:
                    return homeScore >= awayScore;

                case FootballMatchWinnerType.WinOrLose:
                    return homeScore != awayScore;

                case FootballMatchWinnerType.DrawOrLose:
                    return awayScore >= homeScore;

                default:
                    break;
            }

            return false;
        }

        public static bool DiscernBothToScore(YesNoType subType, int homeScore, int awayScore)
        {
            switch (subType)
            {
                case YesNoType.Yes:
                    return homeScore > 0 && awayScore > 0;

                case YesNoType.No:
                    return homeScore == 0 || awayScore == 0;

                default:
                    break;
            }

            return false;
        }

        public static bool DiscernUnderOver(FootballUnderOverType subType, int homeScore, int awayScore)
        {
            switch (subType)
            {
                case FootballUnderOverType.UNDER_1_5:
                    return (homeScore + awayScore) < 2;

                case FootballUnderOverType.OVER_1_5:
                    return (homeScore + awayScore) > 1;

                case FootballUnderOverType.UNDER_2_5:
                    return (homeScore + awayScore) < 3;

                case FootballUnderOverType.OVER_2_5:
                    return (homeScore + awayScore) > 2;

                case FootballUnderOverType.UNDER_3_5:
                    return (homeScore + awayScore) < 4;

                case FootballUnderOverType.OVER_3_5:
                    return (homeScore + awayScore) > 3;

                case FootballUnderOverType.UNDER_4_5:
                    return (homeScore + awayScore) < 5;

                case FootballUnderOverType.OVER_4_5:
                    return (homeScore + awayScore) > 4;

                default:
                    break;
            }

            return false;
        }

        public static string MakePredictionNotificationMessage(FootballTable.Fixture fixture, FootballTable.Prediction prediction)
        {
            var db_league = Logic.Database.FootballDBFacade.SelectLeagues(where: $"id = {fixture.league_id} ").FirstOrDefault();
            var db_homeTeam = Logic.Database.FootballDBFacade.SelectTeams(where: $"id = {fixture.home_team_id}").FirstOrDefault();
            var db_awayTeam = Logic.Database.FootballDBFacade.SelectTeams(where: $"id = {fixture.away_team_id}").FirstOrDefault();

            var country = db_league?.country_name ?? "Undefined";
            var league = db_league?.name ?? "Undefined";
            var homeTeam = db_homeTeam?.name ?? "Undefined";
            var awayTeam = db_awayTeam?.name ?? "Undefined";
            var matchTime = fixture.match_time.ToLocalTime().ToString("yyyy.MM.dd HH:MM");

            string message = string.Empty;
            ((int)prediction.main_label).TryParseEnum(out FootballPredictionType predictionType);
            switch (predictionType)
            {
                case FootballPredictionType.Match_Winner:
                    {
                        ((int)prediction.sub_label).TryParseEnum(out FootballMatchWinnerType matchWinnerSubType);
                        string subTypeString = ConvertFootballMatchWinnerTypeToString(matchWinnerSubType);
                        string grade = ConvertGradeToStartMark(prediction.grade);
                        message = $"[ {matchTime} ] [ {country} - {league} ] " +
                            $"[ {homeTeam} VS {awayTeam} ] " +
                            $"[ 승무패 - {subTypeString} ] " +
                            $"[ 승: {prediction.value1}%, 무: {prediction.value2}%, 패: {prediction.value3}% ] " +
                            $"{grade}";
                    }
                    break;

                case FootballPredictionType.Both_Teams_to_Score:
                    {
                        ((int)prediction.sub_label).TryParseEnum(out YesNoType bothToScoreSubType);
                        string grade = ConvertGradeToStartMark(prediction.grade);
                        message = $"[ {matchTime} ] [ {country} - {league} ] " +
                            $"[ {homeTeam} VS {awayTeam} ] " +
                            $"[ 양팀득점 - {bothToScoreSubType.ToString()} ] " +
                            $"[ NO: {prediction.value1}%, YES: {prediction.value2}% ] " +
                            $"{grade}";
                    }
                    break;

                case FootballPredictionType.Under_Over:
                    {
                        ((int)prediction.sub_label).TryParseEnum(out FootballUnderOverType underOverSubType);
                        string subTypeString = ConvertFootballUnderOverTypeToString(underOverSubType);
                        string grade = ConvertGradeToStartMark(prediction.grade);
                        message = $"[ {matchTime} ] [ {country} - {league} ] " +
                            $"[ {homeTeam} VS {awayTeam} ] " +
                            $"[ 언더 오버 - {subTypeString} ] " +
                            $"[ 언더: {prediction.value1}%, 오버: {prediction.value2}% ] " +
                            $"{grade}";
                    }
                    break;

                default:
                    break;
            }

            return message;
        }

        public static string MakeHitNotificationMessage(Fixture fixture, FootballTable.Prediction prediction)
        {
            var country = fixture.League.Country;
            var league = fixture.League.Name;
            var homeTeam = fixture.HomeTeam.TeamName;
            var awayTeam = fixture.AwayTeam.TeamName;

            string message = string.Empty;
            ((int)prediction.main_label).TryParseEnum(out FootballPredictionType predictionType);
            switch (predictionType)
            {
                case FootballPredictionType.Match_Winner:
                    {
                        ((int)prediction.sub_label).TryParseEnum(out FootballMatchWinnerType matchWinnerSubType);
                        string subTypeString = ConvertFootballMatchWinnerTypeToString(matchWinnerSubType);
                        string grade = ConvertGradeToStartMark(prediction.grade);
                        message = $"[ {country} - {league} ] " +
                            $"[ {homeTeam} VS {awayTeam} ] " +
                            $"[ 승무패 - {subTypeString} ] " +
                            $"[ {fixture.GoalsHomeTeam} : {fixture.GoalsAwayTeam}] " +
                            $"적중!";
                    }
                    break;

                case FootballPredictionType.Both_Teams_to_Score:
                    {
                        ((int)prediction.sub_label).TryParseEnum(out YesNoType bothToScoreSubType);
                        string grade = ConvertGradeToStartMark(prediction.grade);
                        message = $"[ {country} - {league} ] " +
                            $"[ {homeTeam} VS {awayTeam} ] " +
                            $"[ 양팀득점 - {bothToScoreSubType.ToString()} ] " +
                            $"[ {fixture.GoalsHomeTeam} : {fixture.GoalsAwayTeam}] " +
                            $"적중!";
                    }
                    break;

                case FootballPredictionType.Under_Over:
                    {
                        ((int)prediction.sub_label).TryParseEnum(out FootballUnderOverType underOverSubType);
                        string subTypeString = ConvertFootballUnderOverTypeToString(underOverSubType);
                        string grade = ConvertGradeToStartMark(prediction.grade);
                        message = $"[ {country} - {league} ] " +
                            $"[ {homeTeam} VS {awayTeam} ] " +
                            $"[ 언더 오버 - {subTypeString} ] " +
                            $"[ {fixture.GoalsHomeTeam} : {fixture.GoalsAwayTeam}] " +
                            $"적중!";
                    }
                    break;

                default:
                    break;
            }

            return message;
        }

        #region Private Function

        private static string ConvertFootballMatchWinnerTypeToString(FootballMatchWinnerType subType)
        {
            switch (subType)
            {
                case FootballMatchWinnerType.Win:
                    return "승";

                case FootballMatchWinnerType.Lose:
                    return "패";

                case FootballMatchWinnerType.WinOrDraw:
                    return "승 또는 무";

                case FootballMatchWinnerType.WinOrLose:
                    return "승 또는 패";

                case FootballMatchWinnerType.DrawOrLose:
                    return "무 또는 패";

                default:
                    break;
            }

            return string.Empty;
        }

        private static string ConvertFootballUnderOverTypeToString(FootballUnderOverType subType)
        {
            switch (subType)
            {
                case FootballUnderOverType.UNDER_1_5:
                    return "1.5 언더";

                case FootballUnderOverType.OVER_1_5:
                    return "1.5 오버";

                case FootballUnderOverType.UNDER_2_5:
                    return "2.5 언더";

                case FootballUnderOverType.OVER_2_5:
                    return "2.5 오버";

                case FootballUnderOverType.UNDER_3_5:
                    return "3.5 언더";

                case FootballUnderOverType.OVER_3_5:
                    return "3.5 오버";

                case FootballUnderOverType.UNDER_4_5:
                    return "4.5 언더";

                case FootballUnderOverType.OVER_4_5:
                    return "4.5 오버";

                default:
                    break;
            }

            return string.Empty;
        }

        private static string ConvertGradeToStartMark(short grade)
        {
            int totalMark = 5;
            float remainMark = grade / 2.0f;
            int fullImgMark = (int)remainMark;

            string result = "";
            for (int i = 0; i < fullImgMark; i++)
            {
                result += "●";
                remainMark -= 1;
            }

            for (int i = 0; i < (totalMark - fullImgMark); i++)
            {
                if (remainMark > 0)
                {
                    result += "◐";
                    remainMark -= 1;
                }
                else
                {
                    result += "○";
                }
            }

            return result;
        }

        private static List<int> GetAvailableScoreSet(double pred_score, double my_att_trend, double op_def_trend)
        {
            // 예측 스코어 (반올림)
            double score1 = pred_score;
            // 예측 스코어 + my_att_trend - op_def_trend (반올림)
            double score2 = pred_score + my_att_trend - op_def_trend;
            // homeScore2 - 표준편차 (버림)
            double score3 = score2 - PredictionFacade.Score_std_dev;
            // homeScore2 + 표준편차 (버림)
            double score4 = score2 + PredictionFacade.Score_std_dev;

            HashSet<int> scoreSet = new HashSet<int>();
            scoreSet.Add((int)Math.Round(score1, MidpointRounding.AwayFromZero));
            scoreSet.Add((int)Math.Round(score2, MidpointRounding.AwayFromZero));
            scoreSet.Add((int)Math.Truncate(score3));
            scoreSet.Add((int)Math.Truncate(score4));

            var sortedArray = scoreSet.ToArray();
            Array.Sort(sortedArray);

            return new List<int>(sortedArray);
        }

        private static MatchResultType GetMatchWinner(FootballMatchWinner data, out double proba)
        {
            int[] knnProba = ConvertProbaWinnerToArray(data.KNN);
            int[] sgdProba = ConvertProbaWinnerToArray(data.SGD);
            int[] subProba = ConvertProbaWinnerToArray(data.Sub);

            var maenProba = GetProbaArithmeticMean(knnProba, sgdProba, subProba);

            return GetMatchWinner(maenProba, out proba); ;
        }

        private static MatchResultType GetMatchWinner(int[] probas, out double proba)
        {
            if (probas.Length != 3)
                throw new Exception("proba.Length is must be three");

            if (probas[0] > probas[1] && probas[0] > probas[2])
            {
                proba = probas[0];
                return MatchResultType.Win;
            }
            else if (probas[2] > probas[0] && probas[2] > probas[1])
            {
                proba = probas[2];
                return MatchResultType.Lose;
            }

            proba = probas[0];
            return MatchResultType.Draw;
        }

        private static YesNoType GetYesOrNo(int[] probas, out double proba)
        {
            if (probas.Length != 2)
                throw new Exception("proba.Length is must be two");

            if (probas[0] > probas[1])
            {
                proba = probas[0];
                return YesNoType.No;
            }
            else
            {
                proba = probas[1];
                return YesNoType.Yes;
            }
        }

        /// <summary>
        /// 확률 리스트들의 산술 평균
        /// </summary>
        /// <param name="probas"></param>
        /// <returns></returns>
        private static int[] GetProbaArithmeticMean(params int[][] probas)
        {
            if (probas.Length == 0 || probas[0].Length == 0)
                throw new Exception("proba.Length is zero");

            int probaListCnt = probas.Length;
            int probaElemCnt = probas[0].Length;

            List<int> meanProbas = new List<int>();
            for (int i = 0; i < probaElemCnt; i++)
            {
                int elemSum = 0;
                for (int j = 0; j < probaListCnt; j++)
                {
                    elemSum += probas[j][i];
                }

                meanProbas.Add(elemSum);
            }

            double totalSum = meanProbas.Sum();
            for (int i = 0; i < probaElemCnt; i++)
            {
                meanProbas[i] = (int)Math.Truncate((meanProbas[i] / totalSum) * 100);
            }

            return meanProbas.ToArray();
        }

        private static int[] ConvertProbaWinnerToArray(ProbaWinner data)
        {
            return new int[] { (int)Math.Truncate(data.WinProba * 100), (int)Math.Truncate(data.DrawProba * 100), (int)Math.Truncate(data.LoseProba * 100) };
        }

        private static int[] ConvertProbaYNToArray(ProbaYN data)
        {
            return new int[] { (int)Math.Truncate(data.NO * 100), (int)Math.Truncate(data.YES * 100) };
        }

        private static int[] GetWinnerMeanProbas(ProbaWinner knn, ProbaWinner sgd, ProbaWinner sub)
        {
            int[] meanProbas = null;
            var knnProbas = ConvertProbaWinnerToArray(knn);
            var sgdProbas = ConvertProbaWinnerToArray(sgd);
            var subProbas = ConvertProbaWinnerToArray(sub);

            if (GetMatchWinner(knnProbas, out double _) != GetMatchWinner(sgdProbas, out double _))
                meanProbas = GetProbaArithmeticMean(knnProbas, sgdProbas, subProbas);
            else
                meanProbas = GetProbaArithmeticMean(sgdProbas, subProbas);

            return meanProbas;
        }

        private static int[] GetWinnerMeanProbas(ProbaWinner sgd, ProbaWinner sub)
        {
            int[] meanProbas = null;
            var sgdProbas = ConvertProbaWinnerToArray(sgd);
            var subProbas = ConvertProbaWinnerToArray(sub);

            meanProbas = GetProbaArithmeticMean(sgdProbas, subProbas);

            return meanProbas;
        }

        [Obsolete]
        private static int[] GetYNMeanProbas(ProbaYN knn, ProbaYN sgd, ProbaYN sub)
        {
            int[] meanProbas = null;
            var knnProbas = ConvertProbaYNToArray(knn);
            var sgdProbas = ConvertProbaYNToArray(sgd);
            var subProbas = ConvertProbaYNToArray(sub);

            if (GetYesOrNo(knnProbas, out double _) != GetYesOrNo(sgdProbas, out double _))
                meanProbas = GetProbaArithmeticMean(knnProbas, sgdProbas, subProbas);
            else
                meanProbas = GetProbaArithmeticMean(sgdProbas, subProbas);

            return meanProbas;
        }

        private static int[] GetYNMeanProbas(ProbaYN sgd, ProbaYN sub)
        {
            var sgdProbas = ConvertProbaYNToArray(sgd);
            var subProbas = ConvertProbaYNToArray(sub);

            int[] meanProbas = GetProbaArithmeticMean(sgdProbas, subProbas);

            return meanProbas;
        }

        private static double GetMatchWinnerGrade(FootballPrediction data, FootballMatchWinnerType subType, out bool isRecommend)
        {
            var homeScoreSet = GetAvailableScoreSet(data.MatchScore.HomeScore, data.HomeStat.AttTrend, data.AwayStat.DefTrend);
            var awayScoreSet = GetAvailableScoreSet(data.MatchScore.AwayScore, data.AwayStat.AttTrend, data.HomeStat.DefTrend);
            var matchWinnerType = GetMatchWinner(data.MatchWinner, out double proba);

            if (matchWinnerType == MatchResultType.Win)
                homeScoreSet.Reverse();

            if (matchWinnerType == MatchResultType.Lose)
                awayScoreSet.Reverse();

            var homeScores = homeScoreSet.Take(2).ToArray();
            var awayScores = awayScoreSet.Take(2).ToArray();

            int[] meanProbas = GetWinnerMeanProbas(data.MatchWinner.SGD, data.MatchWinner.Sub);
            double meanHomeScore = homeScores.Average();
            double meanAwayScore = awayScores.Average();

            isRecommend = false;
            double grade = 0.0;
            switch (subType)
            {
                case FootballMatchWinnerType.Win:
                    {
                        int probaDiff = meanProbas[0] - PredictionFacade.Winner_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                        grade += (meanHomeScore - meanAwayScore) > 0 ? (meanHomeScore - meanAwayScore) * 1.5 : -2;

                        isRecommend = grade >= 7;
                    }
                    break;

                case FootballMatchWinnerType.Lose:
                    {
                        int probaDiff = meanProbas[2] - PredictionFacade.Winner_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                        grade += (meanAwayScore - meanHomeScore) > 0 ? (meanAwayScore - meanHomeScore) * 1.5 : -2;
                        isRecommend = grade >= 8;
                    }
                    break;

                case FootballMatchWinnerType.WinOrDraw:
                    {
                        int probaDiff = (meanProbas[0] + meanProbas[1]) - PredictionFacade.Winner_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                        grade += meanHomeScore - meanAwayScore;

                        isRecommend = grade >= 8;
                    }
                    break;

                case FootballMatchWinnerType.DrawOrLose:
                    {
                        int probaDiff = (meanProbas[1] + meanProbas[2]) - PredictionFacade.Winner_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                        grade += meanAwayScore - meanHomeScore;

                        isRecommend = grade >= 7;
                    }
                    break;

                //case FootballMatchWinnerType.WinOrLose:
                //    {
                //        int probaDiff = (meanProbas[0] + meanProbas[2]) - PredictionFacade.Winner_Proba_Criteria;
                //        grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                //        grade += Math.Abs(meanAwayScore - meanHomeScore);

                //        isRecommend = Math.Round(grade, MidpointRounding.AwayFromZero) >= 8;
                //    }
                //    break;

                default:
                    break;
            }

            return grade > 10 ? 10 :
                grade < 0 ? 0 : grade;
        }

        private static double GetUnderOverGrade(FootballPrediction data, FootballUnderOverType subType, out bool isRecommend)
        {
            var homeScoreSet = GetAvailableScoreSet(data.MatchScore.HomeScore, data.HomeStat.AttTrend, data.AwayStat.DefTrend);
            var awayScoreSet = GetAvailableScoreSet(data.MatchScore.AwayScore, data.AwayStat.AttTrend, data.HomeStat.DefTrend);
            var matchWinnerType = GetMatchWinner(data.MatchWinner, out double proba);

            if (matchWinnerType == MatchResultType.Win)
                homeScoreSet.Reverse();

            if (matchWinnerType == MatchResultType.Lose)
                awayScoreSet.Reverse();

            var homeScores = homeScoreSet.Take(2).ToArray();
            var awayScores = awayScoreSet.Take(2).ToArray();

            double meanHomeScore = homeScores.Average();
            double meanAwayScore = awayScores.Average();

            isRecommend = false;
            double grade = 0.0;
            switch (subType)
            {
                case FootballUnderOverType.UNDER_1_5:
                    {
                        //int[] meanProbas = GetYNMeanProbas(data.UnderOver.UO_1_5.KNN, data.UnderOver.UO_1_5.SGD, data.UnderOver.UO_1_5.Sub);

                        //int probaDiff = meanProbas[0] - PredictionFacade.YN_Proba_Criteria;
                        //grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                        //grade += (meanHomeScore + meanAwayScore) < 1.5 ? 3 : 0;

                        isRecommend = false;
                    }
                    break;

                case FootballUnderOverType.OVER_1_5:
                    {
                        int[] meanProbas = GetYNMeanProbas(data.UnderOver.UO_1_5.SGD, data.UnderOver.UO_1_5.Sub);

                        int probaDiff = meanProbas[1] - PredictionFacade.YN_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 6.0 : 0;
                        grade += (meanHomeScore + meanAwayScore) > 2 ? 2 : -2;

                        isRecommend = grade >= 8;
                    }
                    break;

                case FootballUnderOverType.UNDER_2_5:
                    {
                        int[] meanProbas = GetYNMeanProbas(data.UnderOver.UO_2_5.SGD, data.UnderOver.UO_2_5.Sub);

                        int probaDiff = meanProbas[0] - PredictionFacade.YN_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                        grade += (meanHomeScore + meanAwayScore) < 2.5 ? 3 : -2;

                        isRecommend = Math.Round(grade, MidpointRounding.AwayFromZero) >= 8;
                    }
                    break;

                case FootballUnderOverType.OVER_2_5:
                    {
                        int[] meanProbas = GetYNMeanProbas(data.UnderOver.UO_2_5.SGD, data.UnderOver.UO_2_5.Sub);

                        int probaDiff = meanProbas[1] - PredictionFacade.YN_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                        grade += (meanHomeScore + meanAwayScore) > 2.5 ? 3 : -2;

                        isRecommend = Math.Round(grade, MidpointRounding.AwayFromZero) >= 8;
                    }
                    break;

                case FootballUnderOverType.UNDER_3_5:
                    {
                        int[] meanProbas = GetYNMeanProbas(data.UnderOver.UO_3_5.SGD, data.UnderOver.UO_3_5.Sub);

                        int probaDiff = meanProbas[0] - PredictionFacade.YN_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 6.0 : 0;
                        grade += (meanHomeScore + meanAwayScore) < 3 ? 2 : -2;

                        isRecommend = grade >= 8;
                    }
                    break;

                case FootballUnderOverType.OVER_3_5:
                    {
                        //int[] meanProbas = GetYNMeanProbas(data.UnderOver.UO_3_5.KNN, data.UnderOver.UO_3_5.SGD, data.UnderOver.UO_3_5.Sub);

                        //int probaDiff = meanProbas[1] - PredictionFacade.YN_Proba_Criteria;
                        //grade += probaDiff > 0 ? probaDiff / 3.5 : 0;
                        //grade += (meanHomeScore + meanAwayScore) > 3 ? 1.5 : -1;

                        isRecommend = false;
                    }
                    break;

                case FootballUnderOverType.UNDER_4_5:
                    {
                        //int[] meanProbas = GetYNMeanProbas(data.UnderOver.UO_4_5.KNN, data.UnderOver.UO_4_5.SGD, data.UnderOver.UO_4_5.Sub);

                        //int probaDiff = meanProbas[0] - PredictionFacade.YN_Proba_Criteria;
                        //grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                        //grade += (meanHomeScore + meanAwayScore) < 4 ? 1 : -3;

                        isRecommend = false;
                    }
                    break;

                case FootballUnderOverType.OVER_4_5:
                    {
                        //int[] meanProbas = GetYNMeanProbas(data.UnderOver.UO_4_5.KNN, data.UnderOver.UO_4_5.SGD, data.UnderOver.UO_4_5.Sub);

                        //int probaDiff = meanProbas[1] - PredictionFacade.YN_Proba_Criteria;
                        //grade += probaDiff > 0 ? probaDiff / 5.0 : 0;
                        //grade += (meanHomeScore + meanAwayScore) > 4.5 ? 3 : 0;

                        isRecommend = false;
                    }
                    break;

                default:
                    break;
            }

            return grade > 10 ? 10 :
                grade < 0 ? 0 : grade;
        }

        private static double GetBothToScoreGrade(FootballPrediction data, YesNoType subType, out bool isRecommend)
        {
            var homeScoreSet = GetAvailableScoreSet(data.MatchScore.HomeScore, data.HomeStat.AttTrend, data.AwayStat.DefTrend);
            var awayScoreSet = GetAvailableScoreSet(data.MatchScore.AwayScore, data.AwayStat.AttTrend, data.HomeStat.DefTrend);
            var matchWinnerType = GetMatchWinner(data.MatchWinner, out double proba);

            if (matchWinnerType == MatchResultType.Win)
                homeScoreSet.Reverse();

            if (matchWinnerType == MatchResultType.Lose)
                awayScoreSet.Reverse();

            var homeScores = homeScoreSet.Take(2).ToArray();
            var awayScores = awayScoreSet.Take(2).ToArray();

            int[] meanProbas = GetYNMeanProbas(data.BothToScore.SGD, data.BothToScore.Sub);
            double meanHomeScore = homeScores.Average();
            double meanAwayScore = awayScores.Average();

            double grade = 0.0;
            isRecommend = false;
            switch (subType)
            {
                case YesNoType.No:
                    {
                        int probaDiff = meanProbas[0] - PredictionFacade.YN_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 3.5 : 0;
                        grade += meanHomeScore < 1 ? 2 : -2;
                        grade += meanAwayScore < 1 ? 2 : -2;

                        isRecommend = false;
                    }
                    break;

                case YesNoType.Yes:
                    {
                        int probaDiff = meanProbas[1] - PredictionFacade.YN_Proba_Criteria;
                        grade += probaDiff > 0 ? probaDiff / 3.5 : 0;
                        grade += meanHomeScore > 1 ? 1.5 : -2;
                        grade += meanAwayScore > 1 ? 1.5 : -2;

                        grade += meanHomeScore >= 2 ? 1.5 : 0;
                        grade += meanAwayScore >= 2 ? 1.5 : 0;

                        isRecommend = false;
                    }
                    break;

                default:
                    break;
            }

            return grade > 10 ? 10 :
                grade < 0 ? 0 : grade;
        }

        #endregion Private Function
    }
}