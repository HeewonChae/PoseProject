using LogicCore.Utility;
using LogicCore.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ApiLogic = SportsAdminTool.Logic.WebAPI;
using FootballDB = Repository.Mysql.FootballDB;
using AppModel = SportsAdminTool.Model;
using ApiModel = RapidAPI.Models;
using System.Windows.Controls;
using SportsAdminTool.Model.Resource.Football;
using LogicCore.Debug;
using RapidAPI.Models.Football.Enums;

namespace SportsAdminTool.Logic.Football
{
    public static class LogicFacade
    {
        public static void UpdateAllFixturesByLeague(short leagueId)
        {
            // Call API
            var api_fixtures = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixturesByLeagueId(leagueId);
            var api_filteredFixtures = api_fixtures.Where(elem => Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status, elem.MatchTime));
            var api_deleteFixtures = api_fixtures.Except(api_filteredFixtures);
            var api_odds = Singleton.Get<ApiLogic.FootballWebAPI>().GetOddsByLeagueIdAndLabelId(leagueId, (int)OddsLabelType.Match_Winner);

            // DB Save
            Database.FootballDBFacade.DeleteFixtures(api_deleteFixtures.ToArray());
            Database.FootballDBFacade.UpdateFixture(api_filteredFixtures.ToArray());
            Database.FootballDBFacade.UpdateOdds(api_odds.ToArray());
        }

        public static void UpdateOdds(int fixtureId)
        {
            if (IsAlreadyUpdatedOdds(fixtureId))
                return;

            // Call API
            var api_odds = Singleton.Get<ApiLogic.FootballWebAPI>().GetOddsByFixtureId(fixtureId);
            if (api_odds == null)
                return;

            // DB Save
            Database.FootballDBFacade.UpdateOdds(fixtureId, api_odds.Bookmakers);
        }

        public static void UpdateFixtureStatistics(int fixtureId)
        {
            if (IsAlreadyUpdatedStatistics(fixtureId))
                return;

            // Call API
            var api_fixtureStatistic = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixtureStatisticByFixtureId(fixtureId);
            if (api_fixtureStatistic == null)
                return;

            var db_fixtures = Database.FootballDBFacade.SelectFixtures(where: $"id = {fixtureId}").FirstOrDefault();
            Dev.Assert(db_fixtures != null, $"UpdateFixtureStatistics db_fixtures is null FixtureId: {fixtureId}");

            // Convert api model to db model
            DataConverter.CovertApiModelToDbModel(fixtureId, db_fixtures.home_team_id, db_fixtures.away_team_id, api_fixtureStatistic,
                out FootballDB.Tables.FixtureStatistics[] coverted_fixtureStatistics);

            // DB Save
            Database.FootballDBFacade.UpdateFixtureStatistics(coverted_fixtureStatistics);
        }

        public static void UpdateH2H(short teamId1, short teamId2)
        {
            // Call API
            var api_h2h = Singleton.Get<ApiLogic.FootballWebAPI>().GetH2HFixtures(teamId1, teamId2);
            var api_filteredFixtures = api_h2h.Where(elem => Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status, elem.MatchTime));

            foreach (var api_fixture in api_filteredFixtures)
            {
                // DB Save
                Database.FootballDBFacade.UpdateFixture(api_fixture);
                UpdateFixtureStatistics(api_fixture.FixtureId);
            }
        }

        public static void UpdateTeamLastFixtures(short teamId, byte count)
        {
            // Call API
            var api_fixtures = Singleton.Get<ApiLogic.FootballWebAPI>().GetLastFixturesByTeamId(teamId, count);
            var api_filteredFixtures = api_fixtures.Where(elem => Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status, elem.MatchTime));

            foreach (var api_fixture in api_filteredFixtures)
            {
                // DB Save
                Database.FootballDBFacade.UpdateFixture(api_fixture);
                UpdateFixtureStatistics(api_fixture.FixtureId);
            }
        }

        public static Task SolveErrors(Label lbl)
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                var leagueErrors = Singleton.Get<CheckValidation>().GetErrorLeauges(InvalidType.NotExistInDB, false);
                int errorLeagueCnt = leagueErrors.Length;
                int loop = 0;
                foreach (var errorLeague in leagueErrors)
                {
                    loop++;
                    mainWindow.Set_Lable(lbl, $"Solve League Errors ({loop}/{errorLeagueCnt})");

                    // Call API
                    var api_league = Singleton.Get<ApiLogic.FootballWebAPI>().GetLeagueByLeagueId(errorLeague.LeagueId);
                    //Dev.Assert(api_league != null, $"api_league is null leagueId: {errorLeague.LeagueId}");

                    if (api_league != null)
                    {
                        // DB Save
                        Database.FootballDBFacade.UpdateCoverage(api_league);
                        Database.FootballDBFacade.UpdateLeague(api_league);

                        Singleton.Get<CheckValidation>().DeleteErrorLeague(errorLeague);
                    }
                }

                var teamErrors = Singleton.Get<CheckValidation>().GetErrorTeams(InvalidType.NotExistInDB, false);
                int errorTeamCnt = teamErrors.Length;
                loop = 0;
                foreach (var errorTeam in teamErrors)
                {
                    loop++;
                    mainWindow.Set_Lable(lbl, $"Solve Team Errors ({loop}/{errorTeamCnt})");

                    // Call API
                    var api_team = Singleton.Get<ApiLogic.FootballWebAPI>().GetTeamByTeamId(errorTeam.TeamId);
                    //Dev.Assert(api_team != null, $"api_team is null teamId: {errorTeam.TeamsId}");

                    if (api_team != null)
                    {
                        // DB Save
                        Database.FootballDBFacade.UpdateTeam(errorTeam.LeagueId, api_team);

                        Singleton.Get<CheckValidation>().DeleteErrorTeam(errorTeam);
                    }
                }
            });
        }

        public static bool UpdateStandings(short leagueId)
        {
            var db_league = Database.FootballDBFacade.SelectLeagues(where: $"id = {leagueId}").FirstOrDefault();

            // Call API
            var api_standings = Singleton.Get<ApiLogic.FootballWebAPI>().GetStandingsByLeagueId(leagueId);
            if (api_standings.Count == 0)
                return true;

            // Call DB
            return Database.FootballDBFacade.UpdateStandings(leagueId, db_league.country_name, api_standings.ToArray());
        }

        public static bool IsAlreadyUpdatedOdds(int fixtureId)
        {
            return Database.FootballDBFacade.SelectOdds(where: $"fixture_id = {fixtureId}").FirstOrDefault() != null;
        }

        public static bool IsAlreadyUpdatedStatistics(int fixtureId)
        {
            return Database.FootballDBFacade.SelectStatistics(where: $"fixture_id = {fixtureId}").FirstOrDefault() != null;
        }

        public static bool IsExistFixture(int fixtureId)
        {
            return Database.FootballDBFacade.SelectFixtures(where: $"id = {fixtureId}").FirstOrDefault() != null;
        }
    }
}