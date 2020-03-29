using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Logic.RapidAPI
{
    public static class RedisKeyMaker
    {
        private static readonly string rootDirectoryName = "FootBallAPI";

        public static string FootballFixtureById(int fixtureId)
        {
            return $"{rootDirectoryName}:FixtureById:{fixtureId}";
        }

        public static string FootballFixtureByTeamId(int teamId)
        {
            return $"{rootDirectoryName}:FixtureByTeamId:{teamId}";
        }

        public static string FootballFixturesByDate(DateTime date)
        {
            return $"{rootDirectoryName}:FixturesByDate:{date.ToString("yyyy_MM_dd")}";
        }

        public static string FootballStandingsByLeagueId(int leagueId)
        {
            return $"{rootDirectoryName}:StandingsByLeagueId:{leagueId}";
        }

        public static string FootballTeamStatisticsByLeagueIdAndTeamId(int leagueId, int teamId)
        {
            return $"{rootDirectoryName}:TeamStatisticsByLeagueIDAndTeamId:{leagueId}:{teamId}";
        }

        public static string FootballH2HFixtureByTeamId(int teamId1, int teamId2)
        {
            if (teamId1 > teamId2)
            {
                int temp = teamId1;
                teamId2 = teamId1;
                teamId1 = temp;
            }

            return $"{rootDirectoryName}:FootballH2HFixtureByTeamId:{teamId1}:{teamId2}";
        }

        public static string FootballSeasonsByLeagueId(int leagueId)
        {
            return $"{rootDirectoryName}:FootballSeasonsByLeagueId:{leagueId}";
        }

        public static string FootballPlayersByTeamIdAndSeason(int teamId, string season)
        {
            return $"{rootDirectoryName}:FootballPlayersByTeamIdAndSeason:{teamId}:{season}";
        }

        public static string FootballPredictionByFixtureId(int fixtureId)
        {
            return $"{rootDirectoryName}:FootballPredictionByFixtureId:{fixtureId}";
        }

        public static string FootballTopScorerByLeagueId(int leagueId)
        {
            return $"{rootDirectoryName}:FootballTopScorerByLeagueId:{leagueId}";
        }

        public static string FootballInterestedFixture()
        {
            return $"{rootDirectoryName}:FootballInterestedFixture";
        }
    }
}