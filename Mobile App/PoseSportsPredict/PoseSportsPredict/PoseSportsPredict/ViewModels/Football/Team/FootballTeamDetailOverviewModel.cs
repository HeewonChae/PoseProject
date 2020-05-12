using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Football.Enums;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.ViewModels.Football.Team.GoalStatistics;
using PoseSportsPredict.Views.Football.Team;
using Sharpnado.Presentation.Forms;
using Shiny;
using Syncfusion.XForms.ComboBox;
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
    public class FootballTeamDetailOverviewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            OverviewTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>>();
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (!OverviewTaskLoaderNotifier.IsNotStarted)
                return;

            OverviewTaskLoaderNotifier.Load(InitOverviewData);
        }

        #endregion BaseViewModel

        #region Services

        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private FootballTeamInfo _teamInfo;
        private List<FootballMatchInfo> _matches;

        private TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> _overviewTaskLoaderNotifier;
        private ObservableList<ParticipatingLeagueInfo> _participatingLeagues;
        private FootballTeamGoalStatisticsViewModel _teamGoalStatisticsViewModel;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballLeagueInfo>> OverviewTaskLoaderNotifier { get => _overviewTaskLoaderNotifier; set => SetValue(ref _overviewTaskLoaderNotifier, value); }
        public ObservableList<ParticipatingLeagueInfo> ParticipatingLeagues { get => _participatingLeagues; set => SetValue(ref _participatingLeagues, value); }
        public FootballTeamGoalStatisticsViewModel GoalStatisticsViewModel { get => _teamGoalStatisticsViewModel; set => SetValue(ref _teamGoalStatisticsViewModel, value); }

        #endregion Properties

        #region Commands

        public ICommand LeagueNameClickCommand { get => new RelayCommand<FootballLeagueInfo>((e) => LeagueNameClick(e)); }

        private async void LeagueNameClick(FootballLeagueInfo leagueInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);

            SetIsBusy(false);
        }

        public ICommand FormClickCommand { get => new RelayCommand<FootballFormInfo>((e) => FormClick(e)); }

        private async void FormClick(FootballFormInfo formInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var foundMatch = _matches.FirstOrDefault(elem => elem.Id == formInfo.FixtureId);

            if (foundMatch != null)
                await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), foundMatch);

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballTeamDetailOverviewModel(
            FootballTeamDetailOverview view,
            IWebApiService webApiService) : base(view)
        {
            _webApiService = webApiService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballTeamDetailOverviewModel SetTeamInfo(FootballTeamInfo teamInfo)
        {
            _teamInfo = teamInfo;
            return this;
        }

        private async Task<IReadOnlyCollection<FootballLeagueInfo>> InitOverviewData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            var server_result = await _webApiService.RequestAsyncWithToken<O_GET_TEAM_OVERVIEW>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_TEAM_OVERVIEW,
                PostData = new I_GET_TEAM_OVERVIEW
                {
                    TeamId = _teamInfo.TeamId,
                }
            });

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            if (server_result.FixtureDetailsByLeague.Count == 0)
            {
                SetIsBusy(false);
                return null;
            }

            _matches = new List<FootballMatchInfo>();
            var matchesByLeague = new Dictionary<FootballLeagueInfo, List<FootballMatchInfo>>();
            foreach (var keyValue in server_result.FixtureDetailsByLeague)
            {
                var matches = new List<FootballMatchInfo>();
                foreach (var fixture in keyValue.Value)
                {
                    matches.Add(ShinyHost.Resolve<FixtureDetailToMatchInfo>().Convert(fixture));
                }

                var league = ShinyHost.Resolve<LeagueDetailToLeagueInfo>().Convert(keyValue.Key);
                matchesByLeague.Add(league, matches.OrderByDescending(elem => elem.MatchTime).ToList());

                _matches.AddRange(matches);
            }

            matchesByLeague = matchesByLeague.OrderByDescending(elem => $"{elem.Key.LeagueType}:{elem.Key.CountryName}").ToDictionary(elem => elem.Key, elem => elem.Value);

            // 참가중인 리그
            ParticipatingLeagues = new ObservableList<ParticipatingLeagueInfo>();
            foreach (var keyValue in matchesByLeague)
            {
                var participatingLeague = new ParticipatingLeagueInfo
                {
                    LeagueInfo = keyValue.Key,
                    RecentForm = new List<FootballFormInfo>(),
                };

                var recentMatches = keyValue.Value.Take(6).Reverse();
                foreach (var match in recentMatches)
                {
                    participatingLeague.RecentForm.Add(ShinyHost.Resolve<MatchInfoToFormInfo>().Convert(match, _teamInfo.TeamId));
                }

                var lastForm = participatingLeague.RecentForm.LastOrDefault();
                if (lastForm != null)
                    lastForm.IsLastMatch = true;

                ParticipatingLeagues.Add(participatingLeague);
            }

            // 팀 득점 통계
            GoalStatisticsViewModel = ShinyHost.Resolve<FootballTeamGoalStatisticsViewModel>();
            GoalStatisticsViewModel.SetMember(matchesByLeague, teamInfo: _teamInfo, goalStatisticsType: FootballGoalStatisticsType.Team);

            SetIsBusy(false);

            return matchesByLeague.Keys;
        }

        #endregion Methods
    }
}