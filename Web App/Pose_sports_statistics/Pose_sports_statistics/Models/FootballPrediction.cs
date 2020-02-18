using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Models
{
	public class FootballPrediction
	{
		public class PredictionPG
		{
			public string Home { get; set; }
			public string Away { get; set; }
			public string Draw { get; set; }
		}

		public class ComparisonInfo
		{
			public class ComparisonDetail
			{
				public string Home { get; set; }
				public string Away { get; set; }
			}

			public ComparisonDetail Forme { get; set; }
			public ComparisonDetail Attack { get; set; }
			public ComparisonDetail Defense { get; set; }
			public ComparisonDetail H2H { get; set; }
			public ComparisonDetail GoalsH2H { get; set; }
		}

		public string MatchWinner { get; set; }
		public string UnderOver { get; set; }
		public string GoalsHome { get; set; }
		public string GoalsAway { get; set; }
		public string Advice { get; set; }
		public PredictionPG WinningPG { get; set; }
		public ComparisonInfo Comparison { get; set; }
	}
}