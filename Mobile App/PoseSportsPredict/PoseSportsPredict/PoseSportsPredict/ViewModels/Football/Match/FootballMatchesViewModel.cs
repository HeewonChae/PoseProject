using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
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
using PoseSportsPredict.Services;
using PoseSportsPredict.Services.Cache.Loader;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Match;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using PacketModels = PosePacket.Service.Football.Models;

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            string message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Match);
            MessagingCenter.Subscribe<BookmarkService, FootballMatchInfo>(this, message, (s, e) => BookmarkMessageHandler(e));

            message = _notificationService.BuildNotificationMessage(SportsType.Football, NotificationType.MatchStart);
            MessagingCenter.Subscribe<NotificationService, NotificationInfo>(this, message, (s, e) => NotificationMessageHandler(e));

            _alarmEditMode = false;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (MatchesTaskLoaderNotifier.IsSuccessfullyCompleted)
            {
                PullToRefresh();
                return;
            }

            if (MatchesTaskLoaderNotifier.IsNotStarted)
                MatchesTaskLoaderNotifier.Load(InitMatchesAsync);
        }

        #endregion NavigableViewModel

        #region Services

        private IBookmarkService _bookmarkService;
        private INotificationService _notificationService;
        private ICacheService _cacheService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _matchesTaskLoaderNotifier;
        private ObservableCollection<FootballMatchListViewModel> _matchListViewModels;
        private List<FootballMatchInfo> _matchList;
        private DateTime _matchDate;
        private DateTime _lastUpdateTime;
        private bool _alarmEditMode;
        private MatchFilterType _curMatchFilterType;
        private bool _isListViewRefrashing = false;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> MatchesTaskLoaderNotifier { get => _matchesTaskLoaderNotifier; set => SetValue(ref _matchesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballMatchListViewModel> MatchListViewModels { get => _matchListViewModels; set => SetValue(ref _matchListViewModels, value); }
        public bool IsListViewRefrashing { get => _isListViewRefrashing; set => SetValue(ref _isListViewRefrashing, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectGroupHeaderCommand { get => new RelayCommand<FootballMatchListViewModel>((e) => SelectGroupHeader(e)); }

        private void SelectGroupHeader(FootballMatchListViewModel groupInfo)
        {
            if (IsBusy)
                return;

            groupInfo.Expanded = !groupInfo.Expanded;

            MatchListViewModels = new ObservableCollection<FootballMatchListViewModel>(MatchListViewModels);
        }

        public ICommand MatchFilterCommand { get => new RelayCommand(MatchFilter); }

        private async void MatchFilter()
        {
            if (IsBusy)
                return;

            string isBookmarkFilter = _curMatchFilterType == MatchFilterType.Bookmark ? $"({LocalizeString.Applying})" : "";
            string isOngoingFilter = _curMatchFilterType == MatchFilterType.Ongoing ? $"({LocalizeString.Applying})" : "";
            string isTimeFilter = _curMatchFilterType == MatchFilterType.SortByTime ? $"({LocalizeString.Applying})" : "";
            string isLeagueFilter = _curMatchFilterType == MatchFilterType.SortByLeague ? $"({LocalizeString.Applying})" : "";
            var actions = new string[]
            {
                $"{LocalizeString.Match_Filter_By_Bookmark} {isBookmarkFilter}",
                $"{LocalizeString.Ongoing_matches} {isOngoingFilter}",
                $"{LocalizeString.Match_Sort_By_Time} {isTimeFilter}",
                $"{LocalizeString.Match_Sort_By_League} {isLeagueFilter}",
            };

            int intResult = await MaterialDialog.Instance.SelectActionAsync(LocalizeString.Select_Filter, actions, DialogConfiguration.DefaultSimpleDialogConfiguration);
            if (intResult == -1)
                return;

            intResult.TryParseEnum<MatchFilterType>(out MatchFilterType filterType);

            _curMatchFilterType = filterType;
            MatchesTaskLoaderNotifier.Load(UpdateFilteredMatchesAsync);
        }

        public ICommand AlarmEditModeCommand { get => new RelayCommand(AlarmEditMode); }

        private void AlarmEditMode()
        {
            if (IsBusy)
                return;

            _alarmEditMode = !_alarmEditMode;

            foreach (var MatchListViewModel in MatchListViewModels)
            {
                MatchListViewModel.AlarmEditMode = _alarmEditMode;
            }
        }

        public ICommand ExpandAllLeaguesCommand { get => new RelayCommand(ExpandAllLeagues); }

        private void ExpandAllLeagues()
        {
            if (IsBusy)
                return;

            foreach (var MatchListViewModel in MatchListViewModels)
            {
                MatchListViewModel.Expanded = true;
            }

            MatchListViewModels = new ObservableCollection<FootballMatchListViewModel>(MatchListViewModels);
        }

        public ICommand CollapseAllLeaguesCommand { get => new RelayCommand(CollapseAllLeagues); }

        private void CollapseAllLeagues()
        {
            if (IsBusy)
                return;

            foreach (var MatchListViewModel in MatchListViewModels)
            {
                MatchListViewModel.Expanded = false;
            }

            MatchListViewModels = new ObservableCollection<FootballMatchListViewModel>(MatchListViewModels);
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
                await RefreshMatchesAsync();

            IsListViewRefrashing = false;
            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchesViewModel(
            FootballMatchesPage page,
            IBookmarkService bookmarkService,
            INotificationService notificationService,
            ICacheService cacheService) : base(page)
        {
            _bookmarkService = bookmarkService;
            _notificationService = notificationService;
            _cacheService = cacheService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballMatchesViewModel SetMatchDate(DateTime date)
        {
            if (_matchDate == date.Date)
                return this;

            MatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();
            MatchListViewModels = null;
            _curMatchFilterType = MatchFilterType.SortByLeague;
            _matchDate = date.Date;

            UpdatePageTitle();

            return this;
        }

        public void UpdatePageTitle()
        {
            if (_matchDate == DateTime.Now.Date.AddDays(-1))
            {
                this.CoupledPage.Title = LocalizeString.Yesterday;
            }
            else if (_matchDate == DateTime.Now.Date)
            {
                this.CoupledPage.Title = LocalizeString.Today;
            }
            else if (_matchDate == DateTime.Now.Date.AddDays(1))
            {
                this.CoupledPage.Title = LocalizeString.Tomorrow;
            }
            else
            {
                this.CoupledPage.Title = _matchDate.ToString("ddd MM-dd");
            }
        }

        public void CultureInfoChanged()
        {
            if (MatchListViewModels?.Count > 0)
                MatchListViewModels = new ObservableCollection<FootballMatchListViewModel>(MatchListViewModels);
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitMatchesAsync()
        {
            this.SetIsBusy(true);

            await Task.Delay(300);

            TimeSpan expireTime = _matchDate == DateTime.Now.Date ? TimeSpan.FromMinutes(1) : DateTime.Now.Date.AddDays(1) - DateTime.Now;
            var result = await _cacheService.GetAsync<O_GET_FIXTURES_BY_DATE>(
                loader: () =>
                {
                    return FootballDataLoader.FixturesByDate(
                        _matchDate.ToUniversalTime(),
                        _matchDate.AddDays(1).AddMinutes(-1).ToUniversalTime());
                },
                key: $"P_GET_FIXTURES_BY_DATE:{_matchDate.ToString("yyyyMMdd")}",
                expireTime: expireTime,
                serializeType: SerializeType.MessagePack);

            if (result == null)
                throw new Exception(LocalizeString.Occur_Error);

            _matchList = new List<FootballMatchInfo>();

            var bookmarkedMatches = (await _bookmarkService.GetAllBookmark<FootballMatchInfo>())
                .Where(elem => _matchDate <= elem.MatchTime && elem.MatchTime < _matchDate.AddDays(1));
            var notifications = (await _notificationService.GetAllNotification(SportsType.Football, NotificationType.MatchStart))
                .Where(elem => _matchDate <= elem.NotifyTime && elem.NotifyTime < _matchDate.AddDays(1));

            foreach (var fixture in result.Fixtures)
            {
                var convertedMatchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(fixture);

                var bookmarkedMatch = bookmarkedMatches.FirstOrDefault(elem => elem.PrimaryKey == convertedMatchInfo.PrimaryKey);
                var notifiedMatch = notifications.FirstOrDefault(elem => elem.Id == convertedMatchInfo.Id);

                convertedMatchInfo.IsBookmarked = bookmarkedMatch?.IsBookmarked ?? false;
                convertedMatchInfo.IsAlarmed = notifiedMatch != null ? true : false;

                _matchList.Add(convertedMatchInfo);
            }

            if (_matchList.Count > 0)
                await UpdateFilteredMatchesAsync();

            _lastUpdateTime = DateTime.UtcNow;

            this.SetIsBusy(false);

            return _matchList;
        }

        private async Task RefreshMatchesAsync()
        {
            this.SetIsBusy(true);

            await Task.Delay(300);

            var needRefrashMatchIndexes = _matchList.Where(elem => elem.MatchTime < DateTime.Now
                   && elem.MatchStatus != FootballMatchStatusType.FT
                   && elem.MatchStatus != FootballMatchStatusType.AET
                   && elem.MatchStatus != FootballMatchStatusType.PEN)
                   .Select(elem => elem.Id).ToList();

            if (needRefrashMatchIndexes.Count() == 0)
            {
                _lastUpdateTime = DateTime.UtcNow;
                this.SetIsBusy(false);
                return;
            }

            var updatedMatches = await RefreshMatchInfos.Execute(needRefrashMatchIndexes.ToArray());
            foreach (var match in updatedMatches)
            {
                var foundIdx = _matchList.FindIndex(elem => elem.Id == match.Id);
                _matchList.RemoveAt(foundIdx);

                _matchList.Add(match);
                needRefrashMatchIndexes.Remove(match.Id);
            }

            foreach (var deletedIndex in needRefrashMatchIndexes)
            {
                var foundIdx = _matchList.FindIndex(elem => elem.Id == deletedIndex);
                _matchList.RemoveAt(foundIdx);
            }

            var filteredMatch = await UpdateFilteredMatchesAsync();

            _lastUpdateTime = DateTime.UtcNow;

            this.SetIsBusy(false);
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> UpdateFilteredMatchesAsync()
        {
            this.SetIsBusy(true);

            await Task.Delay(300);

            List<FootballMatchInfo> matchList = null;
            bool? isAllExpand = null;

            switch (_curMatchFilterType)
            {
                case MatchFilterType.Bookmark:
                    {
                        matchList = new List<FootballMatchInfo>();

                        // Match
                        var bookmarkedMatches = await _bookmarkService.GetAllBookmark<FootballMatchInfo>();
                        foreach (var bookmarkedMatch in bookmarkedMatches)
                        {
                            var match = _matchList.FirstOrDefault(elem => elem.PrimaryKey.Equals(bookmarkedMatch.PrimaryKey));
                            if (match != null)
                                matchList.Add(match);
                        }

                        // League
                        var bookmarkedLeagues = await _bookmarkService.GetAllBookmark<FootballLeagueInfo>();
                        foreach (var bookmarkedLeague in bookmarkedLeagues)
                        {
                            var matches = _matchList.Where(elem =>
                                elem.League_CountryName.Equals(bookmarkedLeague.CountryName)
                                && elem.LeagueName.Equals(bookmarkedLeague.LeagueName)).ToArray();

                            if (matches.Length > 0)
                            {
                                matchList.AddRange(matches);
                            }
                        }

                        // Team
                        var bookmarkedTeams = await _bookmarkService.GetAllBookmark<FootballTeamInfo>();
                        foreach (var bookmarkedTeam in bookmarkedTeams)
                        {
                            var matches = _matchList.Where(elem =>
                                 (elem.Home_CountryName.Equals(bookmarkedTeam.CountryName)
                                && elem.HomeName.Equals(bookmarkedTeam.TeamName))
                                || (elem.Away_CountryName.Equals(bookmarkedTeam.CountryName)
                                && elem.AwayName.Equals(bookmarkedTeam.TeamName))).ToArray();

                            if (matches.Length > 0)
                            {
                                matchList.AddRange(matches);
                            }
                        }

                        matchList = matchList.Distinct().OrderBy(elem => $"{elem.League_CountryName}:{elem.LeagueName}:{elem.MatchTime.ToString("HH:mm")}").ToList();
                        isAllExpand = true;
                    }
                    break;

                case MatchFilterType.Ongoing:
                    matchList = _matchList.Where(elem =>
                        elem.MatchStatus != FootballMatchStatusType.NS
                        && elem.MatchStatus != FootballMatchStatusType.FT
                        && elem.MatchStatus != FootballMatchStatusType.AET
                        && elem.MatchStatus != FootballMatchStatusType.PEN)
                        .OrderBy(elem => $"{elem.League_CountryName}:{elem.LeagueName}:{elem.MatchTime.ToString("HH:mm")}")
                        .ToList();

                    isAllExpand = true;
                    break;

                case MatchFilterType.SortByTime:
                    matchList = _matchList.OrderBy(elem => elem.MatchTime).ToList();
                    isAllExpand = true;
                    break;

                case MatchFilterType.SortByLeague:
                    matchList = _matchList.OrderBy(elem => $"{elem.League_CountryName}:{elem.LeagueName}:{elem.MatchTime.ToString("HH:mm")}").ToList();
                    break;
            }

            Debug.Assert(matchList != null);

            UpdateMatcheGroups(matchList, isAllExpand);

            this.SetIsBusy(false);

            return matchList;
        }

        private void UpdateMatcheGroups(List<FootballMatchInfo> matchList, bool? isAllExpand = null)
        {
            ObservableCollection<FootballMatchListViewModel> matchGroupCollection;
            matchGroupCollection = new ObservableCollection<FootballMatchListViewModel>();

            var grouppingMatches = MatchGroupingByFilterType(matchList, out string logo);

            bool defaultExpand = matchList.Count > 9 ? false : true;

            // World 데이터는 1순위로 등록
            var InternationalLeagues = grouppingMatches.Where(elem => elem.Key.Contains("World. "));
            foreach (var grouppingMatch in InternationalLeagues)
            {
                // isAllExpand 값이 null 이면 기존 groups의 expanded 값 사용
                bool isExpand = isAllExpand == null ? MatchListViewModels?.FirstOrDefault(elem => elem.Title == grouppingMatch.Key)?.Expanded ?? defaultExpand : isAllExpand.Value;

                var matchListViewModel = ShinyHost.Resolve<FootballMatchListViewModel>();
                matchListViewModel.Title = grouppingMatch.Key;
                matchListViewModel.TitleLogo = string.IsNullOrEmpty(logo) ? grouppingMatch.Value.FirstOrDefault()?.League_CountryLogo : logo;
                matchListViewModel.AlarmEditMode = _alarmEditMode;
                matchListViewModel.Matches = new ObservableCollection<FootballMatchInfo>(grouppingMatch.Value);
                matchListViewModel.Expanded = isExpand;

                matchGroupCollection.Add(matchListViewModel);
            }

            foreach (var grouppingMatch in grouppingMatches)
            {
                if (grouppingMatch.Key.Contains("World. "))
                    continue;

                // isAllExpand 값이 null 이면 기존 groups의 expanded 값 사용
                bool isExpand = isAllExpand == null ? MatchListViewModels?.FirstOrDefault(elem => elem.Title == grouppingMatch.Key)?.Expanded ?? defaultExpand : isAllExpand.Value;

                var matchListViewModel = ShinyHost.Resolve<FootballMatchListViewModel>();
                matchListViewModel.Title = grouppingMatch.Key;
                matchListViewModel.TitleLogo = string.IsNullOrEmpty(logo) ? grouppingMatch.Value.FirstOrDefault()?.League_CountryLogo : logo;
                matchListViewModel.AlarmEditMode = _alarmEditMode;
                matchListViewModel.Matches = new ObservableCollection<FootballMatchInfo>(grouppingMatch.Value);
                matchListViewModel.Expanded = isExpand;

                matchGroupCollection.Add(matchListViewModel);
            }

            MatchListViewModels = matchGroupCollection;
        }

        private Dictionary<string, FootballMatchInfo[]> MatchGroupingByFilterType(List<FootballMatchInfo> matchList, out string logo)
        {
            logo = string.Empty;
            var result = new Dictionary<string, FootballMatchInfo[]>();

            switch (_curMatchFilterType)
            {
                case MatchFilterType.Bookmark:
                case MatchFilterType.Ongoing:
                case MatchFilterType.SortByLeague:
                    var grouping = matchList.GroupBy(elem => $"{elem.League_CountryName}. {elem.LeagueName}");
                    foreach (var data in grouping)
                    {
                        result.Add(data.Key, data.ToArray());
                    }
                    break;

                case MatchFilterType.SortByTime:
                    logo = "img_world.png";
                    result.Add(LocalizeString.Match_Sort_By_Time, matchList.ToArray());
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            return result;
        }

        private void BookmarkMessageHandler(FootballMatchInfo item)
        {
            if (_matchList?.Count > 0)
            {
                var foundItem = _matchList.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.PrimaryKey));
                if (foundItem != null)
                {
                    foundItem.IsBookmarked = item.IsBookmarked;
                    foundItem.OnPropertyChanged("IsBookmarked");
                }
            }
        }

        private void NotificationMessageHandler(NotificationInfo item)
        {
            if (_matchList?.Count > 0)
            {
                var foundItem = _matchList.FirstOrDefault(elem => elem.PrimaryKey.Equals(item.Id.ToString()));
                if (foundItem != null)
                {
                    foundItem.IsAlarmed = item.IsAlarmed;
                    foundItem.OnPropertyChanged("IsAlarmed");
                }
            }
        }

        #endregion Methods
    }
}