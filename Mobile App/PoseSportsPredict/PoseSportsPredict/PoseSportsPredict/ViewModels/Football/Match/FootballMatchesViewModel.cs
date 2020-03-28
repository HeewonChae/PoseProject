using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Match;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            MatchesTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>(GetMatchesAsync);
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
            MatchesTaskLoaderNotifier.Load();
        }

        #endregion NavigableViewModel

        #region Services

        private IWebApiService _webApiService;
        private ISQLiteService _sqliteService;

        #endregion Services

        #region Fields

        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _matchesTaskLoaderNotifier;
        private ObservableCollection<FootballMatchGroup> _matchGroups;
        private List<FootballMatchInfo> _matchList;
        private DateTime _matchDate;
        private DateTime _lastUpdateTime;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> MatchesTaskLoaderNotifier { get => _matchesTaskLoaderNotifier; set => SetValue(ref _matchesTaskLoaderNotifier, value); }
        public ObservableCollection<FootballMatchGroup> MatchGroups { get => _matchGroups; set => SetValue(ref _matchGroups, value); }

        #endregion Properties

        #region Commands

        public ICommand SelectGroupHeaderCommand { get => new RelayCommand<FootballMatchGroup>((e) => SelectGroupHeader(e)); }

        private void SelectGroupHeader(FootballMatchGroup groupInfo)
        {
            groupInfo.Expanded = !groupInfo.Expanded;

            MatchGroups = new ObservableCollection<FootballMatchGroup>(MatchGroups);
        }

        public ICommand MatchFilterCommand { get => new RelayCommand(MatchFilter); }

        private async void MatchFilter()
        {
            if (!MatchesTaskLoaderNotifier.IsCompleted)
                return;

            var actions = new string[] { LocalizeString.Match_Filter_By_Bookmark, LocalizeString.Ongoing_matches, LocalizeString.Match_Sort_By_Time, LocalizeString.Match_Sort_By_League };

            int intResult = await MaterialDialog.Instance.SelectActionAsync(actions);
            if (intResult == -1)
                return;

            intResult.TryParseEnum<MatchFilterType>(out MatchFilterType filterType);

            switch (filterType)
            {
                case MatchFilterType.Bookmark:
                    break;

                case MatchFilterType.Ongoing:
                    break;

                case MatchFilterType.SortByTime:
                    break;

                case MatchFilterType.SortByLeague:
                    break;

                default:
                    break;
            }
        }

        public ICommand ExpandAllLeaguesCommand { get => new RelayCommand(ExpandAllLeagues); }

        private void ExpandAllLeagues()
        {
            foreach (var matchGroup in MatchGroups)
            {
                matchGroup.Expanded = true;
            }

            MatchGroups = new ObservableCollection<FootballMatchGroup>(MatchGroups);
        }

        public ICommand CollapseAllLeaguesCommand { get => new RelayCommand(CollapseAllLeagues); }

        private void CollapseAllLeagues()
        {
            foreach (var matchGroup in MatchGroups)
            {
                matchGroup.Expanded = false;
            }

            MatchGroups = new ObservableCollection<FootballMatchGroup>(MatchGroups);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchesViewModel(
            FootballMatchesPage page,
            IWebApiService webApiService,
            ISQLiteService sqliteService) : base(page)
        {
            _webApiService = webApiService;
            _sqliteService = sqliteService;

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        public FootballMatchesViewModel SetMatchDate(DateTime date)
        {
            _matchDate = date;
            return this;
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> GetMatchesAsync()
        {
            await Task.Delay(300);

            var result = await _webApiService.RequestAsyncWithToken<O_GET_FIXTURES_BY_DATE>(new WebRequestContext
            {
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

            // Utc to Local, Check Image
            foreach (var fixture in result.Fixtures)
            {
                fixture.MatchTime = fixture.MatchTime.ToLocalTime();

                // 기본 이미지 설정
                if (string.IsNullOrEmpty(fixture.Country.Logo))
                    fixture.Country.Logo = "img_world.png";
                if (string.IsNullOrEmpty(fixture.League.Logo))
                    fixture.League.Logo = fixture.Country.Logo;
                if (string.IsNullOrEmpty(fixture.HomeTeam.Logo))
                    fixture.HomeTeam.Logo = "img_football.png";
                if (string.IsNullOrEmpty(fixture.AwayTeam.Logo))
                    fixture.AwayTeam.Logo = "img_football.png";

                var matchInfo = ShinyHost.Resolve<FixtureDetailToMatchInfoConverter>().Convert(
                                    fixture,
                                    typeof(FootballMatchInfo),
                                    null,
                                    CultureInfo.CurrentCulture) as FootballMatchInfo;

                var bookmarkedMatch = await _sqliteService.SelectAsync<FootballMatchInfo>(matchInfo.PrimaryKey);

                matchInfo.IsBookmarked = bookmarkedMatch?.IsBookmarked ?? false;

                _matchList.Add(matchInfo);
            }

            _lastUpdateTime = DateTime.UtcNow;

            InitializeMatcheGroups(_matchList);

            return _matchList;
        }

        private void InitializeMatcheGroups(List<FootballMatchInfo> matchList)
        {
            ObservableCollection<FootballMatchGroup> matchGroupCollection;
            matchGroupCollection = new ObservableCollection<FootballMatchGroup>();

            var grouppingMatches = matchList.GroupBy(elem => $"{elem.CountryName} - {elem.LeagueName}");
            foreach (var grouppingMatch in grouppingMatches)
            {
                // 기존 데이터 있는지.. 있으면 Expanded값은 유지
                var foundExistData = MatchGroups?.Where(elem => elem.Title == grouppingMatch.Key).FirstOrDefault();
                matchGroupCollection.Add(new FootballMatchGroup(grouppingMatch.Key, grouppingMatch.First().CountryLogo, foundExistData?.Expanded ?? true)
                {
                    FootballMatchListViewModel = new FootballMatchListViewModel
                    {
                        Matches = new ObservableCollection<FootballMatchInfo>(grouppingMatch.ToArray())
                    }
                });
            }

            MatchGroups = matchGroupCollection;
        }

        #endregion Methods
    }
}