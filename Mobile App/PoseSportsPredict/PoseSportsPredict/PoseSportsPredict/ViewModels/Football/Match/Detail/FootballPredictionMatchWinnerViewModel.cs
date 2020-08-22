using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.PredictionPick;
using PoseSportsPredict.Views.Football.Match.Detail;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballPredictionMatchWinnerViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            PredictionPickViewModel = ShinyHost.Resolve<FootballPredictionPickViewModel>();
            return true;
        }

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            if (datas.Length == 0 || !(datas[0] is FootballPredictionGroup))
                return await Task.FromResult(false);

            var screenHelper = DependencyService.Resolve<IScreenHelper>();
            GaugeHeight = screenHelper.ScreenSize.Width / 4.0;

            if (datas[0] is FootballPredictionGroup matchWinnerPredictionGroup)
            {
                var matchWinnerPredictions = matchWinnerPredictionGroup.Predictions;
                WinProba = matchWinnerPredictions[0].Value1;
                DrawProba = matchWinnerPredictions[0].Value2;
                LoseProba = matchWinnerPredictions[0].Value3;

                PredictionPickViewModel.SetMember(matchWinnerPredictionGroup);
            }

            return await Task.FromResult(true);
        }

        #endregion NavigableViewModel

        #region Fields

        private double _gaugeHeight;
        private double _winProba;
        private double _drawProba;
        private double _loseProba;
        private FootballPredictionPickViewModel _predictionPickViewModel;

        #endregion Fields

        #region Properties

        public double GaugeHeight { get => _gaugeHeight; set => SetValue(ref _gaugeHeight, value); }
        public double WinProba { get => _winProba; set => SetValue(ref _winProba, value); }
        public double DrawProba { get => _drawProba; set => SetValue(ref _drawProba, value); }
        public double LoseProba { get => _loseProba; set => SetValue(ref _loseProba, value); }
        public FootballPredictionPickViewModel PredictionPickViewModel { get => _predictionPickViewModel; set => SetValue(ref _predictionPickViewModel, value); }

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

        public FootballPredictionMatchWinnerViewModel(
            FootballPredictionMatchWinnerPage page) : base(page)
        {
            OnInitializeView();
        }

        #endregion Constructors
    }
}