using LogicCore.Utility;
using SportsAdminTool.Logic.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiLogic = SportsAdminTool.Logic.WebAPI;

namespace SportsAdminTool.Commands.Football
{
    public static class UpdateScheduledFixtures
    {
        public static Task<bool> Execute()
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, "Update scheduled fixtures");

                // Call API (5일치 경기)
                var api_fixtures = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixturesByDate(
                    DateTime.UtcNow
                    , DateTime.UtcNow.AddDays(1)
                    , DateTime.UtcNow.AddDays(2)
                    , DateTime.UtcNow.AddDays(3)
                    , DateTime.UtcNow.AddDays(4));

                // 취소, 연기된 경기 필터링
                var api_filteredFixtures = api_fixtures.Where(elem =>
                     Singleton.Get<CheckValidation>().IsValidLeague((short)elem.LeagueId, elem.League.Name, elem.League.Country)
                     && Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status));

                // grouping by leagueID
                var api_groupingbyLeague = api_filteredFixtures.GroupBy(elem => elem.LeagueId);
                int loop = 0;
                foreach (var api_groupingFixtures in api_groupingbyLeague)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update scheduled leagues ({loop}/{api_groupingbyLeague.Count()})");

                    // Check Already Updated
                    if (LogicFacade.IsAlreadyUpdatedLeague((short)api_groupingFixtures.Key))
                        return true;

                    // Update League All Fixtures
                    LogicFacade.UpdateLeagueAllFixtures((short)api_groupingFixtures.Key);

                    // is_predict_coverage 참인 리그만 업데이트
                    var db_league = Logic.Database.FootballDBFacade.SelectLeagues(where: $"id = {api_groupingFixtures.Key}").FirstOrDefault();
                    if (db_league.is_predict_coverage)
                    {
                        // Update Fixtures, Odds, Statistics,
                        int innerloop = 0;
                        foreach (var fixture in api_groupingFixtures)
                        {
                            innerloop++;
                            mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update scheduled fixtures ({innerloop}/{api_groupingFixtures.Count()})");

                            LogicFacade.UpdateTeamLastFixtures((short)fixture.HomeTeam.TeamId, 20);
                            LogicFacade.UpdateTeamLastFixtures((short)fixture.AwayTeam.TeamId, 20);
                            LogicFacade.UpdateH2H((short)fixture.HomeTeam.TeamId, (short)fixture.AwayTeam.TeamId);
                            LogicFacade.UpdateOdds(fixture.FixtureId);
                        }
                    }

                    // Update League Standings
                    LogicFacade.UpdateStandings((short)api_groupingFixtures.Key);
                }

                return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);
            });
        }
    }
}