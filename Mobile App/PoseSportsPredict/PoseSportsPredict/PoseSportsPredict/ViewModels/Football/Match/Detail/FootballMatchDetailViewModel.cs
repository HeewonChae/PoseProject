﻿using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Team;
using PoseSportsPredict.Views.Football.Match.Detail;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using Shiny;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballMatchDetailViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            var message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Match);
            MessagingCenter.Subscribe<BookmarkService, FootballMatchInfo>(this, message, (s, e) => MatchBookmarkMessageHandler(e));

            message = _notificationService.BuildNotificationMessage(SportsType.Football, NotificationType.MatchStart);
            MessagingCenter.Subscribe<NotificationService, NotificationInfo>(this, message, (s, e) => NotificationMessageHandler(e));

            AlarmIcon = new ChangableIcon(
                "ic_alarm_selected.png",
                AppResourcesHelper.GetResourceColor("IconActivated"),
                "ic_alarm_unselected.png",
                AppResourcesHelper.GetResourceColor("TextColor_L"));

            return true;
        }

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            if (datas == null)
                return false;

            if (!(datas[0] is FootballMatchInfo matchInfo))
                return false;

            // Check Bookmark
            var bookmarkedMatch = await _bookmarkService.GetBookmark<FootballMatchInfo>(matchInfo.PrimaryKey);
            matchInfo.IsBookmarked = bookmarkedMatch?.IsBookmarked ?? false;

            // Check Alarm
            var notification = await _notificationService.GetNotification(matchInfo.Id, SportsType.Football, NotificationType.MatchStart);
            matchInfo.IsAlarmed = notification != null ? true : false;
            AlarmIcon.IsSelected = matchInfo.IsAlarmed;

            MatchInfo = matchInfo;

            OverviewModel = ShinyHost.Resolve<FootballMatchDetailOverviewModel>().SetMatchInfo(MatchInfo);
            H2HViewModel = ShinyHost.Resolve<FootballMatchDetailH2HViewModel>().SetMatchInfo(MatchInfo);
            OddsViewModel = ShinyHost.Resolve<FootballMatchDetailOddsViewModel>();
            PredictionsViewModel = ShinyHost.Resolve<FootballMatchDetailPredictionsViewModel>();

            TabContents = new ObservableCollection<BaseViewModel>
            {
                OverviewModel,
                H2HViewModel,
                OddsViewModel,
                PredictionsViewModel,
            };

            SelectedViewIndex = 0;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            _tabContents[SelectedViewIndex].OnAppearing();
        }

        #endregion NavigableViewModel

        #region Services

        private IWebApiService _webApiService;
        private IBookmarkService _bookmarkService;
        private INotificationService _notificationService;

        #endregion Services

        #region Fields

        private DateTime _LastUpdateTime = DateTime.Now;

        private FootballMatchInfo _matchInfo;
        private int _selectedViewIndex;
        private FootballMatchDetailOverviewModel _overviewModel;
        private FootballMatchDetailH2HViewModel _h2HViewModel;
        private FootballMatchDetailPredictionsViewModel _predictionsViewModel;
        private FootballMatchDetailOddsViewModel _oddsViewModel;
        private ObservableCollection<BaseViewModel> _tabContents;
        private ChangableIcon _alarmIcon;

        #endregion Fields

        #region Properties

        public ChangableIcon AlarmIcon { get => _alarmIcon; set => SetValue(ref _alarmIcon, value); }
        public FootballMatchInfo MatchInfo { get => _matchInfo; set => SetValue(ref _matchInfo, value); }
        public int SelectedViewIndex { get => _selectedViewIndex; set => SetValue(ref _selectedViewIndex, value); }
        public FootballMatchDetailOverviewModel OverviewModel { get => _overviewModel; set => SetValue(ref _overviewModel, value); }
        public FootballMatchDetailH2HViewModel H2HViewModel { get => _h2HViewModel; set => SetValue(ref _h2HViewModel, value); }
        public FootballMatchDetailOddsViewModel OddsViewModel { get => _oddsViewModel; set => SetValue(ref _oddsViewModel, value); }
        public FootballMatchDetailPredictionsViewModel PredictionsViewModel { get => _predictionsViewModel; set => SetValue(ref _predictionsViewModel, value); }
        public ObservableCollection<BaseViewModel> TabContents { get => _tabContents; set => SetValue(ref _tabContents, value); }

        #endregion Properties

        #region Commands

        public ICommand TouchBackButtonCommand { get => new RelayCommand(TouchBackButton); }

        private async void TouchBackButton()
        {
            if (IsPageSwitched)
                return;

            SetIsPageSwitched(true);

            var message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Match);
            MessagingCenter.Unsubscribe<BookmarkService, FootballMatchInfo>(this, message);

            message = _notificationService.BuildNotificationMessage(SportsType.Football, NotificationType.MatchStart);
            MessagingCenter.Unsubscribe<NotificationService, NotificationInfo>(this, message);

            await PageSwitcher.PopNavPageAsync();
        }

        public ICommand TouchRefrashButtonCommand { get => new RelayCommand(TouchRefrashButton); }

        private async void TouchRefrashButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            MatchInfo = await UpdateMatchInfo();

            SetIsBusy(false);
        }

        public ICommand TouchAlarmButtonCommand { get => new RelayCommand(TouchAlarmButton); }

        private async void TouchAlarmButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            if (!MatchInfo.IsAlarmed)
            {
                DateTime notifyTime = MatchInfo.MatchTime;
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

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand(TouchBookmarkButton); }

        private async void TouchBookmarkButton()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            // Add Bookmark
            if (MatchInfo.IsBookmarked)
                await _bookmarkService.RemoveBookmark<FootballMatchInfo>(MatchInfo, SportsType.Football, BookMarkType.Match);
            else
                await _bookmarkService.AddBookmark<FootballMatchInfo>(MatchInfo, SportsType.Football, BookMarkType.Match);

            SetIsBusy(false);
        }

        public ICommand LeagueNameClickCommand { get => new RelayCommand<FootballMatchInfo>((e) => LeagueNameClick(e)); }

        private async void LeagueNameClick(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>()
                , ShinyHost.Resolve<MatchInfoToLeagueInfo>().Convert(matchInfo));

            SetIsBusy(false);
        }

        public ICommand HomeTeamLogoClickCommand { get => new RelayCommand<FootballMatchInfo>((e) => HomeTeamLogoClick(e)); }

        private async void HomeTeamLogoClick(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>()
                , ShinyHost.Resolve<MatchInfoToTeamInfo>().Convert(matchInfo, TeamCampType.Home));

            SetIsBusy(false);
        }

        public ICommand AwayTeamLogoClickCommand { get => new RelayCommand<FootballMatchInfo>((e) => AwayTeamLogoClick(e)); }

        private async void AwayTeamLogoClick(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>()
                , ShinyHost.Resolve<MatchInfoToTeamInfo>().Convert(matchInfo, TeamCampType.Away));

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchDetailViewModel(
            FootballMatchDetailPage page,
            IBookmarkService bookmarkService,
            INotificationService notificationService,
            IWebApiService webApiService) : base(page)
        {
            _webApiService = webApiService;
            _notificationService = notificationService;
            _bookmarkService = bookmarkService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }

            this.CoupledPage.FindByName<SfTabView>("_tabView").SelectionChanged
                += (s, e) => _tabContents[SelectedViewIndex].OnAppearing();
        }

        #endregion Constructors

        #region Methods

        private void MatchBookmarkMessageHandler(FootballMatchInfo item)
        {
            if (MatchInfo.PrimaryKey == item.PrimaryKey)
            {
                MatchInfo.IsBookmarked = item.IsBookmarked;
                MatchInfo.OnPropertyChanged("IsBookmarked");
            }
        }

        private void NotificationMessageHandler(NotificationInfo item)
        {
            if (MatchInfo.Id == item.Id)
            {
                MatchInfo.IsAlarmed = item.IsAlarmed;
                AlarmIcon.IsSelected = item.IsAlarmed;
            }
        }

        private async Task<FootballMatchInfo> UpdateMatchInfo()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            var timeSpan = DateTime.Now - _LastUpdateTime;
            if (timeSpan.TotalMinutes < 5)
            {
                SetIsBusy(false);
                return MatchInfo;
            }

            // call server
            var server_result = await _webApiService.RequestAsyncWithToken<O_GET_FIXTURES_BY_INDEX>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_INDEX,
                PostData = new I_GET_FIXTURES_BY_INDEX
                {
                    FixtureIds = new int[] { MatchInfo.Id },
                }
            });

            if (server_result == null || server_result.Fixtures.Length == 0)
            {
                SetIsBusy(false);
                return MatchInfo;
            }

            var matchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(server_result.Fixtures[0]);
            matchInfo.IsBookmarked = MatchInfo.IsBookmarked;
            matchInfo.IsAlarmed = MatchInfo.IsAlarmed;
            AlarmIcon.IsSelected = MatchInfo.IsAlarmed;

            _LastUpdateTime = DateTime.Now;

            return matchInfo;
        }

        #endregion Methods
    }
}