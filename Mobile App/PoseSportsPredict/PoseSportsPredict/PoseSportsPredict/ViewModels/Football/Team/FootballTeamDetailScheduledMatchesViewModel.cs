using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.Views.Football.Team;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
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
            if (!MatchesTaskLoaderNotifier.IsNotStarted)
                return;

            MatchesTaskLoaderNotifier.Load(InitMatchDatas);
        }

        #endregion BaseViewModel

        #region Services

        private IWebApiService _webApiService;
        private IBookmarkService _bookmarkService;
        private INotificationService _notificationService;

        #endregion Services

        #region Fields

        private bool _alarmEditMode;
        private FootballTeamInfo _teamInfo;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _matchesTaskLoaderNotifier;
        private ObservableList<FootballMatchInfo> _matches;

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

        #endregion Commands

        #region Constructors

        public FootballTeamDetailScheduledMatchesViewModel(
            FootballTeamDetailScheduledMatchesView view,
            IWebApiService webApiService,
            INotificationService notificationService,
            IBookmarkService bookmarkService) : base(view)
        {
            _webApiService = webApiService;
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
            var server_result = await _webApiService.RequestAsyncWithToken<O_GET_FIXTURES_BY_TEAM>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_TEAM,
                PostData = new I_GET_FIXTURES_BY_TEAM
                {
                    SearchFixtureStatusType = SearchFixtureStatusType.Scheduled,
                    TeamId = TeamInfo.TeamId,
                }
            });

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            Matches = new ObservableList<FootballMatchInfo>();
            foreach (var fixture in server_result.Fixtures)
            {
                Matches.Add(ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(fixture));
            }

            SetIsBusy(false);

            return Matches;
        }

        #endregion Methods
    }
}