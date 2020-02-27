using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football
{
	public class Prediction
	{
		public class PredictionPG
		{
			[JsonProperty("home")]
			public string Home { get; set; }

			[JsonProperty("away")]
			public string Away { get; set; }

			[JsonProperty("draws")]
			public string Draw { get; set; }
		}

		public class ComparisonInfo
		{
			public class ComparisonDetail
			{
				[JsonProperty("home")]
				public string Home { get; set; }

				[JsonProperty("away")]
				public string Away { get; set; }
			}

			[JsonProperty("Forme")]
			public ComparisonDetail Forme { get; set; }

			[JsonProperty("att")]
			public ComparisonDetail Attack { get; set; }

			[JsonProperty("def")]
			public ComparisonDetail Defense { get; set; }

			[JsonProperty("h2h")]
			public ComparisonDetail H2H { get; set; }

			[JsonProperty("goals_h2h")]
			public ComparisonDetail GoalsH2H { get; set; }
		}

		[JsonProperty("match_winner")]
		public string MatchWinner { get; set; }

		[JsonProperty("under_over")]
		public string UnderOver { get; set; }

		[JsonProperty("goals_home")]
		public string GoalsHome { get; set; }

		[JsonProperty("goals_away")]
		public string GoalsAway { get; set; }

		[JsonProperty("advice")]
		public string Advice { get; set; }

		[JsonProperty("winning_percent")]
		public PredictionPG WinningPG { get; set; }

		[JsonProperty("comparison")]
		public ComparisonInfo Comparison { get; set; }
	}
}
