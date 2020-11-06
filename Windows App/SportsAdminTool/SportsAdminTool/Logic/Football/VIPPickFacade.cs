using ControlzEx.Standard;
using LogicCore.Converter;
using PosePacket.Service.Enum;
using PosePacket.Service.Football.Models.Enums;
using PredictorAPI.Models.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using FootballTable = Repository.Mysql.FootballDB.Tables;

namespace SportsAdminTool.Logic.Football
{
    public static class VIPPickFacade
    {
        public static void CheckVipPicks(FootballPrediction data, List<FootballTable.Prediction> predictions)
        {
            CheckMatchWinner(data, predictions);
            CheckBothToGoals(data, predictions);
            CheckUnderOver(data, predictions);
        }

        public static void CheckMatchWinner(FootballPrediction data, List<FootballTable.Prediction> predictions)
        {
            var homeScorepred = predictions.First(elem => elem.main_label == (int)FootballPredictionType.Final_Score && elem.sub_label == (int)TeamCampType.Home);
            var awayScorepred = predictions.First(elem => elem.main_label == (int)FootballPredictionType.Final_Score && elem.sub_label == (int)TeamCampType.Away);

            var winPred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Match_Winner && elem.sub_label == (int)FootballMatchWinnerType.Win);
            var winOrDrawPred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Match_Winner && elem.sub_label == (int)FootballMatchWinnerType.WinOrDraw);
            var losePred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Match_Winner && elem.sub_label == (int)FootballMatchWinnerType.Lose);
            var loseOrDrawPred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Match_Winner && elem.sub_label == (int)FootballMatchWinnerType.DrawOrLose);

            if (winPred != null
                && winPred.grade > 5
                && CheckMatchWinnerProcess(FootballMatchWinnerType.Win, data, winPred, homeScorepred, awayScorepred))
                return;
            else if (losePred != null
                && losePred.grade > 5
                && CheckMatchWinnerProcess(FootballMatchWinnerType.Lose, data, losePred, homeScorepred, awayScorepred))
                return;
            else if (winOrDrawPred != null
                && winOrDrawPred.is_recommended
                && CheckMatchWinnerProcess(FootballMatchWinnerType.WinOrDraw, data, winOrDrawPred, homeScorepred, awayScorepred))
                return;
            else if (loseOrDrawPred != null
                && loseOrDrawPred.is_recommended
                && CheckMatchWinnerProcess(FootballMatchWinnerType.DrawOrLose, data, loseOrDrawPred, homeScorepred, awayScorepred))
                return;
        }

        public static bool CheckMatchWinnerProcess(FootballMatchWinnerType matchWinnerType, FootballPrediction data, FootballTable.Prediction matchWinnerPred, FootballTable.Prediction homeScorePred, FootballTable.Prediction awayScorePred)
        {
            double myPredScoreMin = 0;
            double myPredScoreMax = 0;
            double myAvgGF = 0;
            double myAvgGA = 0;
            double myRestDate = 0;

            double opPredScoreMin = 0;
            double opPredScoreMax = 0;
            double opAvgGF = 0;
            double opAvgGA = 0;
            double opRestDate = 0;

            switch (matchWinnerType)
            {
                case FootballMatchWinnerType.Win:
                case FootballMatchWinnerType.WinOrDraw:
                    {
                        myPredScoreMin = homeScorePred.value1;
                        myPredScoreMax = homeScorePred.value2;
                        myAvgGF = data.HomeStat.AvgGF + data.HomeStat.AttTrend;
                        myAvgGA = data.HomeStat.AvgGA - data.HomeStat.DefTrend;
                        myRestDate = data.HomeStat.RestDate;

                        opPredScoreMin = awayScorePred.value1;
                        opPredScoreMax = awayScorePred.value2;
                        opAvgGF = data.AwayStat.AvgGF + data.AwayStat.AttTrend;
                        opAvgGA = data.AwayStat.AvgGA - data.AwayStat.DefTrend;
                        opRestDate = data.AwayStat.RestDate;
                    }
                    break;

                case FootballMatchWinnerType.Lose:
                case FootballMatchWinnerType.DrawOrLose:
                    {
                        myPredScoreMin = awayScorePred.value1;
                        myPredScoreMax = awayScorePred.value2;
                        myAvgGF = data.AwayStat.AvgGF + data.AwayStat.AttTrend;
                        myAvgGA = data.AwayStat.AvgGA - data.AwayStat.DefTrend;
                        myRestDate = data.AwayStat.RestDate;

                        opPredScoreMin = homeScorePred.value1;
                        opPredScoreMax = homeScorePred.value2;
                        opAvgGF = data.HomeStat.AvgGF + data.HomeStat.AttTrend;
                        opAvgGA = data.HomeStat.AvgGA - data.HomeStat.DefTrend;
                        opRestDate = data.HomeStat.RestDate;
                    }
                    break;
            }

            switch (matchWinnerType)
            {
                case FootballMatchWinnerType.Win:
                    {
                        if ((myPredScoreMin - opPredScoreMin) > 1
                            && (myPredScoreMax - opPredScoreMax) > 1
                            && (myAvgGF - opAvgGF) > 1
                            && (myAvgGA - opAvgGA) < -1
                            && myRestDate > 3
                            && (myRestDate - opRestDate) > -6)
                        {
                            matchWinnerPred.is_vip_pick = true;
                            return true;
                        }
                    }
                    break;

                case FootballMatchWinnerType.Lose:
                    {
                        if ((myPredScoreMin - opPredScoreMin) > 1
                            && (myPredScoreMax - opPredScoreMax) > 1
                            && (myAvgGF - opAvgGF) > 1.3
                            && (myAvgGA - opAvgGA) < -1.3
                            && myRestDate > 3
                            && (myRestDate - opRestDate) > -6)
                        {
                            matchWinnerPred.is_vip_pick = true;
                            return true;
                        }
                    }
                    break;

                case FootballMatchWinnerType.WinOrDraw:
                    {
                        if (myPredScoreMin > opPredScoreMin
                            && myPredScoreMax > opPredScoreMax
                            && (myAvgGF - opAvgGF) > 0.5
                            && (myAvgGA - opAvgGA) < -0.5
                            && myRestDate > 3
                            && (myRestDate - opRestDate) > -6)
                        {
                            matchWinnerPred.is_vip_pick = true;
                            return true;
                        }
                    }
                    break;

                case FootballMatchWinnerType.DrawOrLose:
                    {
                        if (myPredScoreMin > opPredScoreMin
                            && myPredScoreMax > opPredScoreMax
                            && (myAvgGF - opAvgGF) > 0.8
                            && (myAvgGA - opAvgGA) < -0.8
                            && myRestDate > 3
                            && (myRestDate - opRestDate) > -6)
                        {
                            matchWinnerPred.is_vip_pick = true;
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        public static void CheckBothToGoals(FootballPrediction data, List<FootballTable.Prediction> predictions)
        {
            var homeScorePred = predictions.First(elem => elem.main_label == (int)FootballPredictionType.Final_Score && elem.sub_label == (int)TeamCampType.Home);
            var awayScorePred = predictions.First(elem => elem.main_label == (int)FootballPredictionType.Final_Score && elem.sub_label == (int)TeamCampType.Away);
            var btsPred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Both_Teams_to_Score);
            if (btsPred == null)
                return;

            double myPredScoreMin = homeScorePred.value1;
            double myPredScoreMax = homeScorePred.value2;
            double myAvgGF = data.HomeStat.AvgGF + data.HomeStat.AttTrend;
            double myAvgGA = data.HomeStat.AvgGA - data.HomeStat.DefTrend;

            double opPredScoreMin = awayScorePred.value1;
            double opPredScoreMax = awayScorePred.value2;
            double opAvgGF = data.AwayStat.AvgGF + data.AwayStat.AttTrend;
            double opAvgGA = data.AwayStat.AvgGA - data.AwayStat.DefTrend;

            btsPred.sub_label.TryParseEnum(out YesNoType btsResultType);

            switch (btsResultType)
            {
                case YesNoType.Yes:
                    {
                        if (btsPred.grade > 5
                            && (myPredScoreMin >= 1 && opPredScoreMin >= 1)
                            && ((myAvgGF > 1.6 && opAvgGA > 1.4) && (opAvgGF > 1.6 && myAvgGA > 1.4)))
                            btsPred.is_vip_pick = true;
                    }
                    break;

                case YesNoType.No:
                    {
                        if (btsPred.grade > 5
                            && (myPredScoreMin == 0 && opPredScoreMin == 0)
                            && ((myAvgGF < 0.8 && opAvgGA < 0.6) || (opAvgGF < 0.8 && myAvgGA < 0.6)))
                            btsPred.is_vip_pick = true;
                    }
                    break;
            }
        }

        public static void CheckUnderOver(FootballPrediction data, List<FootballTable.Prediction> predictions)
        {
            var homeScorePred = predictions.First(elem => elem.main_label == (int)FootballPredictionType.Final_Score && elem.sub_label == (int)TeamCampType.Home);
            var awayScorePred = predictions.First(elem => elem.main_label == (int)FootballPredictionType.Final_Score && elem.sub_label == (int)TeamCampType.Away);
            var over_1_5_Pred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Under_Over && elem.sub_label == (int)FootballUnderOverType.OVER_1_5);
            var over_2_5_Pred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Under_Over && elem.sub_label == (int)FootballUnderOverType.OVER_2_5);
            var under_2_5_Pred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Under_Over && elem.sub_label == (int)FootballUnderOverType.UNDER_2_5);
            var under_3_5_Pred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Under_Over && elem.sub_label == (int)FootballUnderOverType.UNDER_3_5);

            var btsPred = predictions.FirstOrDefault(elem => elem.main_label == (int)FootballPredictionType.Both_Teams_to_Score);
            YesNoType btsResultType = YesNoType.No;
            btsPred?.sub_label.TryParseEnum(out btsResultType);

            if (over_2_5_Pred != null
                && over_2_5_Pred.grade > 5
                && btsResultType == YesNoType.Yes
                && CheckUnderOverProcess(FootballUnderOverType.OVER_2_5, data, over_2_5_Pred, homeScorePred, awayScorePred))
                return;
            else if (under_2_5_Pred != null
                && under_2_5_Pred.grade > 5
                && btsResultType == YesNoType.No
                && CheckUnderOverProcess(FootballUnderOverType.UNDER_2_5, data, under_2_5_Pred, homeScorePred, awayScorePred))
                return;
            else if (over_1_5_Pred != null
                && over_1_5_Pred.is_recommended
                && CheckUnderOverProcess(FootballUnderOverType.OVER_1_5, data, over_1_5_Pred, homeScorePred, awayScorePred))
                return;
            else if (under_3_5_Pred != null
                && under_3_5_Pred.is_recommended
                && CheckUnderOverProcess(FootballUnderOverType.UNDER_3_5, data, under_3_5_Pred, homeScorePred, awayScorePred))
                return;
        }

        public static bool CheckUnderOverProcess(FootballUnderOverType underOverType, FootballPrediction data, FootballTable.Prediction underOverPred, FootballTable.Prediction homeScorePred, FootballTable.Prediction awayScorePred)
        {
            double myPredScoreMin = homeScorePred.value1;
            double myPredScoreMax = homeScorePred.value2;
            double myAvgGF = data.HomeStat.AvgGF + data.HomeStat.AttTrend;
            double myAvgGA = data.HomeStat.AvgGA - data.HomeStat.DefTrend;

            double opPredScoreMin = awayScorePred.value1;
            double opPredScoreMax = awayScorePred.value2;
            double opAvgGF = data.AwayStat.AvgGF + data.AwayStat.AttTrend;
            double opAvgGA = data.AwayStat.AvgGA - data.AwayStat.DefTrend;

            switch (underOverType)
            {
                case FootballUnderOverType.OVER_1_5:
                    {
                        if ((myPredScoreMin + opPredScoreMin) >= 1
                            && (myAvgGF + opAvgGF) > 2.8
                            && ((myAvgGF > 1.4 && opAvgGF > 1.4 && myAvgGA > 1.2 && opAvgGA > 1.2)
                                || ((myAvgGF > 1.8 && opAvgGA > 1.8) || (opAvgGF > 1.8 && myAvgGA > 1.8))))
                        {
                            underOverPred.is_vip_pick = true;
                            return true;
                        }
                    }
                    break;

                case FootballUnderOverType.OVER_2_5:
                    {
                        if ((myPredScoreMin + opPredScoreMin) >= 2
                            && (myAvgGF + opAvgGF) > 3.8
                            && myAvgGF > 1.8
                            && opAvgGF > 1.8
                            && myAvgGA > 1.6
                            && opAvgGA > 1.6)
                        {
                            underOverPred.is_vip_pick = true;
                            return true;
                        }
                    }
                    break;

                case FootballUnderOverType.UNDER_2_5:
                    {
                        if ((myPredScoreMax + opPredScoreMax) <= 2
                            && myAvgGF < 0.8
                            && opAvgGF < 0.8
                            && myAvgGA < 1
                            && opAvgGA < 1
                            && (myAvgGA + opAvgGA) < 1.8)
                        {
                            underOverPred.is_vip_pick = true;
                            return true;
                        }
                    }
                    break;

                case FootballUnderOverType.UNDER_3_5:
                    {
                        if ((myPredScoreMax + opPredScoreMax) <= 3
                            && myAvgGF < 1.2
                            && opAvgGF < 1.2
                            && myAvgGA < 1.4
                            && opAvgGA < 1.4
                            && (myAvgGA + opAvgGA) < 2.8)
                        {
                            underOverPred.is_vip_pick = true;
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }
    }
}