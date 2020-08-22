using LogicCore.Utility;
using PosePacket.Service.Football;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Logics.Converters;
using SportsWebService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Windows.Input;

using FootballDB = Repository.Mysql.FootballDB;
using PacketModels = PosePacket.Service.Football.Models;

namespace SportsWebService.Commands.Football
{
    [WebModelType(InputType = typeof(I_GET_MATCH_PREDICTIONS), OutputType = typeof(O_GET_MATCH_PREDICTIONS))]
    public class P_GET_MATCH_PREDICTIONS
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_MATCH_PREDICTIONS + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_MATCH_PREDICTIONS + 1;
        }

        public static O_GET_MATCH_PREDICTIONS Execute(I_GET_MATCH_PREDICTIONS input)
        {
            if (input == null)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            IEnumerable<FootballDB.Tables.Prediction> db_output;
            using (var P_GET_MATCH_PREDICTIONS = new FootballDB.Procedures.P_GET_MATCH_PREDICTIONS())
            {
                P_GET_MATCH_PREDICTIONS.SetInput(new FootballDB.Procedures.P_GET_MATCH_PREDICTIONS.Input
                {
                    FixtureId = input.FixtureId
                });

                db_output = P_GET_MATCH_PREDICTIONS.OnQuery();

                if (P_GET_MATCH_PREDICTIONS.EntityStatus != null || db_output == null)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error);
            }

            var predictions = new List<PacketModels.FootballPredictionDetail>();
            foreach (var db_prediction in db_output)
            {
                var predictionDetail = Singleton.Get<FootballPredictionDetailConverter>().Convert(db_prediction);
                predictions.Add(predictionDetail);
            }

            return new O_GET_MATCH_PREDICTIONS
            {
                PredictionDetails = predictions.ToArray(),
            };
        }
    }
}