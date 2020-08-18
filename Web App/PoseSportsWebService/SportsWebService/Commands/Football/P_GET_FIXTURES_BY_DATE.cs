using LogicCore.Converter;
using LogicCore.Utility;
using PosePacket;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models.Enums;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Logics.Converters;
using SportsWebService.Services;
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
    [WebModelType(InputType = typeof(I_GET_FIXTURES_BY_DATE), OutputType = typeof(O_GET_FIXTURES_BY_DATE))]
    public static class P_GET_FIXTURES_BY_DATE
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_FIXTURES_BY_DATE + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_FIXTURES_BY_DATE + 1;
        }

        public static O_GET_FIXTURES_BY_DATE Execute(I_GET_FIXTURES_BY_DATE input)
        {
            if (input == null)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            IEnumerable<FootballDB.OutputModels.DB_FootballFixtureDetail> db_output;
            using (var P_SELECT_FIXTURES_BY_DATE = new FootballDB.Procedures.P_SELECT_FIXTURES_BY_DATE())
            {
                P_SELECT_FIXTURES_BY_DATE.SetInput(new FootballDB.Procedures.P_SELECT_FIXTURES_BY_DATE.Input
                {
                    StartTime = input.StartTime,
                    EndTime = input.EndTime,
                });

                db_output = P_SELECT_FIXTURES_BY_DATE.OnQuery();

                if (P_SELECT_FIXTURES_BY_DATE.EntityStatus != null || db_output == null)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error);
            }

            var fixtures = new List<PacketModels.FootballFixtureDetail>();
            foreach (var dbFixtureDetail in db_output)
            {
                var fixtureDetail = Singleton.Get<FootballFixtureDetailConverter>().Convert(dbFixtureDetail);
                fixtures.Add(fixtureDetail);
            }

            return new O_GET_FIXTURES_BY_DATE
            {
                Fixtures = fixtures.ToArray(),
            };
        }
    }
}