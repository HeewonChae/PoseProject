using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Input;

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
    using SportsWebService.Services;

    [WebModelType(InputType = typeof(I_GET_MATCH_H2H), OutputType = typeof(O_GET_MATCH_H2H))]
    public static class P_GET_MATCH_H2H
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_MATCH_H2H + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_MATCH_H2H + 1;
        }

        public static O_GET_MATCH_H2H Execute(I_GET_MATCH_H2H input)
        {
            if (input == null
                || input.HomeTeamId == 0
                || input.AwayTeamId == 0
                || input.FixtureId == 0)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            // DB Procedure
            FootballDB.Procedures.P_GET_MATCH_H2H.Output db_output;
            using (var P_GET_MATCH_H2H = new FootballDB.Procedures.P_GET_MATCH_H2H())
            {
                P_GET_MATCH_H2H.SetInput(new FootballDB.Procedures.P_GET_MATCH_H2H.Input
                {
                    FixtureId = input.FixtureId,
                    HomeTeamId = input.HomeTeamId,
                    AwayTeamId = input.AwayTeamId,
                });

                db_output = P_GET_MATCH_H2H.OnQuery();

                if (P_GET_MATCH_H2H.EntityStatus != null)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error);
            }

            return ConvertOutput(db_output);
        }

        // Convert output
        private static O_GET_MATCH_H2H ConvertOutput(FootballDB.Procedures.P_GET_MATCH_H2H.Output db_output)
        {
            var footballFixtureDetailConverter = Singleton.Get<FootballFixtureDetailConverter>();

            var fixtureDetails = new List<PacketModels.FootballFixtureDetail>();
            foreach (var dbFixtureDetail in db_output.H2HFixtures)
            {
                fixtureDetails.Add(footballFixtureDetailConverter.Convert(dbFixtureDetail));
            }

            return new O_GET_MATCH_H2H
            {
                H2HFixtures = fixtureDetails.ToArray(),
            };
        }
    }
}