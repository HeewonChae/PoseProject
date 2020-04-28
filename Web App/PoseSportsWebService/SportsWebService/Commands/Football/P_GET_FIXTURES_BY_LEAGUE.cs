using LogicCore.Utility;
using PosePacket;
using PosePacket.Service.Football;
using SportsWebService.Infrastructure;
using SportsWebService.Logics.Converters;
using SportsWebService.Utilities;
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
    [WebModelType(InputType = typeof(I_GET_FIXTURES_BY_LEAGUE), OutputType = typeof(O_GET_FIXTURES_BY_LEAGUE))]
    public static class P_GET_FIXTURES_BY_LEAGUE
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_FIXTURES_BY_LEAGUE + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_FIXTURES_BY_LEAGUE + 1;

            [Description("League not found")]
            public const int DB_League_Not_Found = ServiceErrorCode.StoredProcedure_Football.P_GET_FIXTURES_BY_LEAGUE + 2;
        }

        public static O_GET_FIXTURES_BY_LEAGUE Execute(I_GET_FIXTURES_BY_LEAGUE input)
        {
            if (input == null
                || string.IsNullOrEmpty(input.CountryName)
                || string.IsNullOrEmpty(input.LeagueName))
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL_BY_LEAGUE.Output db_output;
            using (var P_SELECT_FIXTURES_DETAIL_BY_LEAGUE = new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL_BY_LEAGUE())
            {
                P_SELECT_FIXTURES_DETAIL_BY_LEAGUE.SetInput(new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL_BY_LEAGUE.Input
                {
                    SearchFixtureStatusType = input.SearchFixtureStatusType,
                    CountryName = input.CountryName,
                    LeagueName = input.LeagueName,
                });

                db_output = P_SELECT_FIXTURES_DETAIL_BY_LEAGUE.OnQuery();

                if (P_SELECT_FIXTURES_DETAIL_BY_LEAGUE.EntityStatus != null || db_output.Result != 0)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error + db_output.Result);
            }

            var fixtureDetails = new List<PacketModels.FootballFixtureDetail>();
            foreach (var dbFixtureDetail in db_output.FixtureDetails)
            {
                if (dbFixtureDetail == null)
                    continue;

                var fixtureDetail = Singleton.Get<FootballFixtureDetailConverter>().Convert(dbFixtureDetail);
                fixtureDetails.Add(fixtureDetail);
            }

            return new O_GET_FIXTURES_BY_LEAGUE
            {
                Fixtures = fixtureDetails,
            };
        }
    }
}