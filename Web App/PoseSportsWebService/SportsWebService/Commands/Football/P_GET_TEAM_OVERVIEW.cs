using LogicCore.Utility;
using PosePacket;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Logics.Converters;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Windows.Input;

namespace SportsWebService.Commands.Football
{
    using FootballDB = Repository.Mysql.FootballDB;
    using PacketModels = PosePacket.Service.Football.Models;

    [WebModelType(InputType = typeof(I_GET_TEAM_OVERVIEW), OutputType = typeof(O_GET_TEAM_OVERVIEW))]
    public static class P_GET_TEAM_OVERVIEW
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_TEAM_OVERVIEW + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_TEAM_OVERVIEW + 1;

            [Description("League not found")]
            public const int DB_League_Not_Found = ServiceErrorCode.StoredProcedure_Football.P_GET_TEAM_OVERVIEW + 2;
        }

        public static O_GET_TEAM_OVERVIEW Execute(I_GET_TEAM_OVERVIEW input)
        {
            if (input == null
                || input.TeamId == 0)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            // DB Procedure
            FootballDB.Procedures.P_GET_TEAM_OVERVIEW.Output db_output;
            using (var P_GET_TEAM_OVERVIEW = new FootballDB.Procedures.P_GET_TEAM_OVERVIEW())
            {
                P_GET_TEAM_OVERVIEW.SetInput(new FootballDB.Procedures.P_GET_TEAM_OVERVIEW.Input
                {
                    TeamId = input.TeamId,
                });

                db_output = P_GET_TEAM_OVERVIEW.OnQuery();

                if (P_GET_TEAM_OVERVIEW.EntityStatus != null || db_output.Result != 0)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error + db_output.Result);
            }

            return ConvertOutput(db_output);
        }

        // Convert output
        private static O_GET_TEAM_OVERVIEW ConvertOutput(FootballDB.Procedures.P_GET_TEAM_OVERVIEW.Output db_output)
        {
            var footballFixtureDetailConverter = Singleton.Get<FootballFixtureDetailConverter>();

            var fixtureDetails = new List<FootballFixtureDetail>();

            foreach (var fixtureDetail in db_output.DB_FixtureDetails)
            {
                fixtureDetails.Add(footballFixtureDetailConverter.Convert(fixtureDetail));
            }

            return new O_GET_TEAM_OVERVIEW
            {
                FixtureDetails = fixtureDetails.ToArray(),
            };
        }
    }
}