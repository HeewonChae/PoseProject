using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football.Team
{
    public class FootballTeamDetailOverviewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
        }

        #endregion BaseViewModel

        #region Services

        private IWebApiService _webApiService;

        #endregion Services

        #region Fields

        private FootballTeamInfo _teamInfo;

        #endregion Fields

        #region Properties

        public FootballTeamInfo TeamInfo { get => _teamInfo; set => SetValue(ref _teamInfo, value); }

        #endregion Properties

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
            TeamInfo = teamInfo;
            return this;
        }

        #endregion Methods
    }
}