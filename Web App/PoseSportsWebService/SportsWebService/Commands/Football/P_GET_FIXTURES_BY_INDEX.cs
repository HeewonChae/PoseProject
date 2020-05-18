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

using FootballDB = Repository.Mysql.FootballDB;
using PacketModels = PosePacket.Service.Football.Models;

namespace SportsWebService.Commands.Football
{
    [WebModelType(InputType = typeof(I_GET_FIXTURES_BY_INDEX), OutputType = typeof(O_GET_FIXTURES_BY_INDEX))]
    public static class P_GET_FIXTURES_BY_INDEX
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_FIXTURES_BY_INDEX + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_FIXTURES_BY_INDEX + 1;
        }

        public static O_GET_FIXTURES_BY_INDEX Execute(I_GET_FIXTURES_BY_INDEX input)
        {
            if (input == null
                || input.FixtureIds == null
                || input.FixtureIds.Length == 0)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            IEnumerable<FootballDB.OutputModels.DB_FootballFixtureDetail> db_output;
            using (var P_SELECT_FIXTURES_DETAIL_BY_INDEX = new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL_BY_INDEX())
            {
                P_SELECT_FIXTURES_DETAIL_BY_INDEX.SetInput(new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL_BY_INDEX.Input
                {
                    Indexes = input.FixtureIds.ToArray(),
                });

                db_output = P_SELECT_FIXTURES_DETAIL_BY_INDEX.OnQuery();

                if (P_SELECT_FIXTURES_DETAIL_BY_INDEX.EntityStatus != null || db_output == null)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error);
            }

            var fixtureDetails = new List<PacketModels.FootballFixtureDetail>();
            foreach (var dbFixtureDetail in db_output)
            {
                if (dbFixtureDetail == null)
                    continue;

                var fixtureDetail = Singleton.Get<FootballFixtureDetailConverter>().Convert(dbFixtureDetail);
                fixtureDetails.Add(fixtureDetail);
            }

            return new O_GET_FIXTURES_BY_INDEX
            {
                Fixtures = fixtureDetails.ToArray(),
            };
        }
    }
}