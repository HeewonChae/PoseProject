using LogicCore.Utility;
using PosePacket;
using PosePacket.Service.Football;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Logics.Converters;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SportsWebService.Commands.Football
{
    using FootballDB = Repository.Mysql.FootballDB;
    using PacketModels = PosePacket.Service.Football.Models;

    [WebModelType(InputType = typeof(I_GET_FIXTURES_BY_TEAM), OutputType = typeof(O_GET_FIXTURES_BY_TEAM))]
    public static class P_GET_FIXTURES_BY_TEAM
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_FIXTURES_BY_TEAM + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_FIXTURES_BY_TEAM + 1;

            [Description("Team not found")]
            public const int DB_Team_Not_Found = ServiceErrorCode.StoredProcedure_Football.P_GET_FIXTURES_BY_TEAM + 2;
        }

        public static O_GET_FIXTURES_BY_TEAM Execute(I_GET_FIXTURES_BY_TEAM input)
        {
            if (input == null
                || input.TeamId == 0)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL_BY_TEAM.Output db_output;
            using (var P_SELECT_FIXTURES_DETAIL_BY_TEAM = new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL_BY_TEAM())
            {
                P_SELECT_FIXTURES_DETAIL_BY_TEAM.SetInput(new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL_BY_TEAM.Input
                {
                    SearchFixtureStatusType = input.SearchFixtureStatusType,
                    TeamId = input.TeamId,
                });

                db_output = P_SELECT_FIXTURES_DETAIL_BY_TEAM.OnQuery();

                if (P_SELECT_FIXTURES_DETAIL_BY_TEAM.EntityStatus != null || db_output.Result != 0)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error + db_output.Result);
            }

            var fixtureDetails = new List<PacketModels.FootballFixtureDetail>();
            foreach (var dbFixtureDetail in db_output.FixtureDetails)
            {
                var fixtureDetail = Singleton.Get<FootballFixtureDetailConverter>().Convert(dbFixtureDetail);
                fixtureDetails.Add(fixtureDetail);
            }

            return new O_GET_FIXTURES_BY_TEAM
            {
                Fixtures = fixtureDetails.ToArray(),
            };
        }
    }
}