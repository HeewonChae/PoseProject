using GalaSoft.MvvmLight.Command;
using PanCardView.Extensions;
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
using PoseSportsPredict.Models.Resources.Football;
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
using Xamarin.Forms.Internals;
using XF.Material.Forms.UI.Dialogs;
using PacketModels = PosePacket.Service.Football.Models;

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            string message = _bookmarkService.BuildBookmarkMessage(SportsType.Football, PageDetailType.Match);
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
        private ObservableList<FootballMatchListViewModel> _matchListViewModels;
        private List<FootballMatchInfo> _matchList;
        private DateTime _matchDate;
        private DateTime _lastUpdateTime;
        private bool _alarmEditMode;
        private MatchFilterType _curMatchFilterType;
        private bool _isListViewRefrashing = false;

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

            var itmeIdx = MatchListViewModels.FindIndex(groupInfo);
            MatchListViewModels.RemoveAt(itmeIdx);
            MatchListViewModels.Insert(itmeIdx, groupInfo);

            //MatchListViewModels = new ObservableList<FootballMatchListViewModel>(MatchListViewModels);
        }

        public ICommand MatchFilterCommand { get => new RelayCommand(MatchFilter); }

        private async void MatchFilter()
        {
            if (IsBusy)
                return;

            string isAllMatchesFilter = _curMatchFilterType == MatchFilterType.AllMatches ? $"({LocalizeString.Applying})" : "";
            string isBookmarkFilter = _curMatchFilterType == MatchFilterType.Bookmark ? $"({LocalizeString.Applying})" : "";
            //string isRecommendedFilter = _curMatchFilterType == MatchFilterType.Recommended ? $"({LocalizeString.Applying})" : "";
            string isOngoingFilter = _curMatchFilterType == MatchFilterType.Ongoing ? $"({LocalizeString.Applying})" : "";
            string isTimeFilter = _curMatchFilterType == MatchFilterType.SortByTime ? $"({LocalizeString.Applying})" : "";
            var actions = new string[]
            {
                $"{LocalizeString.All_Matches} {isAllMatchesFilter}",
                $"{LocalizeString.Match_Filter_By_Bookmark} {isBookmarkFilter}",
                //$"{LocalizeString.Recommended_Matches} {isRecommendedFilter}",
                $"{LocalizeString.Ongoing_matches} {isOngoingFilter}",
                $"{LocalizeString.Match_Sort_By_Time} {isTimeFilter}",
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

            MatchListViewModels = new ObservableList<FootballMatchListViewModel>(MatchListViewModels);
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
            _curMatchFilterType = MatchFilterType.AllMatches;
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
                MatchListViewModels = new ObservableList<FootballMatchListViewModel>(MatchListViewModels);
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitMatchesAsync()
        {
            this.SetIsBusy(true);

            await Task.Delay(300);

            TimeSpan expireTime = Math.Abs((_matchDate - DateTime.Now.Date).Days) <= 1 ? TimeSpan.Zero : DateTime.Now.Date.AddDays(1) - DateTime.Now;
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

            foreach (var remainIndex in needRefrashMatchIndexes)
            {
                var foundIdx = _matchList.FindIndex(elem => elem.Id == remainIndex);
                _matchList.RemoveAt(foundIdx);
            }

            await UpdateFilteredMatchesAsync();

            _lastUpdateTime = DateTime.UtcNow;

            this.SetIsBusy(false);
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> UpdateFilteredMatchesAsync()
        {
            this.SetIsBusy(true);

            await Task.Delay(300);

            List<FootballMatchInfo> matchList = null;
            switch (_curMatchFilterType)
            {
                case MatchFilterType.Bookmark:
                    {
                        var bookmarkedLeagues = await _bookmarkService.GetAllBookmark<FootballLeagueInfo>();
                        var bookmarkedTeams = await _bookmarkService.GetAllBookmark<FootballTeamInfo>();
                        var bookmarkedMatches = (await _bookmarkService.GetAllBookmark<FootballMatchInfo>()).Where(elem => elem.MatchTime >= _matchDate && _matchDate.AddDays(1) > elem.MatchTime);

                        matchList = _matchList.Where(elem =>
                            bookmarkedMatches.FirstOrDefault(innerElem => innerElem.PrimaryKey == elem.PrimaryKey) != null
                            || bookmarkedLeagues.FirstOrDefault(innerElem => innerElem.CountryName == elem.League_CountryName && innerElem.LeagueName == elem.LeagueName) != null
                            || bookmarkedTeams.FirstOrDefault(innerElem => innerElem.TeamId == elem.HomeTeamId || innerElem.TeamId == elem.AwayTeamId) != null)
                            .OrderBy(elem => elem.MatchTime).ToList();
                    }
                    break;

                //case MatchFilterType.Recommended:
                //    {
                //        matchList = _matchList.Where(elem => elem.IsRecommended).OrderBy(elem => elem.MatchTime).ToList();
                //    }
                //    break;

                case MatchFilterType.Ongoing:
                    matchList = _matchList.Where(elem =>
                        elem.MatchStatus != FootballMatchStatusType.NS
                        && elem.MatchStatus != FootballMatchStatusType.FT
                        && elem.MatchStatus != FootballMatchStatusType.AET
                        && elem.MatchStatus != FootballMatchStatusType.PEN)
                        .OrderBy(elem => elem.MatchTime).ToList();
                    break;

                case MatchFilterType.SortByTime:
                case MatchFilterType.AllMatches:
                    matchList = _matchList.OrderBy(elem => elem.MatchTime).ToList();
                    break;
            }

            Debug.Assert(matchList != null);

            UpdateMatcheGroups(matchList);

            this.SetIsBusy(false);

            return matchList;
        }

        private async void UpdateMatcheGroups(List<FootballMatchInfo> matchList)
        {
            var matchGroupCollection = new ObservableList<FootballMatchListViewModel>();

            // 추천 경기
            var recommendedMatches = _matchList.Where(elem => elem.IsRecommended && elem.MaxRating >= 4.5 && elem.MatchTime > DateTime.Now.AddHours(-1)).ToArray();
            if (recommendedMatches.Length > 0)
            {
                var recommendedMatchesViewModel = ShinyHost.Resolve<FootballMatchListViewModel>();
                recommendedMatchesViewModel.CurrentIndex = MatchListViewModels?.First().CurrentIndex ?? 0;
                recommendedMatchesViewModel.GroupType = MatchGroupType.Recommand;
                recommendedMatchesViewModel.AlarmEditMode = _alarmEditMode;
                recommendedMatchesViewModel.Matches = new ObservableCollection<FootballMatchInfo>(recommendedMatches.OrderBy(elem => elem.MatchTime));
                matchGroupCollection.Add(recommendedMatchesViewModel);

#if !REMOVE_ADS
                // 네이티브 광고
                var nativeAds = ShinyHost.Resolve<FootballMatchListViewModel>();
                nativeAds.GroupType = MatchGroupType.NativeAds;
                nativeAds.AdsBannerType = AdsBannerType.NativeSmall;
                matchGroupCollection.Add(nativeAds);
#endif
            }

            var grouppingMatches = await MatchGroupingByFilterType(matchList);

            // 경기 그룹 리스트
            foreach (var grouppingMatch in grouppingMatches)
            {
                var matchListViewModel = ShinyHost.Resolve<FootballMatchListViewModel>();
                matchListViewModel.GroupType = MatchGroupType.Default;
                matchListViewModel.Title = grouppingMatch.Key;
                matchListViewModel.TitleLogo = grouppingMatch.GroupLogo;
                matchListViewModel.AlarmEditMode = _alarmEditMode;
                matchListViewModel.Matches = new ObservableCollection<FootballMatchInfo>(grouppingMatch.Matches);
                matchListViewModel.IsPredicted = grouppingMatch.Matches.Any(elem => elem.IsPredicted);

                var existGroup = _matchListViewModels?.FirstOrDefault(elem => elem.Title == grouppingMatch.Key);
                matchListViewModel.Expanded = grouppingMatch.IsExpanded ? true : existGroup?.Expanded ?? grouppingMatch.IsExpanded;

                matchGroupCollection.Add(matchListViewModel);
            }
#if !REMOVE_ADS
            // 그룹 개수에 따라 동적으로 광고 처리
            int totalGroupCnt = grouppingMatches.Count;
            if (totalGroupCnt > 24)
            {
                var mediumNativeAds = ShinyHost.Resolve<FootballMatchListViewModel>();
                mediumNativeAds.GroupType = MatchGroupType.NativeAds;
                mediumNativeAds.AdsBannerType = AdsBannerType.NativeMedium;
                matchGroupCollection.Insert(14, mediumNativeAds);

                var mediumNativeAds2 = ShinyHost.Resolve<FootballMatchListViewModel>();
                mediumNativeAds2.GroupType = MatchGroupType.NativeAds;
                mediumNativeAds2.AdsBannerType = AdsBannerType.NativeMedium2;
                matchGroupCollection.Insert(matchGroupCollection.Count, mediumNativeAds2);
            }
            else
            {
                var mediumNativeAds = ShinyHost.Resolve<FootballMatchListViewModel>();
                mediumNativeAds.GroupType = MatchGroupType.NativeAds;
                mediumNativeAds.AdsBannerType = AdsBannerType.NativeMedium;

                matchGroupCollection.Insert(matchGroupCollection.Count, mediumNativeAds);
            }
#endif

            MatchListViewModels = matchGroupCollection;
        }

        private async Task<List<FootballMatchGroupInfo>> MatchGroupingByFilterType(List<FootballMatchInfo> matchList)
        {
            var result = new List<FootballMatchGroupInfo>();

            switch (_curMatchFilterType)
            {
                case MatchFilterType.Bookmark:
                    {
                        var grouping = matchList.GroupBy(elem => new { Country = elem.League_CountryName, CountryLogo = elem.League_CountryLogo, League = elem.LeagueName });
                        foreach (var data in grouping)
                        {
                            result.Add(new FootballMatchGroupInfo
                            {
                                Country = data.Key.Country,
                                League = data.Key.League,
                                GroupLogo = data.Key.CountryLogo,
                                Matches = data.ToArray(),
                                IsExpanded = true
                            });
                        }
                    }
                    break;

                //case MatchFilterType.Recommended:
                case MatchFilterType.Ongoing:
                case MatchFilterType.AllMatches:
                    {
                        var grouping = matchList.GroupBy(elem => new { Country = elem.League_CountryName, CountryLogo = elem.League_CountryLogo, League = elem.LeagueName });
                        var bookmarkedLeagues = await _bookmarkService.GetAllBookmark<FootballLeagueInfo>();
                        var bookmarkedTeams = await _bookmarkService.GetAllBookmark<FootballTeamInfo>();
                        var bookmarkedMatches = (await _bookmarkService.GetAllBookmark<FootballMatchInfo>()).Where(elem => elem.MatchTime >= _matchDate && _matchDate.AddDays(1) > elem.MatchTime);

                        //var bookmarked = new List<FootballMatchGroupInfo>();
                        var recommend = new List<FootballMatchGroupInfo>();
                        var world = new List<FootballMatchGroupInfo>();
                        var other = new List<FootballMatchGroupInfo>();

                        // 1순위 북마크(리그, 경기, 팀), 2순위 추천 리그, 3순위 월드리그, 4순위 나머지
                        foreach (var data in grouping)
                        {
                            var speratedData = data.SeperateByCount(15);

                            //if (bookmarkedLeagues.FirstOrDefault(elem => elem.CountryName == data.Key.Country && elem.LeagueName == data.Key.League) != null
                            //    || bookmarkedMatches.FirstOrDefault(elem => data.ToArray().FirstOrDefault(elem2 => elem2.PrimaryKey == elem.PrimaryKey) != null) != null
                            //    || bookmarkedTeams.FirstOrDefault(elem => data.ToArray().FirstOrDefault(elem2 => elem.TeamId == elem2.HomeTeamId || elem.TeamId == elem2.AwayTeamId) != null) != null)
                            //{
                            //    if (speratedData.Count > 1)
                            //    {
                            //        for (int i = 0; i < speratedData.Count; i++)
                            //        {
                            //            bookmarked.Add(new FootballMatchGroupInfo
                            //            {
                            //                Country = data.Key.Country,
                            //                League = data.Key.League,
                            //                GroupLogo = data.Key.CountryLogo,
                            //                Matches = speratedData[i],
                            //                GroupSubString = (i + 1).ToString(),
                            //                IsExpanded = false
                            //            });
                            //        }
                            //    }
                            //    else
                            //    {
                            //        bookmarked.Add(new FootballMatchGroupInfo
                            //        {
                            //            Country = data.Key.Country,
                            //            League = data.Key.League,
                            //            GroupLogo = data.Key.CountryLogo,
                            //            Matches = data.ToArray(),
                            //            IsExpanded = false
                            //        });
                            //    }
                            //}
                            //else if (RecommendedLeague.HasLeague(data.Key.Country, data.Key.League))
                            if (RecommendedLeague.HasLeague(data.Key.Country, data.Key.League))
                            {
                                if (speratedData.Count > 1)
                                {
                                    for (int i = 0; i < speratedData.Count; i++)
                                    {
                                        recommend.Add(new FootballMatchGroupInfo
                                        {
                                            Country = data.Key.Country,
                                            League = data.Key.League,
                                            GroupLogo = data.Key.CountryLogo,
                                            Matches = speratedData[i],
                                            GroupSubString = (i + 1).ToString(),
                                            IsExpanded = true
                                        });
                                    }
                                }
                                else
                                {
                                    recommend.Add(new FootballMatchGroupInfo
                                    {
                                        Country = data.Key.Country,
                                        League = data.Key.League,
                                        GroupLogo = data.Key.CountryLogo,
                                        Matches = data.ToArray(),
                                        IsExpanded = true
                                    });
                                }
                            }
                            else if (data.Key.Country == "World")
                            {
                                if (speratedData.Count > 1)
                                {
                                    for (int i = 0; i < speratedData.Count; i++)
                                    {
                                        world.Add(new FootballMatchGroupInfo
                                        {
                                            Country = data.Key.Country,
                                            League = data.Key.League,
                                            GroupLogo = data.Key.CountryLogo,
                                            Matches = speratedData[i],
                                            GroupSubString = (i + 1).ToString(),
                                            IsExpanded = false
                                        });
                                    }
                                }
                                else
                                {
                                    world.Add(new FootballMatchGroupInfo
                                    {
                                        Country = data.Key.Country,
                                        League = data.Key.League,
                                        GroupLogo = data.Key.CountryLogo,
                                        Matches = data.ToArray(),
                                        IsExpanded = false
                                    });
                                }
                            }
                            else
                            {
                                if (speratedData.Count > 1)
                                {
                                    for (int i = 0; i < speratedData.Count; i++)
                                    {
                                        other.Add(new FootballMatchGroupInfo
                                        {
                                            Country = data.Key.Country,
                                            League = data.Key.League,
                                            GroupLogo = data.Key.CountryLogo,
                                            Matches = speratedData[i],
                                            GroupSubString = (i + 1).ToString(),
                                            IsExpanded = false
                                        });
                                    }
                                }
                                else
                                {
                                    other.Add(new FootballMatchGroupInfo
                                    {
                                        Country = data.Key.Country,
                                        League = data.Key.League,
                                        GroupLogo = data.Key.CountryLogo,
                                        Matches = data.ToArray(),
                                        IsExpanded = false
                                    });
                                }
                            }
                        }

                        recommend.AddRange(world);
                        recommend.AddRange(other);

                        result = recommend;
                    }
                    break;

                case MatchFilterType.SortByTime:
                    {
                        var speratedData = matchList.SeperateByCount(15);
                        if (speratedData.Count > 1)
                        {
                            for (int i = 0; i < speratedData.Count; i++)
                            {
                                result.Add(new FootballMatchGroupInfo
                                {
                                    GroupLogo = "img_world.png",
                                    Matches = speratedData[i],
                                    GroupSubString = $"{speratedData[i].First().MatchTime:tt hh:mm} ~",
                                    IsExpanded = false
                                });
                            }
                        }
                        else
                        {
                            result.Add(new FootballMatchGroupInfo
                            {
                                Country = LocalizeString.Match_Sort_By_Time,
                                GroupLogo = "img_world.png",
                                Matches = matchList.ToArray(),
                                IsExpanded = true
                            });
                        }
                    }
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