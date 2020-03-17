using LogicCore.Converter;
using PosePacket;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models.Enums;
using SportsWebService.Infrastructure;
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

            IEnumerable<FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL.Output> db_output;
            using (var P_SELECT_FIXTURES_BY_DATE = new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL())
            {
                P_SELECT_FIXTURES_BY_DATE.SetInput(new FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL.Input
                {
                    WHERE = $"f.{nameof(FootballDB.Tables.Fixture.match_time)} BETWEEN \"{input.StartTime.ToString("yyyyMMddTHHmmss")}\" AND \"{input.EndTime.ToString("yyyyMMddTHHmmss")}\"",
                });

                db_output = P_SELECT_FIXTURES_BY_DATE.OnQuery();

                if (P_SELECT_FIXTURES_BY_DATE.EntityStatus != null || db_output == null)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error);
            }

            var output = new O_GET_FIXTURES_BY_DATE
            {
                Fixtures = new List<PacketModels.FixtureDetail>()
            };

            foreach (var dbFixtureDetail in db_output)
            {
                output.Fixtures.Add(FixtureDetailConverter(dbFixtureDetail));
            }

            return output;
        }

        private static readonly Converter<FootballDB.Procedures.P_SELECT_FIXTURES_DETAIL.Output, PacketModels.FixtureDetail> FixtureDetailConverter =
        (input) =>
        {
            input.MatchStatus.TryParseEnum(out FootballFixtureStatusType statusType);
            input.LeagueType.TryParseEnum(out FootballLeagueType leagueType);

            var output = new PacketModels.FixtureDetail
            {
                Country = new PacketModels.FixtureDetail.DataInfo
                {
                    Name = input.CountryName,
                    Logo = input.CountryLogo,
                },
                League = new PacketModels.FixtureDetail.DataInfo
                {
                    Name = input.LeagueName,
                    Logo = input.LeagueLogo,
                },
                HomeTeam = new PacketModels.FixtureDetail.TeamInfo
                {
                    Name = input.HomeTeamName,
                    Logo = input.HomeTeamLogo,
                    Score = input.HomeTeamScore,
                },
                AwayTeam = new PacketModels.FixtureDetail.TeamInfo
                {
                    Name = input.AwayTeamName,
                    Logo = input.AwayTeamLogo,
                    Score = input.AwayTeamScore
                },
                FixtureId = input.FixtureId,
                MatchStatus = statusType,
                MatchTime = input.MatchTime,
                LeagueType = leagueType,
            };

            return output;
        };
    }
}