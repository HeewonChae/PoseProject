using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Model.Football
{
	public class FixtureStatistic
	{
		public class DetailInfo
		{
			public string Home { get; set; }
			public string Away { get; set; }
		}

		public DetailInfo TotalShorts { get; set; }
		public DetailInfo ShotsOnGoal { get; set; }
		public DetailInfo ShotsOffGoal { get; set; }
		public DetailInfo BlockedShots { get; set; }
		public DetailInfo ShotsInsideBox { get; set; }
		public DetailInfo ShotsOutsideBox { get; set; }
		public DetailInfo GoalkeeperSaves { get; set; }

		public DetailInfo Fouls { get; set; }
		public DetailInfo CornerKicks { get; set; }
		public DetailInfo Offsides { get; set; }
		public DetailInfo YellowCards { get; set; }
		public DetailInfo RedCards { get; set; }

		public DetailInfo BallPossession { get; set; }
		public DetailInfo TotalPasses { get; set; }
		public DetailInfo AccuratePasses { get; set; }
	}
}
