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
    public class FootballPredictionBothToScoreViewModel : NavigableViewModel
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

            if (datas[0] is FootballPredictionGroup bothToScorePredictionGroup)
            {
                var bothToScorePredictions = bothToScorePredictionGroup.Predictions;
                NoProba = bothToScorePredictions[0].Value1;
                YesProba = bothToScorePredictions[0].Value2;

                PredictionPickViewModel.SetMember(bothToScorePredictionGroup);
            }

            return await Task.FromResult(true);
        }

        #endregion NavigableViewModel

        #region Fields

        private double _gaugeHeight;
        private double _yesProba;
        private double _noProba;
        private FootballPredictionPickViewModel _predictionPickViewModel;

        #endregion Fields

        #region Properties

        public double GaugeHeight { get => _gaugeHeight; set => SetValue(ref _gaugeHeight, value); }
        public double YesProba { get => _yesProba; set => SetValue(ref _yesProba, value); }
        public double NoProba { get => _noProba; set => SetValue(ref _noProba, value); }
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

        public FootballPredictionBothToScoreViewModel(
            FootballPredictionBothToScorePage page) : base(page)
        {
            OnInitializeView();
        }

        #endregion Constructors
    }
}