using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football
{
	public class FixtureStatistic
	{
		public class DetailInfo
		{
			[JsonProperty("home")]
			public string Home { get; set; }

			[JsonProperty("away")]
			public string Away { get; set; }
		}

		[JsonProperty("Total Shots")]
		public DetailInfo TotalShorts { get; set; }
		[JsonProperty("Shots on Goal")]
		public DetailInfo ShotsOnGoal { get; set; }
		[JsonProperty("Shots off Goal")]
		public DetailInfo ShotsOffGoal { get; set; }
		[JsonProperty("Blocked Shots")]
		public DetailInfo BlockedShots { get; set; }
		[JsonProperty("Shots insidebox")]
		public DetailInfo ShotsInsideBox { get; set; }
		[JsonProperty("Shots outsidebox")]
		public DetailInfo ShotsOutsideBox { get; set; }
		[JsonProperty("Goalkeeper Saves")]
		public DetailInfo GoalkeeperSaves { get; set; }

		[JsonProperty("Fouls")]
		public DetailInfo Fouls { get; set; }
		[JsonProperty("Corner Kicks")]
		public DetailInfo CornerKicks { get; set; }
		[JsonProperty("Offsides")]
		public DetailInfo Offsides { get; set; }
		[JsonProperty("Yellow Cards")]
		public DetailInfo YellowCards { get; set; }
		[JsonProperty("Red Cards")]
		public DetailInfo RedCards { get; set; }

		[JsonProperty("Ball Possession")]
		public DetailInfo BallPossession { get; set; }
		[JsonProperty("Total passes")]
		public DetailInfo TotalPasses { get; set; }
		[JsonProperty("Passes accurate")]
		public DetailInfo AccuratePasses { get; set; }
	}
}
