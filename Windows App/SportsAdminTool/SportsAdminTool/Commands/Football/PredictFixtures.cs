using LogicCore.Debug;
using LogicCore.Utility;
using Repository.Mysql.FootballDB.Procedures;
using Repository.Mysql.FootballDB.Tables;
using SportsAdminTool.Logic.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FootballDB = Repository.Mysql.FootballDB;

namespace SportsAdminTool.Commands.Football
{
    public static class PredictFixtures
    {
        public static Task Execute()
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, "Predict fixtrues");

                IEnumerable<FootballDB.Tables.Fixture> db_fixtures;
                using (var P_SELECT_PREDICTABLE_FIXTURES = new FootballDB.Procedures.P_SELECT_PREDICTABLE_FIXTURES())
                {
                    P_SELECT_PREDICTABLE_FIXTURES.SetInput(new FootballDB.Procedures.P_SELECT_PREDICTABLE_FIXTURES.Input
                    {
                        WHERE = $"f.is_predicted = 0 AND f.match_time BETWEEN \"{DateTime.UtcNow.ToString("yyyyMMdd")}\" AND \"{DateTime.UtcNow.AddDays(4).ToString("yyyyMMdd")}\" AND lc.{nameof(LeagueCoverage.predictions)} = 1",
                    });
                    db_fixtures = P_SELECT_PREDICTABLE_FIXTURES.OnQuery();

                    Dev.Assert(P_SELECT_PREDICTABLE_FIXTURES.EntityStatus == null);
                }

                int fixtureCnt = db_fixtures.Count();
                int loop = 0;
                foreach (var db_fixture in db_fixtures)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Predict fixtrues ({loop}/{fixtureCnt})");

                    var pred_data = Singleton.Get<FootballPredictorAPI>().PredictFixture(db_fixture.id);

                    // DB Save
                    //db_fixture.is_predicted = true;
                    //Logic.Database.FootballDBFacade.UpdateFixture(db_fixture);
                }
            });
        }
    }
}