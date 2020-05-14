using LogicCore.Utility;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Logics.Comparer
{
    public class FootballFixtureEqualityComparer : IEqualityComparer<FootballFixtureDetail>, Singleton.INode
    {
        public bool Equals(FootballFixtureDetail x, FootballFixtureDetail y)
        {
            return x.FixtureId == y.FixtureId;
        }

        public int GetHashCode(FootballFixtureDetail obj)
        {
            return obj.FixtureId;
        }
    }
}