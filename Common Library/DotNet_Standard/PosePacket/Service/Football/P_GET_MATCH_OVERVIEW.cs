using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    public class I_GET_MATCH_OVERVIEW
    {
        public int FixtureId { get; set; }
    }

    public class O_GET_MATCH_OVERVIEW
    {
        public List<FootballFixtureDetail> HomeRecentFixtures { get; set; }
        public List<FootballFixtureDetail> AwayRecentFixtures { get; set; }

        public List<FootballFixtureDetail> League_HomeRecentFixtures { get; set; }
        public List<FootballFixtureDetail> Leauge_AwayRecentFixtures { get; set; }

        public List<FootballStandingsDetail> StandingsDetails { get; set; }
    }
}