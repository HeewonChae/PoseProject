using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Logic.RapidAPI
{
	public static class RedisKeyMaker
	{
		static readonly string rootDirectoryName = "FootBallAPI";

		public static string FootballFixtureByID(int fixtureID)
		{
			return $"{rootDirectoryName}:FixtureByID:{fixtureID}";
		}

		public static string FootballFixtureByTeamID(int teamID)
		{
			return $"{rootDirectoryName}:FixtureByTeamID:{teamID}";
		}
		public static string FootballFixturesByDate(DateTime date)
		{
			return $"{rootDirectoryName}:FixturesByDate:{date.ToString("yyyy_MM_dd")}";
		}

		public static string FootballStandingsByLeagueID(int leagueID)
		{
			return $"{rootDirectoryName}:StandingsByLeagueID:{leagueID}";
		}

		public static string FootballTeamStatisticsByLeagueIDAndTeamID(int leagueID, int teamID)
		{
			return $"{rootDirectoryName}:TeamStatisticsByLeagueIDAndTeamID:{leagueID}:{teamID}";
		}

		public static string FootballH2HFixtureByTeamID(int teamID1, int teamID2)
		{
			return $"{rootDirectoryName}:FootballH2HFixtureByTeamID:{teamID1}:{teamID2}";
		}

		public static string FootballSeasonsByLeagueID(int leagueID)
		{
			return $"{rootDirectoryName}:FootballSeasonsByLeagueID:{leagueID}";
		}

		public static string FootballPlayersByTeamIDAndSeason(int teamID, string season)
		{
			return $"{rootDirectoryName}:FootballPlayersByTeamIDAndSeason:{teamID}:{season}";
		}

		public static string FootballPredictionByFixtureID(int fixtureID)
		{
			return $"{rootDirectoryName}:FootballPredictionByFixtureID:{fixtureID}";
		}

		public static string FootballTopScorerByLeagueID(int leagueID)
		{
			return $"{rootDirectoryName}:FootballTopScorerByLeagueID:{leagueID}";
		}

		public static string FootballInterestedFixture()
		{
			return $"{rootDirectoryName}:FootballInterestedFixture";
		}
	}
}