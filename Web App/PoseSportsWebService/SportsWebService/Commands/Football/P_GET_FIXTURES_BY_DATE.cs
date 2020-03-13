using PosePacket;
using PosePacket.Service.Football;
using SportsWebService.Infrastructure;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

using FootballDB = Repository.Mysql.FootballDB;

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
            public const int Failed_DB_Error = ServiceErrorCode.WebMethod_Football.P_GET_FIXTURES_BY_DATE + 2;
        }

        public static O_GET_FIXTURES_BY_DATE Execute(I_GET_FIXTURES_BY_DATE input)
        {
            if (input == null)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            IEnumerable<FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL.Output> db_output;
            using (var P_SELECT_FIXTURES_BY_DATE = new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL())
            {
                P_SELECT_FIXTURES_BY_DATE.SetInput(new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL.Input
                {
                    StartTime = input.StartTime,
                    EndTime = input.EndTime,
                });

                db_output = P_SELECT_FIXTURES_BY_DATE.OnQuery();

                if (P_SELECT_FIXTURES_BY_DATE.EntityStatus != null || db_output == null)
                    ErrorHandler.OccurException(RowCode.Failed_DB_Error);
            }

            var output = new O_GET_FIXTURES_BY_DATE();
            output.Fixtures = new List<O_GET_FIXTURES_BY_DATE.FixtureInfo>();

            foreach (var dbFixtureDetail in db_output)
            {
                output.Fixtures.Add(DBFixtureDetailConverter(dbFixtureDetail));
            }

            return output;
        }

        private static readonly Converter<FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL.Output, O_GET_FIXTURES_BY_DATE.FixtureInfo> DBFixtureDetailConverter =
        (input) =>
        {
            var output = new O_GET_FIXTURES_BY_DATE.FixtureInfo
            {
                Country = new O_GET_FIXTURES_BY_DATE.FixtureInfo.DataInfo
                {
                    Name = input.CountryName,
                    Logo = input.CountryLogo,
                },
                League = new O_GET_FIXTURES_BY_DATE.FixtureInfo.DataInfo
                {
                    Name = input.LeagueName,
                    Logo = input.LeagueLogo,
                },
                HomeTeam = new O_GET_FIXTURES_BY_DATE.FixtureInfo.DataInfo
                {
                    Name = input.HomeTeamName,
                    Logo = input.HomeTeamLogo,
                },
                AwayTeam = new O_GET_FIXTURES_BY_DATE.FixtureInfo.DataInfo
                {
                    Name = input.AwayTeamName,
                    Logo = input.AwayTeamLogo,
                },
                MatchStatus = input.MatchStatus,
                MatchTime = input.MatchTime,
            };

            return output;
        };
    }
}