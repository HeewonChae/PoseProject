using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football
{
	public class TeamStatistics
	{
		public class MatchsInfo
		{
			[JsonProperty("matchsPlayed")]
			public PlayedInfo MatchsPlayed { get; set; }

			[JsonProperty("wins")]
			public PlayedInfo Wins { get; set; }

			[JsonProperty("draws")]
			public PlayedInfo Draws { get; set; }

			[JsonProperty("loses")]
			public PlayedInfo Loses { get; set; }
		}

		public class GoalsInfo
		{
			[JsonProperty("goalsFor")]
			public PlayedInfo GoalsFor { get; set; }

			[JsonProperty("goalsAgainst")]
			public PlayedInfo GoalsAgainst { get; set; }
		}

		public class GoalsAvgInfo
		{
			[JsonProperty("goalsFor")]
			public PlayedInfo GoalsFor { get; set; }

			[JsonProperty("goalsAgainst")]
			public PlayedInfo GoalsAgainst { get; set; }
		}

		public class PlayedInfo
		{
			[JsonProperty("home")]
			public float Home { get; set; }

			[JsonProperty("away")]
			public float Away { get; set; }

			[JsonProperty("total")]
			public float Total { get; set; }
		}

		[JsonProperty("matchs")]
		public MatchsInfo Matchs { get; set; }

		[JsonProperty("goals")]
		public GoalsInfo Goals { get; set; }

		[JsonProperty("goalsAvg")]
		public GoalsAvgInfo GoalsAvg { get; set; }
	}
}
