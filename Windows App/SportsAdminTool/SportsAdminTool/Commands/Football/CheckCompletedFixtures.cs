using LogicCore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiModel = RapidAPI.Models;
using ApiLogic = SportsAdminTool.Logic.WebAPI;
using SportsAdminTool.Logic.Football;

namespace SportsAdminTool.Commands.Football
{
    public static class CheckCompletedFixtures
    {
        public static Task Execute()
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                mainWindow.Set_Lable(mainWindow._lbl_check_completed_fixtures, "Check completed fixtures");

                var db_fixtures = Logic.Database.FootballDBFacade.SelectFixtures(
                    where: $"is_completed = 0 AND match_time < '{DateTime.UtcNow.AddHours(12).ToString("yyyyMMddTHHmmss")}'");

                int fixtureCnt = db_fixtures.Count();
                int loop = 0;
                foreach (var db_fixture in db_fixtures)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_check_completed_fixtures, $"Check completed fixtures ({loop}/{fixtureCnt})");

                    // Call API
                    var api_fixture = Singleton.Get<ApiLogic.FootballWebAPI>().GetFixturesByFixtureId(db_fixture.id);
                    if (api_fixture == null || !Singleton.Get<CheckValidation>().IsValidFixtureStatus(api_fixture.Status, api_fixture.MatchTime))
                    {
                        Logic.Database.FootballDBFacade.DeleteFixtures(where: $"id = {db_fixture.id}");
                        continue;
                    }

                    // 종료된 경기 처리
                    if (api_fixture.Status == ApiModel.Football.Enums.FixtureStatusType.FT // 종료
                        || api_fixture.Status == ApiModel.Football.Enums.FixtureStatusType.AET // 연장 후 종료
                        || api_fixture.Status == ApiModel.Football.Enums.FixtureStatusType.PEN)// 승부차기 후 종료
                    {
                        db_fixture.is_completed = true;

                        // Update statistics
                        LogicFacade.UpdateFixtureStatistics(db_fixture.id);
                    }

                    // DB Save
                    db_fixture.home_score = (short)api_fixture.GoalsHomeTeam;
                    db_fixture.away_score = (short)api_fixture.GoalsAwayTeam;
                    db_fixture.match_time = api_fixture.MatchTime;
                    db_fixture.status = api_fixture.Status.ToString();
                    Logic.Database.FootballDBFacade.UpdateFixture(db_fixture);
                }
            });
        }
    }
}