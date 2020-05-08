using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Base;
using Sharpnado.Presentation.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football.Team.GoalStatistics
{
    public class FootballTeamGoalStatisticsViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            TaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>>();
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
        }

        #endregion BaseViewModel

        #region Fields

        private FootballTeamInfo _teamInfo;
        private List<FootballMatchInfo> _matches;

        private FootballLeagueInfo _leagueInfo;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> _taskLoaderNotifier;

        #endregion Fields

        #region Properties

        public TaskLoaderNotifier<IReadOnlyCollection<FootballMatchInfo>> TaskLoaderNotifier { get => _taskLoaderNotifier; set => SetValue(ref _taskLoaderNotifier, value); }
        public FootballLeagueInfo LeagueInfo { get => _leagueInfo; set => SetValue(ref _leagueInfo, value); }

        #endregion Properties

        #region Methods

        public void SetMember(List<FootballMatchInfo> matches, FootballTeamInfo teamInfo, FootballLeagueInfo leagueInfo)
        {
            _matches = matches;
            _teamInfo = teamInfo;

            LeagueInfo = leagueInfo;
        }

        #endregion Methods
    }
}