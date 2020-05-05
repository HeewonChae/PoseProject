using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Standings;
using PoseSportsPredict.ViewModels.Football.Team;
using PoseSportsPredict.Views.Football.League.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.ViewModels.Football.League.Detail
{
    public class FootballLeagueDetailOverviewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            OverviewTaskLoaderNotifier = new TaskLoaderNotifier();

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

        private FootballLeagueInfo _leagueInfo;
        private TaskLoaderNotifier _overviewTaskLoaderNotifier;
        private ObservableList<FootballTeamInfo> _participatingTeams_Left;
        private ObservableList<FootballTeamInfo> _participatingTeams_Right;
        private ObservableList<FootballStandingsViewModel> _standingsViewModels;

        #endregion Fields

        #region Properties

        public FootballLeagueInfo LeagueInfo { get => _leagueInfo; set => SetValue(ref _leagueInfo, value); }
        public TaskLoaderNotifier OverviewTaskLoaderNotifier { get => _overviewTaskLoaderNotifier; set => SetValue(ref _overviewTaskLoaderNotifier, value); }
        public ObservableList<FootballTeamInfo> ParticipatingTeams_Left { get => _participatingTeams_Left; set => SetValue(ref _participatingTeams_Left, value); }
        public ObservableList<FootballTeamInfo> ParticipatingTeams_Right { get => _participatingTeams_Right; set => SetValue(ref _participatingTeams_Right, value); }
        public ObservableList<FootballStandingsViewModel> StandingsViewModels { get => _standingsViewModels; set => SetValue(ref _standingsViewModels, value); }

        #endregion Properties

        #region Commands

        public ICommand ParticipatingTeamClickCommand { get => new RelayCommand<FootballTeamInfo>(e => ParticipatingTeamClick(e)); }

        private async void ParticipatingTeamClick(FootballTeamInfo teamInfo)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>(), teamInfo);

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballLeagueDetailOverviewModel(
            FootballLeagueDetailOverview view,
            IWebApiService webApiService) : base(view)
        {
            _webApiService = webApiService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public FootballLeagueDetailOverviewModel SetLeagueInfo(FootballLeagueInfo leagueInfo)
        {
            LeagueInfo = leagueInfo;
            return this;
        }

        private async Task InitOverviewData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            // call server
            var server_result = await _webApiService.RequestAsyncWithToken<O_GET_LEAGUE_OVERVIEW>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = FootballProxy.ServiceUrl,
                SegmentGroup = FootballProxy.P_GET_LEAGUE_OVERVIEW,
                PostData = new I_GET_LEAGUE_OVERVIEW
                {
                    CountryName = LeagueInfo.CountryName,
                    LeagueName = LeagueInfo.LeagueName
                }
            });

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            // LeagueInfo
            LeagueInfo = ShinyHost.Resolve<LeagueDetailToLeagueInfo>().Convert(server_result.LeagueDetail, null, null, null) as FootballLeagueInfo;

            // 참가중인 팀
            var teamInfos = new List<FootballTeamInfo>();
            if (server_result.ParticipatingTeams.Count > 0)
            {
                foreach (var teamDetail in server_result.ParticipatingTeams)
                {
                    teamInfos.Add(
                        ShinyHost.Resolve<TeamDetailToTeamInfo>().Convert(teamDetail, null, null, null) as FootballTeamInfo);
                }

                ParticipatingTeams_Left = new ObservableList<FootballTeamInfo>();
                ParticipatingTeams_Right = new ObservableList<FootballTeamInfo>();

                teamInfos = teamInfos.OrderBy(elem => elem.TeamName).ToList();
                var loopCnt = 0;
                foreach (var teamInfo in teamInfos)
                {
                    loopCnt++;

                    if (loopCnt % 2 == 1)
                        ParticipatingTeams_Left.Add(teamInfo);
                    else
                        ParticipatingTeams_Right.Add(teamInfo);
                }
            }

            // 순위 테이블
            var standingsInfos = new List<FootballStandingsInfo>();
            foreach (var standingsDetail in server_result.StandingsDetails)
            {
                standingsInfos.Add(
                    ShinyHost.Resolve<StandingsDetailToStandingsInfo>().Convert(standingsDetail, null, null, null) as FootballStandingsInfo);
            }

            //// Set RankColor
            var descGroups = standingsInfos.OrderBy(elem => elem.Rank).GroupBy(elem => elem.Description);
            int positiveDescCnt = 0;
            int negativeDescCnt = 0;
            int neutralDescCnt = 0;
            foreach (var descGroup in descGroups)
            {
                if (string.IsNullOrEmpty(descGroup.Key))
                    continue;

                var descCategoryType = StandginsDescription.GetDescCategory(descGroup.Key);
                Color descColor;
                if (descCategoryType == StandingsDescCategoryType.Positive)
                {
                    descColor = StandingsRankColor.GetRankColor(descCategoryType, positiveDescCnt);
                    positiveDescCnt++;
                }
                else if (descCategoryType == StandingsDescCategoryType.Negative)
                {
                    descColor = StandingsRankColor.GetRankColor(descCategoryType, negativeDescCnt);
                    negativeDescCnt++;
                }
                else if (descCategoryType == StandingsDescCategoryType.Neutral)
                {
                    descColor = StandingsRankColor.GetRankColor(descCategoryType, neutralDescCnt);
                    neutralDescCnt++;
                }
                else
                {
                    descColor = StandingsRankColor.GetRankColor(StandingsDescCategoryType.None, 0);
                }

                foreach (var standings in descGroup)
                {
                    standings.RankColor = descColor;
                }
            }

            // Groupping Standings by group data
            var standingsGroups = standingsInfos.OrderBy(elem => elem.Rank).GroupBy(elem => elem.Group);
            var standingsViewModels = new List<FootballStandingsViewModel>();
            foreach (var standingsGroup in standingsGroups)
            {
                var standingsViewModel = ShinyHost.Resolve<FootballStandingsViewModel>();
                standingsViewModel.OnInitializeView(standingsGroup.ToArray());

                standingsViewModels.Add(standingsViewModel);
            }

            StandingsViewModels = new ObservableList<FootballStandingsViewModel>(
                standingsViewModels.OrderBy(elem => elem.LeagueTitle).ToArray());

            SetIsBusy(false);
        }

        #endregion Methods
    }
}