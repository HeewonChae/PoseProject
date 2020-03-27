using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using Plugin.LocalNotification;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.MessagingCenterMessageType;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Team;
using PoseSportsPredict.Views.Football.Match.Detail;
using Shiny;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using PacketModels = PosePacket.Service.Football.Models;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballMatchDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override async Task<bool> OnInitializeViewAsync(params object[] datas)
        {
            if (datas == null)
                return false;

            if (!(datas[0] is FootballMatchInfo matchInfo))
                return false;

            // Check Bookmark
            var bookmarkedMatch = await _sqliteService.SelectAsync<FootballMatchInfo>(matchInfo.PrimaryKey);
            MatchInfo = bookmarkedMatch ?? matchInfo;

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

        #region Services

        private ISQLiteService _sqliteService;

        #endregion Services

        #region Fields

        private FootballMatchInfo _matchInfo;
        private int _selectedViewIndex;
        private FootballMatchDetailOverviewModel _overviewModel;
        private FootballMatchDetailH2HViewModel _h2hViewModel;
        private FootballMatchDetailPredictionsViewModel _predictionsViewModel;
        private FootballMatchDetailOddsViewModel _oddsViewModel;

        #endregion Fields

        #region Properties

        public Color IsAlarmed => (MatchInfo?.IsAlarmed ?? false) ? Color.White : AppResourcesHelper.GetResourceColor("CustomGrey");
        public FootballMatchInfo MatchInfo { get => _matchInfo; set => SetValue(ref _matchInfo, value); }
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

        private void TouchAlarmButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            MatchInfo.IsAlarmed = !MatchInfo.IsAlarmed;
            OnPropertyChanged("IsAlarmed");

            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "App Alarm",
                Description = "Test Description",
                ReturningData = MatchInfo.JsonSerialize(),
                NotifyTime = DateTime.Now.AddSeconds(10),
                Android = new AndroidOptions
                {
                    IconName = "ic_soccer_alarm",
                },
            };

            NotificationCenter.Current.Show(notification);

            var message = MatchInfo.IsAlarmed ? LocalizeString.Set_Alarm : LocalizeString.Cancle_Alarm;
            UserDialogs.Instance.Toast(message);

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand(TouchBookmarkButton); }

        private async void TouchBookmarkButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            MatchInfo.Order = 0;
            MatchInfo.StoredTime = DateTime.Now;
            MatchInfo.IsBookmarked = !MatchInfo.IsBookmarked;

            // Add Bookmark
            if (MatchInfo.IsBookmarked)
                await _sqliteService.InsertOrUpdateAsync<FootballMatchInfo>(MatchInfo);
            else
                await _sqliteService.DeleteAsync<FootballMatchInfo>(MatchInfo.PrimaryKey);

            MessagingCenter.Send(this, FootballMessageType.Update_Bookmark_Match.ToString(), MatchInfo);

            var message = MatchInfo.IsBookmarked ? LocalizeString.Set_Bookmark : LocalizeString.Delete_Bookmark;
            UserDialogs.Instance.Toast(message);

            MatchInfo.OnPropertyChanged("IsBookmarked");

            SetIsBusy(false);
        }

        public ICommand LeagueNameClickCommand { get => new RelayCommand<FootballMatchInfo>((e) => LeagueNameClick(e)); }

        private async void LeagueNameClick(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>()
                , ShinyHost.Resolve<MatchInfoToLeagueInfoConverter>().Convert(matchInfo, typeof(FootballLeagueInfo), null, CultureInfo.CurrentUICulture));

            SetIsBusy(false);
        }

        public ICommand HomeTeamLogoClickCommand { get => new RelayCommand<FootballMatchInfo>((e) => HomeTeamLogoClick(e)); }

        private async void HomeTeamLogoClick(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>()
                , ShinyHost.Resolve<MatchInfoToTeamInfoConverter>().Convert(matchInfo, typeof(FootballTeamInfo), TeamType.Home, CultureInfo.CurrentUICulture));

            SetIsBusy(false);
        }

        public ICommand AwayTeamLogoClickCommand { get => new RelayCommand<FootballMatchInfo>((e) => AwayTeamLogoClick(e)); }

        private async void AwayTeamLogoClick(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>()
                , ShinyHost.Resolve<MatchInfoToTeamInfoConverter>().Convert(matchInfo, typeof(FootballTeamInfo), TeamType.Away, CultureInfo.CurrentUICulture));

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

        public FootballMatchDetailViewModel(
            FootballMatchDetailPage page
            , ISQLiteService sqliteService) : base(page)
        {
            //var tabHost = page.FindByName<TabHostView>("_tabHost");
            //var viewSwitcher = page.FindByName<ViewSwitcher>("_switcher");
            //tabHost.SelectedIndex = -1;

            //tabHost.SelectedTabIndexChanged += (s, e) =>
            //{
            //    var bindingCtx = viewSwitcher.Children[viewSwitcher.SelectedIndex].BindingContext as BaseViewModel;
            //    bindingCtx.OnAppearing();
            //};

            _sqliteService = sqliteService;

            CoupledPage.Appearing += (s, e) => OnAppearing();
        }

        #endregion Constructors
    }
}