using SportsAdminTool.Model.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppModel = SportsAdminTool.Model;

namespace SportsAdminTool.Logic.Comparer
{
	public class AppFootballFixtureComparer : IEqualityComparer<AppModel.Football.Fixture>
	{
		public bool Equals(Fixture x, Fixture y)
		{
			return x.FixtureId == y.FixtureId;
		}

		public int GetHashCode(Fixture obj)
		{
			return obj.FixtureId;
		}
	}
}
