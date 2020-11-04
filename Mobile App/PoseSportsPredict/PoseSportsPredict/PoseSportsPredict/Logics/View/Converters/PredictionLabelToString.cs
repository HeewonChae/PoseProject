using PosePacket.Service.Enum;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class PredictionLabelToString
    {
        public string Convert(FootballPredictionType mainLabel, byte subLabel)
        {
            string return_value = string.Empty;

            switch (mainLabel)
            {
                case FootballPredictionType.Match_Winner:
                    return_value = GetMatchWinnerSubTitle(subLabel);
                    break;

                case FootballPredictionType.Both_Teams_to_Score:
                    return_value = GetBothToScoreSubTitle(subLabel);
                    break;

                case FootballPredictionType.Under_Over:
                    return_value = GetUnderOverSubTitle(subLabel);
                    break;

                default:
                    break;
            }

            return return_value;
        }

        private string GetMatchWinnerSubTitle(int subTitle)
        {
            string result = string.Empty;

            subTitle.TryParseEnum(out FootballMatchWinnerType subType);
            switch (subType)
            {
                case FootballMatchWinnerType.Win:
                    result = LocalizeString.Match_Winner_Win;
                    break;

                case FootballMatchWinnerType.Lose:
                    result = LocalizeString.Match_Winner_Lose;
                    break;

                case FootballMatchWinnerType.WinOrDraw:
                    result = LocalizeString.Match_Winner_Win_Or_Draw;
                    break;

                case FootballMatchWinnerType.DrawOrLose:
                    result = LocalizeString.Match_Winner_Lose_Or_Draw;
                    break;

                default:
                    break;
            }

            return result;
        }

        private string GetBothToScoreSubTitle(int subTitle)
        {
            string result = string.Empty;

            subTitle.TryParseEnum(out YesNoType subType);
            switch (subType)
            {
                case YesNoType.Yes:
                    result = LocalizeString.Both_To_Score_Yes;
                    break;

                case YesNoType.No:
                    result = LocalizeString.Both_To_Score_No;
                    break;

                default:
                    break;
            }

            return result;
        }

        private string GetUnderOverSubTitle(int subTitle)
        {
            string result = string.Empty;

            subTitle.TryParseEnum(out FootballUnderOverType subType);
            switch (subType)
            {
                case FootballUnderOverType.UNDER_1_5:
                    result = LocalizeString.Under_1_5;
                    break;

                case FootballUnderOverType.OVER_1_5:
                    result = LocalizeString.Over_1_5;
                    break;

                case FootballUnderOverType.UNDER_2_5:
                    result = LocalizeString.Under_2_5;
                    break;

                case FootballUnderOverType.OVER_2_5:
                    result = LocalizeString.Over_2_5;
                    break;

                case FootballUnderOverType.UNDER_3_5:
                    result = LocalizeString.Under_3_5;
                    break;

                case FootballUnderOverType.OVER_3_5:
                    result = LocalizeString.Over_3_5;
                    break;

                case FootballUnderOverType.UNDER_4_5:
                    result = LocalizeString.Under_4_5;
                    break;

                case FootballUnderOverType.OVER_4_5:
                    result = LocalizeString.Over_4_5;
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}