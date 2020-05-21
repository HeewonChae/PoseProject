using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Match.RecentForm;
using PoseSportsPredict.ViewModels.Football.League.Standings;
using PoseSportsPredict.Views.Football.Match.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.Services.Cache.Loader;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballMatchDetailOverviewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            OverviewTaskLoaderNotifier = new TaskLoaderNotifier();

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (OverviewTaskLoaderNotifier.IsNotStarted)
                OverviewTaskLoaderNotifier.Load(InitOverviewData);
        }

        #endregion BaseViewModel

        #region Services

        private ICacheService _cacheService;

        #endregion Services

        #region Fields

        private FootballMatchInfo _matchInfo;
        private TaskLoaderNotifier _overviewTaskLoaderNotifier;
        private Models.Football.FootballMatchStatistics _matchStatistics;
        private FootballRecentFormViewModel _recentFormViewModel;
        private FootballStandingsViewModel _standingsViewModel;

        #endregion Fields

        #region Properties

        public FootballMatchInfo MatchInfo { get => _matchInfo; set => SetValue(ref _matchInfo, value); }
        public TaskLoaderNotifier OverviewTaskLoaderNotifier { get => _overviewTaskLoaderNotifier; set => SetValue(ref _overviewTaskLoaderNotifier, value); }
        public Models.Football.FootballMatchStatistics MatchStatistics { get => _matchStatistics; set => SetValue(ref _matchStatistics, value); }
        public FootballRecentFormViewModel RecentFormViewModel { get => _recentFormViewModel; set => SetValue(ref _recentFormViewModel, value); }
        public FootballStandingsViewModel StandingsViewModel { get => _standingsViewModel; set => SetValue(ref _standingsViewModel, value); }

        #endregion Properties

        #region Commands

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

        #endregion Commands

        #region Constructors

        public FootballMatchDetailOverviewModel(
            FootballMatchDetailOverview view,
            ICacheService cacheService) : base(view)
        {
            _cacheService = cacheService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballMatchDetailOverviewModel SetMatchInfo(FootballMatchInfo matchInfo)
        {
            MatchInfo = matchInfo;
            return this;
        }

        private async Task InitOverviewData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            TimeSpan expireTime = _matchInfo.MatchTime.Date > DateTime.Now.Date ? TimeSpan.FromMinutes(15) : DateTime.Now.Date.AddDays(1) - DateTime.Now;
            var server_result = await _cacheService.GetAsync<O_GET_MATCH_OVERVIEW>(
              loader: () =>
              {
                  return FootballDataLoader.MatchOverview(_matchInfo.Id);
              },
              key: $"P_GET_MATCH_OVERVIEW:{_matchInfo.PrimaryKey}",
              expireTime: expireTime,
              serializeType: SerializeType.MessagePack);

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            // All Form
            var allRecentMatches = new List<FootballMatchInfo>();
            foreach (var fixture in server_result.RecentFixtures)
            {
                allRecentMatches.Add(ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(fixture));
            }

            var homeRecentMatches = allRecentMatches.Where(elem =>
            elem.HomeTeamId == _matchInfo.HomeTeamId || elem.AwayTeamId == _matchInfo.HomeTeamId);

            var awayRecentMatches = allRecentMatches.Where(elem =>
            elem.HomeTeamId == _matchInfo.AwayTeamId || elem.AwayTeamId == _matchInfo.AwayTeamId);

            // 기본정보 (평균 득실점, 회복기간)
            MatchStatistics = new FootballMatchStatistics
            {
                HomeTeamStatistics = ShinyHost.Resolve<FixtureDetailToTeamStatistics>()
                .Convert(server_result.League_RecentFixtures, MatchInfo.HomeTeamId, 6, 3),

                AwayTeamStatistics = ShinyHost.Resolve<FixtureDetailToTeamStatistics>()
                .Convert(server_result.League_RecentFixtures, MatchInfo.AwayTeamId, 6, 3),

                HomeRestPeriod = (MatchInfo.MatchTime.Date - (homeRecentMatches.FirstOrDefault()?.MatchTime.Date ?? MatchInfo.MatchTime.Date)).Days,
                AwayRestPeriod = (MatchInfo.MatchTime.Date - (awayRecentMatches.FirstOrDefault()?.MatchTime.Date ?? MatchInfo.MatchTime.Date)).Days,
            };

            // RecentForm
            RecentFormViewModel = ShinyHost.Resolve<FootballRecentFormViewModel>();
            RecentFormViewModel.SetMembers(homeRecentMatches.ToList(), _matchInfo.HomeTeamId, awayRecentMatches.ToList(), _matchInfo.AwayTeamId);

            // League Standings Table
            List<FootballStandingsInfo> standingsInfos = new List<FootballStandingsInfo>();
            foreach (var standingsDetail in server_result.StandingsDetails)
            {
                standingsInfos.Add(ShinyHost.Resolve<StandingsDetailToStandingsInfo>().Convert(standingsDetail));
            }

            if (standingsInfos.Count > 0)
            {
                var standingsByGroup = standingsInfos
                .GroupBy(elem => elem.Group)
                .ToDictionary(elem => elem.Key, elem => elem.ToList());
                StandingsViewModel = ShinyHost.Resolve<FootballStandingsViewModel>();
                StandingsViewModel.SetMember(standingsByGroup);
            }

            SetIsBusy(false);
        }

        #endregion Methods
    }
}