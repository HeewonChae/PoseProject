using GalaSoft.MvvmLight.Command;
using PosePacket.Proxy;
using PosePacket.Service.Football;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services.Cache.Loader;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Standings;
using PoseSportsPredict.ViewModels.Football.Team;
using PoseSportsPredict.Views.Football.League.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using Syncfusion.XForms.ComboBox;
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
            if (OverviewTaskLoaderNotifier.IsNotStarted)
                OverviewTaskLoaderNotifier.Load(InitOverviewData);
        }

        #endregion BaseViewModel

        #region Services

        private ICacheService _cacheService;

        #endregion Services

        #region Fields

        private FootballLeagueInfo _leagueInfo;
        private TaskLoaderNotifier _overviewTaskLoaderNotifier;
        private ObservableList<FootballTeamInfo> _participatingTeams_Left;
        private ObservableList<FootballTeamInfo> _participatingTeams_Right;
        private FootballStandingsViewModel _standingsViewModel;

        #endregion Fields

        #region Properties

        public FootballLeagueInfo LeagueInfo { get => _leagueInfo; set => SetValue(ref _leagueInfo, value); }
        public TaskLoaderNotifier OverviewTaskLoaderNotifier { get => _overviewTaskLoaderNotifier; set => SetValue(ref _overviewTaskLoaderNotifier, value); }
        public ObservableList<FootballTeamInfo> ParticipatingTeams_Left { get => _participatingTeams_Left; set => SetValue(ref _participatingTeams_Left, value); }
        public ObservableList<FootballTeamInfo> ParticipatingTeams_Right { get => _participatingTeams_Right; set => SetValue(ref _participatingTeams_Right, value); }
        public FootballStandingsViewModel StandingsViewModel { get => _standingsViewModel; set => SetValue(ref _standingsViewModel, value); }

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
            ICacheService cacheService) : base(view)
        {
            _cacheService = cacheService;

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
            var server_result = await _cacheService.GetAsync<O_GET_LEAGUE_OVERVIEW>(
                loader: () =>
                {
                    return FootballDataLoader.LeagueOverview(
                        _leagueInfo.CountryName,
                        _leagueInfo.LeagueName);
                },
                key: $"P_GET_LEAGUE_OVERVIEW:{_leagueInfo.PrimaryKey}",
                expireTime: TimeSpan.Zero,
                serializeType: SerializeType.MessagePack);

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            // LeagueInfo
            LeagueInfo = ShinyHost.Resolve<LeagueDetailToLeagueInfo>().Convert(server_result.LeagueDetail);

            // 참가중인 팀
            //var teamInfos = new List<FootballTeamInfo>();
            //if (server_result.ParticipatingTeams.Length > 0)
            //{
            //    foreach (var teamDetail in server_result.ParticipatingTeams)
            //    {
            //        teamInfos.Add(ShinyHost.Resolve<TeamDetailToTeamInfo>().Convert(teamDetail));
            //    }

            //    ParticipatingTeams_Left = new ObservableList<FootballTeamInfo>();
            //    ParticipatingTeams_Right = new ObservableList<FootballTeamInfo>();

            //    teamInfos = teamInfos.OrderBy(elem => elem.TeamName).ToList();
            //    var loopCnt = 0;
            //    foreach (var teamInfo in teamInfos)
            //    {
            //        loopCnt++;

            //        if (loopCnt % 2 == 1)
            //            ParticipatingTeams_Left.Add(teamInfo);
            //        else
            //            ParticipatingTeams_Right.Add(teamInfo);
            //    }
            //}

            // 순위 테이블
            var standingsInfos = new List<FootballStandingsInfo>();
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