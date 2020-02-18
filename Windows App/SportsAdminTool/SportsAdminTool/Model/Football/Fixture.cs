using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiModel = RapidAPI.Models;

namespace SportsAdminTool.Model.Football
{
	public class Fixture
	{
		public class LeagueInfo
		{
			public string Name { get; set; }
			public string Country { get; set; }
		}

		public class TeamInfo
		{
			public int TeamID { get; set; }
			public string TeamName { get; set; }
		}

		public int FixtureID { get; set; }
		public int LeagueID { get; set; }
		public LeagueInfo League { get; set; }
		public DateTime EventDate { get; set; }
		public string Round { get; set; }
		public ApiModel.Football.Enums.FixtureStatusType Status { get; set; }
		public TeamInfo HomeTeam { get; set; }
		public TeamInfo AwayTeam { get; set; }
		public int GoalsHomeTeam { get; set; }
		public int GoalsAwayTeam { get; set; }
		public FixtureStatistic Statistic { get; set; }
	}
}
