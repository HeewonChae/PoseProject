using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Model.Football
{
	public class League
	{
		public class CoverageInfo
		{
			public class FixtureCoverageInfo
			{
				public bool Statistics { get; set; }
				public bool Lineups { get; set; }
			}

			public bool Standings { get; set; }
			public bool Players { get; set; }
			public bool Predictions { get; set; }
			public bool Odds { get; set; }
			public FixtureCoverageInfo FixtureCoverage { get; set; }
		}

		public int LeagueID { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Country { get; set; }
		public string Logo { get; set; }
		public DateTime SeasonStart { get; set; }
		public DateTime SeasonEnd { get; set; }
		public int IsCurrent { get; set; }
		public CoverageInfo Coverage { get; set; }
	}
}
