using LogicCore.Debug;
using LogicCore.Utility;
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
                var api_filteredFixtures = api_fixtures.Where(elem => Singleton.Get<CheckValidation>().IsValidFixtureStatus(elem.Status, elem.MatchTime));

                // grouping by leagueID
                var api_groupingbyLeague = api_filteredFixtures.GroupBy(elem => elem.LeagueId);
                int loop = 0;
                foreach (var api_groupingFixtures in api_groupingbyLeague)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update scheduled leagues ({loop}/{api_groupingbyLeague.Count()})");

                    if (!Singleton.Get<CheckValidation>().IsValidLeague((short)api_groupingFixtures.Key, null, null, out League db_league, out LeagueCoverage db_leagueCoverage))
                        continue;

                    // Check Already Updated
                    if (db_league.upt_time.Date == DateTime.UtcNow.Date)
                        continue;

                    // Update League All Fixtures
                    LogicFacade.UpdateAllFixturesByLeague((short)api_groupingFixtures.Key);

                    // is_predict_coverage 참인 리그만 업데이트
                    if (db_leagueCoverage.predictions)
                    {
                        // Update Fixtures, Odds, Statistics,
                        int innerloop = 0;
                        foreach (var fixture in api_groupingFixtures)
                        {
                            innerloop++;
                            mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update scheduled fixtures ({innerloop}/{api_groupingFixtures.Count()})");

                            LogicFacade.UpdateTeamLastFixtures((short)fixture.HomeTeam.TeamId, 30);
                            LogicFacade.UpdateTeamLastFixtures((short)fixture.AwayTeam.TeamId, 30);
                            LogicFacade.UpdateH2H((short)fixture.HomeTeam.TeamId, (short)fixture.AwayTeam.TeamId);
                            LogicFacade.UpdateOdds(fixture.FixtureId);
                        }
                    }

                    // Update League Standings
                    if (!LogicFacade.UpdateStandings((short)api_groupingFixtures.Key))
                        continue;

                    db_league.upt_time = DateTime.UtcNow;
                    Logic.Database.FootballDBFacade.UpdateLeague(db_league);
                }

                return !Singleton.Get<CheckValidation>().IsExistError(InvalidType.NotExistInDB);
            });
        }
    }
}