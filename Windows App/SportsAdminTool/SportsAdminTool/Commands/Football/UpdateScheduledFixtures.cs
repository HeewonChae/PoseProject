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

                // 5일치 경기
                var db_fixtures = Logic.Database.FootballDBFacade.SelectFixtures(
                    where: $"is_completed = 0 " +
                    $"AND match_time BETWEEN '{DateTime.UtcNow.ToString("yyyyMMddTHHmmss")}' AND '{DateTime.UtcNow.AddDays(6).ToString("yyyyMMddTHHmmss")}'");

                // grouping by leagueID
                var api_groupingbyLeague = db_fixtures.GroupBy(elem => elem.league_id);
                int loop = 0;
                foreach (var api_groupingFixtures in api_groupingbyLeague)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update scheduled leagues ({loop}/{api_groupingbyLeague.Count()})");

                    if (!Singleton.Get<CheckValidation>().IsValidLeague((short)api_groupingFixtures.Key, null, null, out League db_league, out LeagueCoverage db_leagueCoverage))
                        continue;

                    // Update Fixtures, Odds, Statistics,
                    int innerloop = 0;
                    foreach (var fixture in api_groupingFixtures)
                    {
                        innerloop++;
                        mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Update scheduled fixtures ({innerloop}/{api_groupingFixtures.Count()})");

                        // 취소, 연기된 경기 삭제
                        var api_fixture = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixturesByFixtureId(fixture.id);
                        if (api_fixture == null || !Singleton.Get<CheckValidation>().IsValidFixtureStatus(api_fixture.Status, api_fixture.MatchTime))
                        {
                            Logic.Database.FootballDBFacade.DeleteFixtures(where: $"id = {fixture.id}");
                            Logic.Database.FootballDBFacade.DeletePrediction(where: $"fixture_id = {fixture.id}");
                            continue;
                        }

                        if (db_leagueCoverage.predictions)
                        {
                            //LogicFacade.UpdateTeamLastFixtures(fixture.home_team_id, 10);
                            //LogicFacade.UpdateTeamLastFixtures(fixture.away_team_id, 10);
                            //LogicFacade.UpdateH2H(fixture.home_team_id, fixture.away_team_id);
                            LogicFacade.UpdateOdds(fixture.id);
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