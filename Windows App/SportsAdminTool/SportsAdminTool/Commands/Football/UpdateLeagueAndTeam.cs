﻿using LogicCore.Utility;
using Repository.Mysql.FootballDB.Tables;
using SportsAdminTool.Logic.Football;
using SportsAdminTool.Model.Resource.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiLogic = SportsAdminTool.Logic.WebAPI;

namespace SportsAdminTool.Commands.Football
{
    // CoverageLeague.json table 없을때..
    // api_league.Coverage.Predictions = CoverageLeague.HasLeague(api_league.Country, api_league.Name, api_league.Type);
    //|| api_league.Coverage.FixtureCoverage.Statistics
    //|| (api_league.Coverage.Players && api_league.Coverage.FixtureCoverage.Lineups);

    public static class UpdateLeagueAndTeam
    {
        public static Task<bool> Execute(bool isUpdateAll)
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                // Update all countrys
                mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, "Update countries");
                var api_countries = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllCountries();
                Logic.Database.FootballDBFacade.UpdateCountry(api_countries.ToArray());

                // Update all leagues
                mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, "Update leagues");
                var api_leagues = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllAvailableLeauges();

                var leagueCnt = api_leagues.Count();
                int loop = 0;
                foreach (var api_league in api_leagues)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_initialize_footballdb, $"Update Leagues ({loop}/{leagueCnt})");

                    // Update League
                    Singleton.Get<CheckValidation>().IsValidLeague((short)api_league.LeagueId, api_league.Name, api_league.Country, out League league, out LeagueCoverage leagueCoverage);
                    if (league != null && leagueCoverage != null)
                        api_league.Coverage.Predictions = leagueCoverage.predictions || CoverageLeague.HasLeague(api_league.Country, api_league.Name, api_league.Type);
                    else
                        continue;

                    Logic.Database.FootballDBFacade.UpdateCoverage(api_league);
                    Logic.Database.FootballDBFacade.UpdateLeague(api_league);

                    // 전체 경기 다 가져오고 싶을땐 해당 코드 주석처리
                    if (!isUpdateAll && api_league.IsCurrent == 0)
                        continue;

                    // Update All Teams
                    var api_teams = Singleton.Get<ApiLogic.FootballWebAPI>().GetAllTeamsByLeagueId((short)api_league.LeagueId);
                    Logic.Database.FootballDBFacade.UpdateTeam((short)api_league.LeagueId, api_teams.ToArray());

                    // Update All Fixtures
                    LogicFacade.UpdateAllFixturesByLeague((short)api_league.LeagueId);
                    LogicFacade.UpdateStandings((short)api_league.LeagueId);
                }

                return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);
            });
        }
    }
}