using PosePacket.Service.Enum;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Logics.View.Converters;
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
        private bool _isExistPrediction;

        #endregion Fields

        #region Properties

        public ObservableList<FootballPredictionPickInfo> PredictionPicks { get => _predictionPicks; set => SetValue(ref _predictionPicks, value); }
        public bool IsExistPrediction { get => _isExistPrediction; set => SetValue(ref _isExistPrediction, value); }

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

            IsExistPrediction = PredictionPicks.Count > 0;
        }

        private FootballPredictionInfo[] SortByGrade(FootballPredictionInfo[] predictionInfos)
        {
            return predictionInfos.OrderByDescending(elem => elem.Grade).ToArray();
        }

        private void SetMatchWinnerPredictionPick(FootballPredictionInfo[] predictionInfos)
        {
            var predTitleConverter = ShinyHost.Resolve<PredictionLabelToString>();
            var sortedPredictions = SortByGrade(predictionInfos);

            foreach (var prediction in sortedPredictions)
            {
                string predictionTitle = predTitleConverter.Convert(prediction.MainLabel, prediction.SubLabel);
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
            var predTitleConverter = ShinyHost.Resolve<PredictionLabelToString>();
            var sortedPredictions = SortByGrade(predictionInfos);

            foreach (var prediction in sortedPredictions)
            {
                string predictionTitle = predTitleConverter.Convert(prediction.MainLabel, prediction.SubLabel);
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
            var predTitleConverter = ShinyHost.Resolve<PredictionLabelToString>();
            var sortedPredictions = SortByGrade(predictionInfos);

            foreach (var prediction in sortedPredictions)
            {
                string predictionTitle = predTitleConverter.Convert(prediction.MainLabel, prediction.SubLabel);
                PredictionPicks.Add(new FootballPredictionPickInfo
                {
                    Title = predictionTitle,
                    IsRecommend = prediction.IsRecommended,
                    Rate = prediction.Grade / 2.0,
                });
            }
        }

        #endregion Methods
    }
}