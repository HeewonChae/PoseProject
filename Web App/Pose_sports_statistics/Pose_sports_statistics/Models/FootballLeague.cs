using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Models
{
	public class FootballLeague : FootballFixture.LeagueInfo
	{
		public class CoverageInfo
		{
			public bool Standings { get; set; }
			public bool Players { get; set; }
			public bool TopScorers { get; set; }
			public bool Predictions { get; set; }
			public bool Odds { get; set; }
		}

		public int LeagueID { get; set; }
		public string Season { get; set; }
		public DateTime SeasonStart { get; set; }
		public DateTime SeasonEnd { get; set; }
		public int IsCurrent { get; set; }
		public CoverageInfo Coverage { get; set; }
	}
}