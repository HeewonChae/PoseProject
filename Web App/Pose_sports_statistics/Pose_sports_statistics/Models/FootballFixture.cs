using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Models
{
	public class FootballFixture
	{
		public class LeagueInfo
		{
			public string Name { get; set; }
			public string Country { get; set; }
			public string Logo { get; set; }
			public string Flag { get; set; }
		}

		public class TeamInfo
		{
			public int TeamID { get; set; }
			public string TeamName { get; set; }
			public string Logo { get; set; }
		}

		public class ScoreInfo
		{
			public string HalfTime { get; set; }
			public string FullTime { get; set; }
		}

		public int FixtureID { get; set; }
		public int LeagueID { get; set; }
		public LeagueInfo League { get; set; }
		public DateTime EventDate { get; set; }
		public string Round { get; set; }
		public string Status { get; set; }
		public string Venue { get; set; }
		public TeamInfo HomeTeam { get; set; }
		public TeamInfo AwayTeam { get; set; }
		public string HomeOdds { get; set; } = "0";
		public string DrawOdds { get; set; } = "0";
		public string AwayOdds { get; set; } = "0";
		public ScoreInfo Score { get; set; }

		// 최근 6경기 득실점
		[NonSerialized]
		public string HomeLateSixGoalPoints;

		[NonSerialized]
		public string AwayLateSixGoalPoints;

		// 최근 3경기 홈,원정 득실점
		[NonSerialized]
		public string HomeLateThreeGoalPoints;

		[NonSerialized]
		public string AwayLateThreeGoalPoints;

		// 회복기간
		[NonSerialized]
		public string HomeRecoveryDays;

		[NonSerialized]
		public string AwayRecoveryDays;
	}
}