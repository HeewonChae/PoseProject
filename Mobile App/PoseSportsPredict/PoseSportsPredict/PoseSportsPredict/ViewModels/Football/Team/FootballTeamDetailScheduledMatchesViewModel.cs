using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Enum;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.Cache.Loader;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.Views.Football.Team;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.ViewModels.Football.Team
{
    public class FootballTeamDetailScheduledMatchesViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            MatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (MatchesTaskLoaderNotifier.IsNotStarted)
                MatchesTaskLoaderNotifier.Load(InitMatchDatas);
        }

        #endregion BaseViewModel

        #region Services

        private ICacheService _cacheService;
        private IBookmarkService _bookmarkService;
        private INotificationService _notificationService;

        #endregion Services

        #region Fields

        private DateTime _lastUpdateTime;
        private bool _isListViewRefrashing;
        private bool _alarmEditMode;
        private FootballTeamInfo _teamInfo;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _matchesTaskLoaderNotifier;
        private ObservableList<FootballMatchInfo> _matches;
        public bool IsListViewRefrashing { get => _isListViewRefrashing; set => SetValue(ref _isListViewRefrashing, value); }

        #endregion Fields

        #region Properties

        public bool AlarmEditMode { get => _alarmEditMode; set => SetValue(ref _alarmEditMode, value); }
        public FootballTeamInfo TeamInfo { get => _teamInfo; set => SetValue(ref _teamInfo, value); }
        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> MatchesTaskLoaderNotifier { get => _matchesTaskLoaderNotifier; set => SetValue(ref _matchesTaskLoaderNotifier, value); }
        public ObservableList<FootballMatchInfo> Matches { get => _matches; set => SetValue(ref _matches, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectMatchCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch(e)); }

        private async void SelectMatch(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);

            SetIsBusy(false);
        }

        public ICommand SelectMatch_LongTapCommand { get => new RelayCommand<FootballMatchInfo>((e) => SelectMatch_LongTap(e)); }

        private void SelectMatch_LongTap(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            MatchInfoLongTapPopup.Execute(matchInfo);

            SetIsBusy(false);
        }

        public ICommand TouchAlarmButtonCommand { get => new RelayCommand<FootballMatchInfo>((e) => TouchAlarmButton(e)); }

        private async void TouchAlarmButton(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            if (!matchInfo.IsAlarmed)
            {
                DateTime notifyTime = matchInfo.MatchTime;
                await _notificationService.AddNotification(new NotificationInfo
                {
                    Id = matchInfo.Id,
                    Title = LocalizeString.Match_Begin_Soon,
                    Description = $"{matchInfo.LeagueName}.  {matchInfo.HomeName}  vs  {matchInfo.AwayName}",
                    IntentData = matchInfo.JsonSerialize(),
                    IconName = "ic_soccer_alarm",
                    SportsType = SportsType.Football,
                    NotificationType = NotificationType.MatchStart,
                    NotifyTime = notifyTime,
                    StoredTime = DateTime.UtcNow,
                });
            }
            else
            {
                await _notificationService.DeleteNotification(matchInfo.Id, SportsType.Football, NotificationType.MatchStart);
            }

            SetIsBusy(false);
        }

        public ICommand TouchBookmarkButtonCommand { get => new RelayCommand<FootballMatchInfo>((e) => TouchBookmarkButton(e)); }

        private async void TouchBookmarkButton(FootballMatchInfo matchInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            // Add Bookmark
            if (matchInfo.IsBookmarked)
                await _bookmarkService.RemoveBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, BookMarkType.Match);
            else
                await _bookmarkService.AddBookmark<FootballMatchInfo>(matchInfo, SportsType.Football, BookMarkType.Match);

            SetIsBusy(false);
        }

        public ICommand PullToRefreshCommand { get => new RelayCommand(PullToRefresh); }

        private async void PullToRefresh()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            IsListViewRefrashing = true;

            var timeSpan = DateTime.UtcNow - _lastUpdateTime;
            if (timeSpan.TotalMinutes > 1) // 1분 마다 갱신
                await InitMatchDatas();

            IsListViewRefrashing = false;
            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballTeamDetailScheduledMatchesViewModel(
            FootballTeamDetailScheduledMatchesView view,
            ICacheService cacheService,
            INotificationService notificationService,
            IBookmarkService bookmarkService) : base(view)
        {
            _cacheService = cacheService;
            _notificationService = notificationService;
            _bookmarkService = bookmarkService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballTeamDetailScheduledMatchesViewModel SetTeamInfo(FootballTeamInfo teamInfo)
        {
            TeamInfo = teamInfo;
            return this;
        }

        public async void EditAlarmMode()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            AlarmEditMode = !AlarmEditMode;

            await Task.Delay(300);

            SetIsBusy(false);
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitMatchDatas()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            var server_result = await _cacheService.GetAsync<O_GET_FIXTURES_BY_TEAM>(
               loader: () =>
               {
                   return FootballDataLoader.FixturesByTeam(
                       TeamInfo.TeamId,
                       SearchFixtureStatusType.Scheduled);
               },
               key: $"P_GET_FIXTURES_BY_TEAM:{TeamInfo.PrimaryKey}:{SearchFixtureStatusType.Scheduled}",
               expireTime: TimeSpan.FromMinutes(1),
               serializeType: SerializeType.MessagePack);

            var bookmarkedMatches = (await _bookmarkService.GetAllBookmark<FootballMatchInfo>())
               .Where(elem => elem.HomeTeamId == _teamInfo.TeamId || elem.AwayTeamId == _teamInfo.TeamId);
            var notifications = await _notificationService.GetAllNotification(SportsType.Football, NotificationType.MatchStart);

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            Matches = new ObservableList<FootballMatchInfo>();
            foreach (var fixture in server_result.Fixtures)
            {
                var convertedMatchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(fixture);

                var bookmarkedMatch = bookmarkedMatches.FirstOrDefault(elem => elem.PrimaryKey == convertedMatchInfo.PrimaryKey);
                var notifiedMatch = notifications.FirstOrDefault(elem => elem.Id == convertedMatchInfo.Id);

                convertedMatchInfo.IsBookmarked = bookmarkedMatch?.IsBookmarked ?? false;
                convertedMatchInfo.IsAlarmed = notifiedMatch != null ? true : false;

                Matches.Add(convertedMatchInfo);
            }

            _lastUpdateTime = DateTime.UtcNow;

            SetIsBusy(false);

            return Matches;
        }

        #endregion Methods
    }
}