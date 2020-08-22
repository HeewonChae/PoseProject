using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Match.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballPredictionFinalScoreViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            return true;
        }

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            if (datas.Length == 0 || !(datas[0] is FootballPredictionGroup))
                return await Task.FromResult(false);

            var finalScorePredictions = (datas[0] as FootballPredictionGroup).Predictions;

            var homeScorePrediction = finalScorePredictions.First(elem => elem.SubLabel == 1);
            var homeScoreList = new List<int>
            {
                homeScorePrediction.Value1,
            };

            if (homeScorePrediction.Value2 != 0)
                homeScoreList.Add(homeScorePrediction.Value2);
            if (homeScorePrediction.Value3 != 0)
                homeScoreList.Add(homeScorePrediction.Value3);
            if (homeScorePrediction.Value4 != 0)
                homeScoreList.Add(homeScorePrediction.Value4);

            HomeScoreMinValue = homeScoreList.Min();
            HomeScoreMaxValue = homeScoreList.Max();
            IsHomeScoreCountOne = HomeScoreMinValue == HomeScoreMaxValue;

            var awayScorePrediction = finalScorePredictions.First(elem => elem.SubLabel == 2);
            var awayScoreList = new List<int>
            {
                awayScorePrediction.Value1,
            };

            if (awayScorePrediction.Value2 != 0)
                awayScoreList.Add(awayScorePrediction.Value2);
            if (awayScorePrediction.Value3 != 0)
                awayScoreList.Add(awayScorePrediction.Value3);
            if (awayScorePrediction.Value4 != 0)
                awayScoreList.Add(awayScorePrediction.Value4);

            AwayScoreMinValue = awayScoreList.Min();
            AwayScoreMaxValue = awayScoreList.Max();
            IsAwayScoreCountOne = AwayScoreMinValue == AwayScoreMaxValue;

            return await Task.FromResult(true);
        }

        #endregion NavigableViewModel

        #region Fields

        public bool _isHomeScoreCountOne;
        public bool _isAwayScoreCountOne;
        public int _homeScoreMinValue;
        public int _homeScoreMaxValue;
        public int _awayScoreMinValue;
        public int _awayScoreMaxValue;

        #endregion Fields

        #region Properties

        public bool IsHomeScoreCountOne { get => _isHomeScoreCountOne; set => SetValue(ref _isHomeScoreCountOne, value); }
        public bool IsAwayScoreCountOne { get => _isAwayScoreCountOne; set => SetValue(ref _isAwayScoreCountOne, value); }
        public int HomeScoreMinValue { get => _homeScoreMinValue; set => SetValue(ref _homeScoreMinValue, value); }
        public int HomeScoreMaxValue { get => _homeScoreMaxValue; set => SetValue(ref _homeScoreMaxValue, value); }
        public int AwayScoreMinValue { get => _awayScoreMinValue; set => SetValue(ref _awayScoreMinValue, value); }
        public int AwayScoreMaxValue { get => _awayScoreMaxValue; set => SetValue(ref _awayScoreMaxValue, value); }

        #endregion Properties

        #region Commands

        public ICommand TouchBackButtonCommand { get => new RelayCommand(TouchBackButton); }

        private async void TouchBackButton()
        {
            if (IsPageSwitched)
                return;

            SetIsPageSwitched(true);

            await PageSwitcher.PopPopupAsync();

            SetIsPageSwitched(false);
        }

        #endregion Commands

        #region Constructors

        public FootballPredictionFinalScoreViewModel(
            FootballPredictionFinalScorePage page) : base(page)
        {
            OnInitializeView();
        }

        #endregion Constructors
    }
}