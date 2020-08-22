using PosePacket.Service.Enum;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football.Match.PredictionPick
{
    public class FootballPredictionPickViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            PredictionPicks = new ObservableList<FootballPredictionPickInfo>();
            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private FootballPredictionGroup _predictionGroup;
        private ObservableList<FootballPredictionPickInfo> _predictionPicks;

        #endregion Fields

        #region Properties

        public ObservableList<FootballPredictionPickInfo> PredictionPicks { get => _predictionPicks; set => SetValue(ref _predictionPicks, value); }

        #endregion Properties

        #region Constructors

        public FootballPredictionPickViewModel()
        {
            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public void SetMember(FootballPredictionGroup predictionGroup)
        {
            _predictionGroup = predictionGroup;
            PredictionPicks.Clear();

            var predictions = predictionGroup.Predictions.Where(elem => elem.Grade != 0).ToArray();
            switch (predictionGroup.MainLabel)
            {
                case FootballPredictionType.Match_Winner:
                    SetMatchWinnerPredictionPick(predictions);
                    break;

                case FootballPredictionType.Both_Teams_to_Score:
                    SetBothToScorePredictionPick(predictions);
                    break;

                case FootballPredictionType.Under_Over:
                    SetUnderOverPredictionPick(predictions);
                    break;

                default:
                    break;
            }
        }

        private FootballPredictionInfo[] SortByGrade(FootballPredictionInfo[] predictionInfos)
        {
            return predictionInfos.OrderByDescending(elem => elem.Grade).ToArray();
        }

        private void SetMatchWinnerPredictionPick(FootballPredictionInfo[] predictionInfos)
        {
            var sortedPredictions = SortByGrade(predictionInfos);

            foreach (var prediction in sortedPredictions)
            {
                string predictionTitle = GetMatchWinnerSubTitle(prediction.SubLabel);
                PredictionPicks.Add(new FootballPredictionPickInfo
                {
                    Title = predictionTitle,
                    IsRecommend = prediction.IsRecommended,
                    Rate = prediction.Grade / 2.0,
                });
            }
        }

        private void SetBothToScorePredictionPick(FootballPredictionInfo[] predictionInfos)
        {
            var sortedPredictions = SortByGrade(predictionInfos);

            foreach (var prediction in sortedPredictions)
            {
                string predictionTitle = GetBothToScoreSubTitle(prediction.SubLabel);
                PredictionPicks.Add(new FootballPredictionPickInfo
                {
                    Title = predictionTitle,
                    IsRecommend = prediction.IsRecommended,
                    Rate = prediction.Grade / 2.0,
                });
            }
        }

        private void SetUnderOverPredictionPick(FootballPredictionInfo[] predictionInfos)
        {
            var sortedPredictions = SortByGrade(predictionInfos);

            foreach (var prediction in sortedPredictions)
            {
                string predictionTitle = GetUnderOverSubTitle(prediction.SubLabel);
                PredictionPicks.Add(new FootballPredictionPickInfo
                {
                    Title = predictionTitle,
                    IsRecommend = prediction.IsRecommended,
                    Rate = prediction.Grade / 2.0,
                });
            }
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

        #endregion Methods
    }
}