using LogicCore.Utility;
using PosePacket;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models;
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
using System.Windows.Input;

namespace SportsWebService.Commands.Football
{
    using FootballDB = Repository.Mysql.FootballDB;
    using PacketModels = PosePacket.Service.Football.Models;

    [WebModelType(InputType = typeof(I_GET_LEAGUE_OVERVIEW), OutputType = typeof(O_GET_LEAGUE_OVERVIEW))]
    public static class P_GET_LEAGUE_OVERVIEW
    {
        public static class RowCode
        {
            [Description("Invalid input")]
            public const int Invalid_Input = ServiceErrorCode.WebMethod_Football.P_GET_LEAGUE_OVERVIEW + 1;

            [Description("Failed db error")]
            public const int DB_Failed_Error = ServiceErrorCode.StoredProcedure_Football.P_GET_LEAGUE_OVERVIEW + 1;

            [Description("League not found")]
            public const int DB_League_Not_Found = ServiceErrorCode.StoredProcedure_Football.P_GET_LEAGUE_OVERVIEW + 2;
        }

        public static O_GET_LEAGUE_OVERVIEW Execute(I_GET_LEAGUE_OVERVIEW input)
        {
            if (input == null
                || string.IsNullOrEmpty(input.CountryName)
                || string.IsNullOrEmpty(input.LeagueName))
                ErrorHandler.OccurException(RowCode.Invalid_Input);

            // DB Procedure
            FootballDB.Procedures.P_GET_LEAGUE_OVERVIEW.Output db_output;
            using (var P_GET_LEAGUE_OVERVIEW = new FootballDB.Procedures.P_GET_LEAGUE_OVERVIEW())
            {
                P_GET_LEAGUE_OVERVIEW.SetInput(new FootballDB.Procedures.P_GET_LEAGUE_OVERVIEW.Input
                {
                    CountryName = input.CountryName,
                    LeagueName = input.LeagueName,
                });

                db_output = P_GET_LEAGUE_OVERVIEW.OnQuery();

                if (P_GET_LEAGUE_OVERVIEW.EntityStatus != null || db_output.Result != 0)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Error + db_output.Result);
            }

            return ConvertOutput(db_output);
        }

        // Convert output
        private static O_GET_LEAGUE_OVERVIEW ConvertOutput(FootballDB.Procedures.P_GET_LEAGUE_OVERVIEW.Output db_output)
        {
            var leagueDetailConverter = Singleton.Get<FootballLeagueDetailConverter>();
            var teamDetailConverter = Singleton.Get<FootballTeamDetailConverter>();
            var standingsDetailConverter = Singleton.Get<FootballStandingsDetailConverter>();

            var leagueDetail = leagueDetailConverter.Convert(db_output.LeagueDetail);

            var standignsDetails = new List<FootballStandingsDetail>();
            foreach (var db_standings in db_output.StandingsDetails)
            {
                standignsDetails.Add(standingsDetailConverter.Convert(db_standings));
            }

            var teamDetails = new List<FootballTeamDetail>();
            if (db_output.ParticipatingTeamDetails != null)
            {
                foreach (var db_team in db_output.ParticipatingTeamDetails)
                {
                    teamDetails.Add(teamDetailConverter.Convert(db_team));
                }
            }

            return new O_GET_LEAGUE_OVERVIEW
            {
                LeagueDetail = leagueDetail,
                ParticipatingTeams = teamDetails.ToArray(),
                StandingsDetails = standignsDetails.ToArray(),
            };
        }
    }
}