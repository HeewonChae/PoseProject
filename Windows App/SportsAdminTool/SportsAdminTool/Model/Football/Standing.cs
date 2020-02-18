using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Model.Football
{
	public class Standing
	{
		public class MatchsPlayedInfo
		{
			public int Played { get; set; }
			public int Win { get; set; }
			public int Draw { get; set; }
			public int Lose { get; set; }
			public int GoalsFor { get; set; }
			public int GoalsAgainst { get; set; }
		}

		public int Rank { get; set; }
		public int TeamID { get; set; }
		public string TeamName { get; set; }
		public string Logo { get; set; }
		public string Group { get; set; }
		public string Forme { get; set; }
		public string Description { get; set; }
		public MatchsPlayedInfo AllPlayedInfo { get; set; }
		public int GoalsDiff { get; set; }
		public int Points { get; set; }
	}
}
