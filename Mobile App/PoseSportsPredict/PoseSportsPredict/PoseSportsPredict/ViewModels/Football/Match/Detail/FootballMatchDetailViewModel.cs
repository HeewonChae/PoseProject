using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using PacketModels = PosePacket.Service.Football.Models;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMatchDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            if (datas == null)
                return false;

            if (!(datas[0] is PacketModels.FixtureDetail fixtureDetail))
                return false;

            MatchDetail = fixtureDetail;

            OverviewModel = ShinyHost.Resolve<FootballMatchDetailOverviewModel>();
            H2HViewModel = ShinyHost.Resolve<FootballMatchDetailH2HViewModel>();
            PredictionsViewModel = ShinyHost.Resolve<FootballMatchDetailPredictionsViewModel>();
            OddsViewModel = ShinyHost.Resolve<FootballMatchDetailOddsViewModel>();

            SelectedViewIndex = 0;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
        }

        #endregion NavigableViewModel

        #region Fields

        private bool _isAlarmed;
        private bool _isBookmarked;
        private PacketModels.FixtureDetail _matchDetail;
        private int _selectedViewIndex;
        private FootballMatchDetailOverviewModel _overviewModel;
        private FootballMatchDetailH2HViewModel _h2hViewModel;
        private FootballMatchDetailPredictionsViewModel _predictionsViewModel;
        private FootballMatchDetailOddsViewModel _oddsViewModel;

        #endregion Fields

        #region Properties

        public bool IsBookmarked { get => _isBookmarked; set => SetValue(ref _isBookmarked, value); }
        public Color IsAlarmed => _isAlarmed ? Color.White : AppResourcesHelper.GetResourceColor("CustomGrey");
        public PacketModels.FixtureDetail MatchDetail { get => _matchDetail; set => SetValue(ref _matchDetail, value); }
        public int SelectedViewIndex { get => _selectedViewIndex; set => SetValue(ref _selectedViewIndex, value); }
        public FootballMatchDetailOverviewModel OverviewModel { get => _overviewModel; set => SetValue(ref _overviewModel, value); }
        public FootballMatchDetailH2HViewModel H2HViewModel { get => _h2hViewModel; set => SetValue(ref _h2hViewModel, value); }
        public FootballMatchDetailPredictionsViewModel PredictionsViewModel { get => _predictionsViewModel; set => SetValue(ref _predictionsViewModel, value); }
        public FootballMatchDetailOddsViewModel OddsViewModel { get => _oddsViewModel; set => SetValue(ref _oddsViewModel, value); }

        #endregion Properties

        #region Commands

        public ICommand TouchBackButtonCommand { get => new RelayCommand(TouchBackButton); }

        private async void TouchBackButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PopModalAsync();

            SetIsBusy(false);
        }

        public ICommand TouchAlarmButtonCommand { get => new RelayCommand(TouchAlarmButton); }

        private async void TouchAlarmButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var message = _isAlarmed ? LocalizeString.Cancle_Alarm : LocalizeString.Set_Alarm;
            await MaterialDialog.Instance.SnackbarAsync(message, 1500);

            _isAlarmed = !_isAlarmed;
            OnPropertyChanged("IsAlarmed");

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand(TouchBookmarkButton); }

        private async void TouchBookmarkButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var message = IsBookmarked ? LocalizeString.Delete_Bookmark : LocalizeString.Set_Bookmark;
            await MaterialDialog.Instance.SnackbarAsync(message, 1500);

            IsBookmarked = !IsBookmarked;

            SetIsBusy(false);
        }

        public ICommand SwipeLeftViewSwitcherCommad { get => new RelayCommand(SwipeLeftViewSwitcher); }

        private void SwipeLeftViewSwitcher()
        {
            if (SelectedViewIndex < 3)
                SelectedViewIndex++;
        }

        public ICommand SwipeRightViewSwitcherCommad { get => new RelayCommand(SwipeRightViewSwitcher); }

        private void SwipeRightViewSwitcher()
        {
            if (SelectedViewIndex > 0)
                SelectedViewIndex--;
        }

        #endregion Commands

        #region Constructors

        public FootballMatchDetailViewModel(FootballMatchDetailPage page) : base(page)
        {
            //var tabHost = page.FindByName<TabHostView>("_tabHost");
            //var viewSwitcher = page.FindByName<ViewSwitcher>("_switcher");
            //tabHost.SelectedIndex = -1;

            //tabHost.SelectedTabIndexChanged += (s, e) =>
            //{
            //    var bindingCtx = viewSwitcher.Children[viewSwitcher.SelectedIndex].BindingContext as BaseViewModel;
            //    bindingCtx.OnAppearing();
            //};

            CoupledPage.Appearing += (s, e) => OnAppearing();
        }

        #endregion Constructors
    }
}