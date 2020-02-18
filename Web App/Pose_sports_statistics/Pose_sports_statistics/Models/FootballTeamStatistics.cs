using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Models
{
	public class FootballTeamStatistics
	{
		public class MatchsInfo
		{
			public PlayedInfo MatchsPlayed { get; set; }
			public PlayedInfo Wins { get; set; }
			public PlayedInfo Draws { get; set; }
			public PlayedInfo Loses { get; set; }
		}

		public class GoalsInfo
		{
			public PlayedInfo GoalsFor { get; set; }
			public PlayedInfo GoalsAgainst { get; set; }
		}

		public class GoalsAvgInfo
		{
			public PlayedInfo GoalsFor { get; set; }
			public PlayedInfo GoalsAgainst { get; set; }
		}

		public class PlayedInfo
		{
			public float Home { get; set; }
			public float Away { get; set; }
			public float Total { get; set; }
		}

		public MatchsInfo Matchs { get; set; }
		public GoalsInfo Goals { get; set; }
		public GoalsAvgInfo GoalsAvg { get; set; }
	}
}