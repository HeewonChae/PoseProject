﻿using LogicCore.Utility;
using PosePacket;
using PosePacket.Service.Football;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Logics.Comparer;
using SportsWebService.Logics.Converters;
using SportsWebService.Services;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SportsWebService.Commands.Football
{
    using FootballDB = Repository.Mysql.FootballDB;
    using PacketModels = PosePacket.Service.Football.Models;

    [WebModelType(InputType = typeof(I_GET_MATCH_OVERVIEW), OutputType = typeof(O_GET_MATCH_OVERVIEW))]
    public static class P_GET_MATCH_OVERVIEW
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_MATCH_OVERVIEW + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_MATCH_OVERVIEW + 1;

            [Description("Fixture not found")]
            public const int DB_Fixture_Not_Found = ServiceErrorCode.StoredProcedure_Football.P_GET_MATCH_OVERVIEW + 2;
        }

        public static O_GET_MATCH_OVERVIEW Execute(I_GET_MATCH_OVERVIEW input)
        {
            if (input == null
                || input.FixtureId == 0)
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            // DB Procedure
            FootballDB.Procedures.P_GET_MATCH_OVERVIEW.Output db_output;
            using (var P_GET_MATCH_OVERVIEW = new FootballDB.Procedures.P_GET_MATCH_OVERVIEW())
            {
                P_GET_MATCH_OVERVIEW.SetInput(new FootballDB.Procedures.P_GET_MATCH_OVERVIEW.Input
                {
                    FixtureId = input.FixtureId,
                });

                db_output = P_GET_MATCH_OVERVIEW.OnQuery();

                if (P_GET_MATCH_OVERVIEW.EntityStatus != null || db_output.Result != 0)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error + db_output.Result);
            }

            return ConvertOutput(db_output);
        }

        // Convert output
        private static O_GET_MATCH_OVERVIEW ConvertOutput(FootballDB.Procedures.P_GET_MATCH_OVERVIEW.Output db_output)
        {
            var footballFixtureDetailConverter = Singleton.Get<FootballFixtureDetailConverter>();
            var footballStandingsDetailConverter = Singleton.Get<FootballStandingsDetailConverter>();

            var fixtureDetails = new List<PacketModels.FootballFixtureDetail>();
            foreach (var dbFixtureDetail in db_output.HomeRecntFixtures)
            {
                fixtureDetails.Add(footballFixtureDetailConverter.Convert(dbFixtureDetail));
            }
            foreach (var dbFixtureDetail in db_output.AwayRecentFixtures)
            {
                fixtureDetails.Add(footballFixtureDetailConverter.Convert(dbFixtureDetail));
            }

            var league_FixtureDetails = new List<PacketModels.FootballFixtureDetail>();
            foreach (var dbFixtureDetail in db_output.League_HomeRecentFixtures)
            {
                league_FixtureDetails.Add(footballFixtureDetailConverter.Convert(dbFixtureDetail));
            }
            foreach (var dbFixtureDetail in db_output.League_AwayRecentFixtures)
            {
                league_FixtureDetails.Add(footballFixtureDetailConverter.Convert(dbFixtureDetail));
            }

            var standingsDetail = new List<PacketModels.FootballStandingsDetail>();
            foreach (var dbStandingsDetail in db_output.StandingsDetails)
            {
                standingsDetail.Add(footballStandingsDetailConverter.Convert(dbStandingsDetail));
            }

            return new O_GET_MATCH_OVERVIEW
            {
                // 중복 경기 삭제
                RecentFixtures = fixtureDetails.Distinct(Singleton.Get<FootballFixtureEqualityComparer>()).ToArray(),
                League_RecentFixtures = league_FixtureDetails.Distinct(Singleton.Get<FootballFixtureEqualityComparer>()).ToArray(),
                StandingsDetails = standingsDetail.ToArray(),
            };
        }
    }
}