using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Football.Enums;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.Cache.Loader;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Team.GoalStatistics;
using PoseSportsPredict.Views.Football.Match.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballMatchDetailH2HViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            H2HTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (H2HTaskLoaderNotifier.IsNotStarted)
                H2HTaskLoaderNotifier.Load(InitH2HData);
        }

        #endregion BaseViewModel

        #region Services

        private ICacheService _cacheService;

        #endregion Services

        #region Fields

        private FootballMatchInfo _matchInfo;

        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _H2HTaskLoaderNotifier;
        private FootballTeamGoalStatisticsViewModel _teamGoalStatisticsViewModel;
        private FootballTeamStatistics.RecordInfo _h2hRecordInfo;
        private double _recordGaugeHeight;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> H2HTaskLoaderNotifier { get => _H2HTaskLoaderNotifier; set => SetValue(ref _H2HTaskLoaderNotifier, value); }
        public FootballTeamGoalStatisticsViewModel GoalStatisticsViewModel { get => _teamGoalStatisticsViewModel; set => SetValue(ref _teamGoalStatisticsViewModel, value); }
        public FootballTeamStatistics.RecordInfo H2HRecordInfo { get => _h2hRecordInfo; set => SetValue(ref _h2hRecordInfo, value); }
        public double RecordGaugeHeight { get => _recordGaugeHeight; set => SetValue(ref _recordGaugeHeight, value); }

        #endregion Properties

        #region Constructors

        public FootballMatchDetailH2HViewModel(
            FootballMatchDetailH2HView view,
            ICacheService cacheService) : base(view)
        {
            _cacheService = cacheService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballMatchDetailH2HViewModel SetMatchInfo(FootballMatchInfo matchInfo)
        {
            _matchInfo = matchInfo;
            return this;
        }

        private async Task<IReadOnlyCollection<FootballMatchInfo>> InitH2HData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            var server_result = await _cacheService.GetAsync<O_GET_MATCH_H2H>(
               loader: () =>
               {
                   return FootballDataLoader.MatchH2H(
                       _matchInfo.Id,
                       _matchInfo.HomeTeamId,
                       _matchInfo.AwayTeamId);
               },
               key: $"P_GET_MATCH_H2H:{_matchInfo.PrimaryKey}",
               expireTime: DateTime.Now.Date.AddDays(1) - DateTime.Now,
               serializeType: SerializeType.MessagePack);

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            if (server_result.H2HFixtures.Length == 0)
            {
                SetIsBusy(false);
                return null;
            }

            var matches = new List<FootballMatchInfo>();
            foreach (var fixture in server_result.H2HFixtures)
            {
                matches.Add(ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(fixture));
            }

            var screenHelper = DependencyService.Resolve<IScreenHelper>();
            RecordGaugeHeight = screenHelper.ScreenSize.Width / 9.0 * 2.7;

            // Total Record
            var homeTeamStatistics = ShinyHost.Resolve<MatchInfoToTeamStatistics>().Convert(matches, _matchInfo.HomeTeamId, matches.Count, 0);
            H2HRecordInfo = homeTeamStatistics.RecentRecord;

            var matchesByGroups = matches.GroupBy(elem => $"{elem.League_CountryName}:{elem.LeagueName}:{elem.LeagueType}");
            var matchesByLeague = new Dictionary<FootballLeagueInfo, List<FootballMatchInfo>>();
            foreach (var matchesByGroup in matchesByGroups)
            {
                var league = ShinyHost.Resolve<MatchInfoToLeagueInfo>().Convert(matchesByGroup.First());
                matchesByLeague.Add(league, matchesByGroup.OrderByDescending(elem => elem.MatchTime).ToList());
            }

            matchesByLeague = matchesByLeague.OrderByDescending(elem => $"{elem.Key.LeagueType}:{elem.Key.CountryName}").ToDictionary(elem => elem.Key, elem => elem.Value);

            // 팀 득점 통계
            GoalStatisticsViewModel = ShinyHost.Resolve<FootballTeamGoalStatisticsViewModel>();
            GoalStatisticsViewModel.SetMember(matchesByLeague, matchInfo: _matchInfo, goalStatisticsType: FootballGoalStatisticsType.H2H);

            SetIsBusy(false);

            return matches;
        }

        #endregion Methods
    }
}