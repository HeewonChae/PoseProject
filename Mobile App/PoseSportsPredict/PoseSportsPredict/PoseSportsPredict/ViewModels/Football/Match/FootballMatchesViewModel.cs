using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.Views.Football.Match;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
            MatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();

            string message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, BookMarkType.Match);
            MessagingCenter.Subscribe<BookmarkService, FootballMatchInfo>(this, message, (s, e) => BookmarkMessageHandler(e));

            message = _notificationService.BuildNotificationMessage(SportsType.Football, NotificationType.MatchStart);
            MessagingCenter.Subscribe<NotificationService, NotificationInfo>(this, message, (s, e) => NotificationMessageHandler(e));

            _alarmEditMode = false;
            _lastUpdateTime = DateTime.MinValue;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            var timeSpan = DateTime.UtcNow - _lastUpdateTime;
#if DEBUG
            if (!MatchesTaskLoaderNotifier.IsNotStarted && timeSpan.TotalMinutes < 1) // 1분 마다 갱신
                return;
#else
            if (!MatchesTaskLoaderNotifier.IsNotStarted && timeSpan.TotalMinutes < 15) // 15분 마다 갱신
                return;
#endif
            MatchesTaskLoaderNotifier.Load(InitMatchesAsync);
        }

        #endregion NavigableViewModel

        #region Services

        private IWebApiService _webApiService;
        private IBookmarkService _bookmarkService;
        private INotificationService _notificationService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _matchesTaskLoaderNotifier;
        private ObservableCollection<FootballMatchListViewModel> _matchListViewModels;
        private List<FootballMatchInfo> _matchList;
        private DateTime _matchDate;
        private DateTime _lastUpdateTime;
        private bool _alarmEditMode;
        private MatchFilterType _curMatchFilterType;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> MatchesTaskLoaderNotifier { get => _matchesTaskLoaderNotifier; set => SetValue(ref _matchesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballMatchListViewModel> MatchListViewModels { get => _matchListViewModels; set => SetValue(ref _matchListViewModels, value); }

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

        #endregion Commands

        #region Constructors

        public FootballMatchesViewModel(
            FootballMatchesPage page,
            IWebApiService webApiService,
            IBookmarkService bookmarkService,
            INotificationService notificationService) : base(page)
        {
            _webApiService = webApiService;
            _bookmarkService = bookmarkService;
            _notificationService = notificationService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballMatchesViewModel SetMatchDate(DateTime date)
        {
            if (_matchDate == date)
                return this;

            _matchDate = date;
            _lastUpdateTime = DateTime.MinValue;

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
                this.CoupledPage.Title = _matchDate.ToString("ddd dd MMM");
            }
            return this;
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitMatchesAsync()
        {
            this.SetIsBusy(true);

            var result = await _webApiService.RequestAsyncWithToken<O_GET_FIXTURES_BY_DATE>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_FIXTURES_BY_DATE,
                PostData = new I_GET_FIXTURES_BY_DATE
                {
                    StartTime = _matchDate.ToUniversalTime(),
                    EndTime = _matchDate.ToUniversalTime().AddDays(1).AddSeconds(-1),
                }
            });

            if (result == null)
                throw new Exception(LocalizeString.Occur_Error);

            _matchList = new List<FootballMatchInfo>();

            var bookmarkedMatches = (await _bookmarkService.GetAllBookmark<FootballMatchInfo>())
                .Where(elem => _matchDate.AddDays(-1) < elem.MatchTime && elem.MatchTime < _matchDate.AddDays(1));
            var notifications = (await _notificationService.GetAllNotification(SportsType.Football, NotificationType.MatchStart))
                .Where(elem => _matchDate.AddDays(-1) < elem.NotifyTime && elem.NotifyTime < _matchDate.AddDays(1));

            foreach (var fixture in result.Fixtures)
            {
                var convertedMatchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(fixture);

                var bookmarkedMatch = bookmarkedMatches.FirstOrDefault(elem => elem.PrimaryKey == convertedMatchInfo.PrimaryKey);
                var notifiedMatch = notifications.FirstOrDefault(elem => elem.Id == convertedMatchInfo.Id);

                convertedMatchInfo.IsBookmarked = bookmarkedMatch?.IsBookmarked ?? false;
                convertedMatchInfo.IsAlarmed = notifiedMatch != null ? true : false;

                _matchList.Add(convertedMatchInfo);
            }

            _curMatchFilterType = MatchFilterType.SortByLeague;
            var filteredMatch = await UpdateFilteredMatchesAsync();

            _lastUpdateTime = DateTime.UtcNow;

            this.SetIsBusy(false);

            return filteredMatch;
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
                            var matches = _matchList.Where(elem => elem.LeagueName.Equals(bookmarkedLeague.LeagueName)).ToArray();

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
                                elem.HomeName.Equals(bookmarkedTeam.TeamName)
                                || elem.AwayName.Equals(bookmarkedTeam.TeamName)).ToArray();

                            if (matches.Length > 0)
                            {
                                matchList.AddRange(matches);
                            }
                        }

                        matchList = matchList.Distinct().OrderBy(elem => elem.MatchTime).ToList();
                        isAllExpand = true;
                    }
                    break;

                case MatchFilterType.Ongoing:
                    matchList = _matchList.Where(elem =>
                        elem.MatchStatus != PacketModels.Enums.FootballMatchStatusType.NS
                        && elem.MatchStatus != PacketModels.Enums.FootballMatchStatusType.FT
                        && elem.MatchStatus != PacketModels.Enums.FootballMatchStatusType.AET
                        && elem.MatchStatus != PacketModels.Enums.FootballMatchStatusType.PEN)
                        .ToList();

                    isAllExpand = true;
                    break;

                case MatchFilterType.SortByTime:
                    matchList = _matchList.OrderBy(elem => elem.MatchTime).ToList();
                    isAllExpand = true;
                    break;

                case MatchFilterType.SortByLeague:
                    matchList = _matchList.OrderBy(elem => elem.MatchTime).ToList();
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
            foreach (var grouppingMatch in grouppingMatches)
            {
                // isAllExpand 값이 null 이면 기존 groups의 expanded 값 사용
                bool isExpand = isAllExpand == null ? MatchListViewModels?.FirstOrDefault(elem => elem.Title == grouppingMatch.Key)?.Expanded ?? false : isAllExpand.Value;

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
                    result.Add(LocalizeString.Match_Sort_By_Time, matchList.OrderBy(elem => elem.MatchTime).ToArray());
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