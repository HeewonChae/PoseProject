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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballPredictionUnderOverViewModel : NavigableViewModel
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
            GaugeHeight = screenHelper.ScreenSize.Width / 5.3;

            if (datas[0] is FootballPredictionGroup underOverPredictionGroup)
            {
                var underOverPredictions = underOverPredictionGroup.Predictions;
                var underOver_1_5_pred = underOverPredictions.First(elem => elem.SubLabel == 1 || elem.SubLabel == 2);
                var underOver_2_5_pred = underOverPredictions.First(elem => elem.SubLabel == 3 || elem.SubLabel == 4);
                var underOver_3_5_pred = underOverPredictions.First(elem => elem.SubLabel == 5 || elem.SubLabel == 6);
                var underOver_4_5_pred = underOverPredictions.First(elem => elem.SubLabel == 7 || elem.SubLabel == 8);

                UnderProba_1_5 = underOver_1_5_pred.Value1;
                OverProba_1_5 = underOver_1_5_pred.Value2;

                UnderProba_2_5 = underOver_2_5_pred.Value1;
                OverProba_2_5 = underOver_2_5_pred.Value2;

                UnderProba_3_5 = underOver_3_5_pred.Value1;
                OverProba_3_5 = underOver_3_5_pred.Value2;

                UnderProba_4_5 = underOver_4_5_pred.Value1;
                OverProba_4_5 = underOver_4_5_pred.Value2;

                PredictionPickViewModel.SetMember(underOverPredictionGroup);
            }

            return await Task.FromResult(true);
        }

        #endregion NavigableViewModel

        #region Fields

        private double _gaugeHeight;
        private double _underProba_1_5;
        private double _overProba_1_5;
        private double _underProba_2_5;
        private double _overProba_2_5;
        private double _underProba_3_5;
        private double _overProba_3_5;
        private double _underProba_4_5;
        private double _overProba_4_5;
        private FootballPredictionPickViewModel _predictionPickViewModel;

        #endregion Fields

        #region Properties

        public double GaugeHeight { get => _gaugeHeight; set => SetValue(ref _gaugeHeight, value); }
        public double UnderProba_1_5 { get => _underProba_1_5; set => SetValue(ref _underProba_1_5, value); }
        public double OverProba_1_5 { get => _overProba_1_5; set => SetValue(ref _overProba_1_5, value); }
        public double UnderProba_2_5 { get => _underProba_2_5; set => SetValue(ref _underProba_2_5, value); }
        public double OverProba_2_5 { get => _overProba_2_5; set => SetValue(ref _overProba_2_5, value); }
        public double UnderProba_3_5 { get => _underProba_3_5; set => SetValue(ref _underProba_3_5, value); }
        public double OverProba_3_5 { get => _overProba_3_5; set => SetValue(ref _overProba_3_5, value); }
        public double UnderProba_4_5 { get => _underProba_4_5; set => SetValue(ref _underProba_4_5, value); }
        public double OverProba_4_5 { get => _overProba_4_5; set => SetValue(ref _overProba_4_5, value); }
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

        public FootballPredictionUnderOverViewModel(
            FootballPredictionUnderOverPage page) : base(page)
        {
            OnInitializeView();
        }

        #endregion Constructors
    }
}