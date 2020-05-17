using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Commands.Football
{
    using FootballDB = Repository.Mysql.FootballDB;
    using PacketModels = PosePacket.Service.Football.Models;
    using PosePacket.Service.Football;
    using SportsWebService.Infrastructure;
    using PosePacket;
    using System.ComponentModel;
    using SportsWebService.Utilities;
    using LogicCore.Utility;
    using SportsWebService.Logics.Converters;
    using SportsWebService.Logics;

    [WebModelType(InputType = typeof(I_GET_MATCH_ODDS), OutputType = typeof(O_GET_MATCH_ODDS))]
    public static class P_GET_MATCH_ODDS
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_MATCH_ODDS + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_MATCH_ODDS + 1;
        }

        public static O_GET_MATCH_ODDS Execute(I_GET_MATCH_ODDS input)
        {
            if (input == null
                || input.FixtureId == 0)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            // DB Procedure
            IEnumerable<FootballDB.Tables.Odds> db_output;
            using (var P_GET_MATCH_ODDS = new FootballDB.Procedures.P_GET_MATCH_ODDS())
            {
                P_GET_MATCH_ODDS.SetInput(new FootballDB.Procedures.P_GET_MATCH_ODDS.Input
                {
                    FixtureId = input.FixtureId,
                });

                db_output = P_GET_MATCH_ODDS.OnQuery();

                if (P_GET_MATCH_ODDS.EntityStatus != null)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error);
            }

            return ConvertOutput(db_output);
        }

        // Convert output
        private static O_GET_MATCH_ODDS ConvertOutput(IEnumerable<FootballDB.Tables.Odds> db_output)
        {
            var oddsDetailConverter = Singleton.Get<FootballOddsDetailConverter>();

            var oddsDetails = new List<PacketModels.FootballOddsDetail>();
            foreach (var dbOddsDetail in db_output)
            {
                oddsDetails.Add(oddsDetailConverter.Convert(dbOddsDetail));
            }

            return new O_GET_MATCH_ODDS
            {
                OddsDetails = oddsDetails.ToArray(),
            };
        }
    }
}