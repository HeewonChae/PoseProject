using LogicCore.DataMapping;
using LogicCore.Utility;
using PredictionBackTesting.Commands;
using SportsAdminTool.Logic.Database;
using SportsAdminTool.Logic.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictionBackTesting
{
    using db_table = Repository.Mysql.FootballDB.Tables;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Init LogicStatic
            Repository.RepositoryStatic.Init_Mysql();
            Singleton.Register(new FootballPredictorAPI());

            // register Mapper
            DataMapper.Resolve<db_table.Prediction, db_table.PredictionBackTesting>();

            // select predicted fixtures
            var predictedFixtures = SelecltPredictedFixtures.Execute(startDate: new DateTime(2020, 8, 15), endData: new DateTime(2020, 8, 31));

            // Predict fixture and check hit
            var dic_predictions = PredictFixtureAndCheckHit.Execute(predictedFixtures);

            // Save prediction percentage
            SaveJsonPredictionPercentage.Execute(dic_predictions);
        }
    }
}