using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Team;
using PoseSportsPredict.Views.Football.Match.Detail;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using Shiny;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballMatchDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            AlarmIcon = new ChangableIcon(
                "ic_alarm_selected.png",
                AppResourcesHelper.GetResourceColor("IconActivated"),
                "ic_alarm_unselected.png",
                Color.Black);

            return true;
        }

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            if (datas == null)
                return false;

            if (!(datas[0] is FootballMatchInfo matchInfo))
                return false;

            MatchInfo = matchInfo;

            // Check Bookmark
            var bookmarkedMatch = await _bookmarkService.GetBookmark<FootballMatchInfo>(matchInfo.PrimaryKey);
            MatchInfo.IsBookmarked = bookmarkedMatch?.IsBookmarked ?? false;
            MatchInfo.OnPropertyChanged(nameof(MatchInfo.IsBookmarked));

            // Check Alarm
            var notification = await _notificationService.GetNotification(MatchInfo.Id, SportsType.Football, NotificationType.MatchStart);
            MatchInfo.IsAlarmed = notification != null ? true : false;

            AlarmIcon.IsSelected = MatchInfo.IsAlarmed;

            OverviewModel = ShinyHost.Resolve<FootballMatchDetailOverviewModel>().SetMatchInfo(matchInfo);
            H2HViewModel = ShinyHost.Resolve<FootballMatchDetailH2HViewModel>();
            PredictionsViewModel = ShinyHost.Resolve<FootballMatchDetailPredictionsViewModel>();
            OddsViewModel = ShinyHost.Resolve<FootballMatchDetailOddsViewModel>();
            SelectedViewIndex = 0;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            var viewSwitcher = this.CoupledPage.FindByName<ViewSwitcher>("_switcher");
            var bindingCtx = viewSwitcher.Children[SelectedViewIndex].BindingContext as BaseViewModel;
            bindingCtx.OnAppearing();
        }

        #endregion NavigableViewModel

        #region Services

        private IBookmarkService _bookmarkService;
        private INotificationService _notificationService;

        #endregion Services

        #region Fields

        private FootballMatchInfo _matchInfo;
        private int _selectedViewIndex;
        private FootballMatchDetailOverviewModel _overviewModel;
        private FootballMatchDetailH2HViewModel _h2hViewModel;
        private FootballMatchDetailPredictionsViewModel _predictionsViewModel;
        private FootballMatchDetailOddsViewModel _oddsViewModel;
        private ChangableIcon _alarmIcon;

        #endregion Fields

        #region Properties

        public ChangableIcon AlarmIcon { get => _alarmIcon; set => SetValue(ref _alarmIcon, value); }
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

            //await PageSwitcher.PopModalPageAsync();
            await PageSwitcher.PopNavPageAsync();

            SetIsBusy(false);
        }

        public ICommand TouchAlarmButtonCommand { get => new RelayCommand(TouchAlarmButton); }

        private async void TouchAlarmButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            MatchInfo.IsAlarmed = !MatchInfo.IsAlarmed;

            if (MatchInfo.IsAlarmed)
            {
                DateTime notifyTime = MatchInfo.MatchTime.AddMinutes(-5);
                if (notifyTime < DateTime.Now)
                    notifyTime = DateTime.Now.AddSeconds(10);

                await _notificationService.AddNotification(new NotificationInfo
                {
                    Id = MatchInfo.Id,
                    Title = LocalizeString.Match_Begin_Soon,
                    Description = $"{MatchInfo.LeagueName}.  {MatchInfo.HomeName}  vs  {MatchInfo.AwayName}",
                    IntentData = MatchInfo.JsonSerialize(),
                    IconName = "ic_soccer_alarm",
                    SportsType = SportsType.Football,
                    NotificationType = NotificationType.MatchStart,
                    NotifyTime = notifyTime,
                    StoredTime = DateTime.UtcNow,
                });
            }
            else
            {
                await _notificationService.DeleteNotification(MatchInfo.Id, SportsType.Football, NotificationType.MatchStart);
            }

            var message = MatchInfo.IsAlarmed ? LocalizeString.Set_Alarm : LocalizeString.Cancle_Alarm;
            UserDialogs.Instance.Toast(message);

            AlarmIcon.IsSelected = MatchInfo.IsAlarmed;

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
                await _bookmarkService.AddBookmark<FootballMatchInfo>(MatchInfo, SportsType.Football, BookMarkType.Match);
            else
                await _bookmarkService.RemoveBookmark<FootballMatchInfo>(MatchInfo, SportsType.Football, BookMarkType.Match);

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
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>()
                , ShinyHost.Resolve<MatchInfoToLeagueInfoConverter>().Convert(matchInfo, typeof(FootballLeagueInfo), null, CultureInfo.CurrentUICulture));

            SetIsBusy(false);
        }

        public ICommand HomeTeamLogoClickCommand { get => new RelayCommand<FootballMatchInfo>((e) => HomeTeamLogoClick(e)); }

        private async void HomeTeamLogoClick(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>()
                , ShinyHost.Resolve<MatchInfoToTeamInfoConverter>().Convert(matchInfo, typeof(FootballTeamInfo), TeamType.Home, CultureInfo.CurrentUICulture));

            SetIsBusy(false);
        }

        public ICommand AwayTeamLogoClickCommand { get => new RelayCommand<FootballMatchInfo>((e) => AwayTeamLogoClick(e)); }

        private async void AwayTeamLogoClick(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>()
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
            FootballMatchDetailPage page,
            IBookmarkService bookmarkService,
            INotificationService notificationService) : base(page)
        {
            _notificationService = notificationService;
            _bookmarkService = bookmarkService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }

            this.CoupledPage.FindByName<TabHostView>("_tabHost").SelectedTabIndexChanged += (s, e) =>
            {
                var viewSwitcher = this.CoupledPage.FindByName<ViewSwitcher>("_switcher");
                var bindingCtx = viewSwitcher.Children[(int)e.SelectedPosition].BindingContext as BaseViewModel;
                bindingCtx.OnAppearing();
            };
        }

        #endregion Constructors
    }
}