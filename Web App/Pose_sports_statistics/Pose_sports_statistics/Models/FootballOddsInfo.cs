using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Models
{
	public class FootballOddsInfo
	{
		public class BookmakerInfo
		{
			public class BetInfo
			{
				public class BetValue
				{
					public string Name;
					public float Odds;
				}

				public int LabelID { get; set; }
				public string LabelName { get; set; }
				public BetValue[] BetValues { get; set; }
			}

			public int BookmakerID { get; set; }
			public string BookmakerName { get; set; }
			public BetInfo[] BetInfos { get; set; }
		}

		public BookmakerInfo[] Bookmakers { get; set; }
	}
}