using LogicCore.Debug;
using LogicCore.Utility;
using Repository.Mysql.FootballDB.Tables;
using SportsAdminTool.Logic.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootballDB = Repository.Mysql.FootballDB;

namespace SportsAdminTool.Commands.Football
{
    public static class NotifyFootballPredictions
    {
        public static Task Execute()
        {
            return Task.Run(() =>
            {
                var mainWindow = Singleton.Get<MainWindow>();

                mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, "Football Predictions");

                var db_fixtures = Logic.Database.FootballDBFacade.SelectFixtures(
                    where: $"is_predicted = 1 " +
                    $"AND match_time BETWEEN '{DateTime.UtcNow.ToString("yyyyMMddTHHmmss")}' AND '{DateTime.UtcNow.AddDays(1).ToString("yyyyMMddTHHmmss")}' ",
                    orderBy: "match_time ASC ");

                int fixtureCnt = db_fixtures.Count();
                int loop = 0;

                Singleton.Get<LineNotifyAPI>().SendMessage(LineNotifyType.Football_Picks,
                    $"-----{DateTime.Now.ToString("yyyy.MM.dd HH:mm")}-----");

                foreach (var db_fixture in db_fixtures)
                {
                    loop++;
                    mainWindow.Set_Lable(mainWindow._lbl_collectDatasAndPredict, $"Predicted fixtrues ({loop}/{fixtureCnt})");

                    var db_predictions = Logic.Database.FootballDBFacade.SelectPredictions(
                        where: $"fixture_id = {db_fixture.id} AND is_recommended = 1");
                    foreach (var db_prediction in db_predictions)
                    {
                        string notiMSG = Logic.Football.PredictionFacade.MakePredictionNotificationMessage(db_fixture, db_prediction);

                        if (!string.IsNullOrEmpty(notiMSG))
                            Singleton.Get<LineNotifyAPI>().SendMessage(LineNotifyType.Football_Picks, notiMSG);
                    }
                }
            });
        }
    }
}