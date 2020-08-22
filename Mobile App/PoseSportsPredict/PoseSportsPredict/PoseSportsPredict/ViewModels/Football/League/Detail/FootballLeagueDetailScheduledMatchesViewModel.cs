using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Enum;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.Logics.Football;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.Cache.Loader;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match;
using PoseSportsPredict.Views.Football.League.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.ViewModels.Football.League.Detail
{
    public class FootballLeagueDetailScheduledMatchesViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            MatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();

            _alarmEditMode = false;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (MatchesTaskLoaderNotifier.IsNotStarted)
                MatchesTaskLoaderNotifier.Load(InitMatcheDatas);
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
        private FootballLeagueInfo _leagueInfo;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _matchesTaskLoaderNotifier;
        public List<FootballMatchInfo> _matchList;
        private ObservableList<FootballMatchListViewModel> _matchListViewModels;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> MatchesTaskLoaderNotifier { get => _matchesTaskLoaderNotifier; set => SetValue(ref _matchesTaskLoaderNotifier, value); }
        public ObservableList<FootballMatchListViewModel> MatchListViewModels { get => _matchListViewModels; set => SetValue(ref _matchListViewModels, value); }
        public bool IsListViewRefrashing { get => _isListViewRefrashing; set => SetValue(ref _isListViewRefrashing, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectGroupHeaderCommand { get => new RelayCommand<FootballMatchListViewModel>((e) => SelectGroupHeader(e)); }

        private void SelectGroupHeader(FootballMatchListViewModel groupInfo)
        {
            if (IsBusy)
                return;

            groupInfo.Expanded = !groupInfo.Expanded;

            MatchListViewModels = new ObservableList<FootballMatchListViewModel>(MatchListViewModels);
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
                await InitMatcheDatas();

            IsListViewRefrashing = false;
            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballLeagueDetailScheduledMatchesViewModel(
            FootballLeagueDetailScheduledMatchesView view,
            IBookmarkService bookmarkService,
            INotificationService notificationService,
            ICacheService cacheService) : base(view)
        {
            _bookmarkService = bookmarkService;
            _notificationService = notificationService;
            _cacheService = cacheService;

            OnInitializeView();
        }

        #endregion Constructors

        public FootballLeagueDetailScheduledMatchesViewModel SetLeagueInfo(FootballLeagueInfo leagueInfo)
        {
            _leagueInfo = leagueInfo;
            return this;
        }

        #region Methods

        public async void AlarmEditMode()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            _alarmEditMode = !_alarmEditMode;

            foreach (var matchListViewModel in MatchListViewModels)
            {
                matchListViewModel.AlarmEditMode = _alarmEditMode;
            };

            await Task.Delay(300);

            SetIsBusy(false);
        }

        public async void ExpandAllMatches()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            foreach (var MatchListViewModel in MatchListViewModels)
            {
                MatchListViewModel.Expanded = true;
            }

            MatchListViewModels = new ObservableList<FootballMatchListViewModel>(MatchListViewModels);

            await Task.Delay(300);

            SetIsBusy(false);
        }

        public async void CollapseAllMatches()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            foreach (var MatchListViewModel in MatchListViewModels)
            {
                MatchListViewModel.Expanded = false;
            }

            MatchListViewModels = new ObservableList<FootballMatchListViewModel>(MatchListViewModels);

            await Task.Delay(300);

            SetIsBusy(false);
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitMatcheDatas()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            var server_result = await _cacheService.GetAsync<O_GET_FIXTURES_BY_LEAGUE>(
                loader: () =>
                {
                    return FootballDataLoader.FixturesByLeague(
                        _leagueInfo.CountryName,
                        _leagueInfo.LeagueName,
                        SearchFixtureStatusType.Scheduled);
                },
                key: $"P_GET_FIXTURES_BY_LEAGUE:{_leagueInfo.PrimaryKey}:{SearchFixtureStatusType.Scheduled}",
                expireTime: TimeSpan.FromMinutes(1),
                serializeType: SerializeType.MessagePack);

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            var bookmarkedMatches = (await _bookmarkService.GetAllBookmark<FootballMatchInfo>())
               .Where(elem => elem.LeagueName == _leagueInfo.LeagueName);
            var notifications = await _notificationService.GetAllNotification(SportsType.Football, NotificationType.MatchStart);

            _matchList = new List<FootballMatchInfo>();
            foreach (var fixture in server_result.Fixtures)
            {
                var convertedMatchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(fixture);

                var bookmarkedMatch = bookmarkedMatches.FirstOrDefault(elem => elem.PrimaryKey == convertedMatchInfo.PrimaryKey);
                var notifiedMatch = notifications.FirstOrDefault(elem => elem.Id == convertedMatchInfo.Id);

                convertedMatchInfo.IsBookmarked = bookmarkedMatch?.IsBookmarked ?? false;
                convertedMatchInfo.IsAlarmed = notifiedMatch != null ? true : false;

                _matchList.Add(convertedMatchInfo);
            }

            UpdateMatcheGroups(_matchList);

            _lastUpdateTime = DateTime.UtcNow;

            SetIsBusy(false);

            return _matchList;
        }

        private void UpdateMatcheGroups(List<FootballMatchInfo> matchList, bool? isAllExpand = null)
        {
            ObservableList<FootballMatchListViewModel> matchGroupCollection;
            matchGroupCollection = new ObservableList<FootballMatchListViewModel>();

            var grouppingMatches = matchList.GroupBy(elem => elem.MatchTime.Date.ToString("yyyy. MM. dd (ddd)"));
            foreach (var grouppingMatch in grouppingMatches)
            {
                // isAllExpand 값이 null 이면 기존 groups의 expanded 값 사용
                bool isExpand = isAllExpand == null ? MatchListViewModels?.FirstOrDefault(elem => elem.Title == grouppingMatch.Key)?.Expanded ?? true : isAllExpand.Value;

                var matchListViewModel = ShinyHost.Resolve<FootballMatchListViewModel>();
                matchListViewModel.Title = grouppingMatch.Key;
                matchListViewModel.TitleLogo = grouppingMatch.First().LeagueLogo;
                matchListViewModel.AlarmEditMode = _alarmEditMode;
                matchListViewModel.Matches = new ObservableCollection<FootballMatchInfo>(grouppingMatch.ToArray());
                matchListViewModel.Expanded = isExpand;
                matchListViewModel.IsPredicted = grouppingMatch.Any(elem => elem.IsPredicted);

                matchGroupCollection.Add(matchListViewModel);
            }

            MatchListViewModels = matchGroupCollection;
        }

        #endregion Methods
    }
}