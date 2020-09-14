using LogicCore.Converter;
using LogicCore.DataMapping;
using LogicCore.Utility;
using PosePacket.Service.Football.Models.Enums;
using SportsAdminTool.Logic.Football;
using SportsAdminTool.Logic.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictionBackTesting.Commands
{
    using db_table = Repository.Mysql.FootballDB.Tables;

    public static class PredictFixtureAndCheckHit
    {
        public static Dictionary<FootballPredictionType, List<db_table.PredictionBackTesting>> Execute(db_table.Fixture[] db_fixtures)
        {
            var dic_result = new Dictionary<FootballPredictionType, List<db_table.PredictionBackTesting>>()
            {
                { FootballPredictionType.Match_Winner, new List<db_table.PredictionBackTesting>() },
                { FootballPredictionType.Both_Teams_to_Score, new List<db_table.PredictionBackTesting>() },
                { FootballPredictionType.Under_Over, new List<db_table.PredictionBackTesting>() },
            };

            int fixtureCnt = db_fixtures.Count();
            int loop = 0;
            foreach (var db_fixture in db_fixtures)
            {
                loop++;
                if (loop % 10 == 0 || loop == fixtureCnt)
                    Console.WriteLine($"Progress {loop} / {fixtureCnt}");

                // predict
                var pred_data = Singleton.Get<FootballPredictorAPI>().PredictFixture(db_fixture.id);
                if (pred_data == null)
                    continue;

                // Predict Fixture
                List<db_table.Prediction> db_predictions = new List<db_table.Prediction>();
                db_predictions.AddRange(PredictionFacade.PredictFinalScore(db_fixture.id, pred_data));
                db_predictions.AddRange(PredictionFacade.PredictMatchWinner(db_fixture.id, pred_data));
                db_predictions.AddRange(PredictionFacade.PredictBothToScore(db_fixture.id, pred_data));
                db_predictions.AddRange(PredictionFacade.PredictUnderOver(db_fixture.id, pred_data));

                // Check hit
                LogicFacade.DiscernPrediction(db_predictions.ToArray(), db_fixture.home_score, db_fixture.away_score);

                // Map (Prediction => PredictionBackTesting)
                List<db_table.PredictionBackTesting> db_predictionBackTestings = new List<db_table.PredictionBackTesting>();
                foreach (var prediction in db_predictions)
                {
                    var db_predictionBackTesting = DataMapper.Map<db_table.Prediction, db_table.PredictionBackTesting>(prediction);

                    // Add recommended pick in Dictionary
                    if (db_predictionBackTesting.grade != 0)
                    {
                        ((int)db_predictionBackTesting.main_label).TryParseEnum(out FootballPredictionType predictionType);
                        dic_result[predictionType].Add(db_predictionBackTesting);
                    }

                    db_predictionBackTestings.Add(db_predictionBackTesting);
                }

                // DB Save
                SportsAdminTool.Logic.Database.FootballDBFacade.UpdatePredictionBackTesting(db_predictionBackTestings.ToArray());
            }

            return dic_result;
        }
    }
}