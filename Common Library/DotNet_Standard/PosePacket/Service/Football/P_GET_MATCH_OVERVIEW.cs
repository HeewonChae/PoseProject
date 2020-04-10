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
        public List<FootballFixtureDetail> HomeLastFixtures { get; set; }
        public List<FootballFixtureDetail> AwayLastFixtures { get; set; }

        public List<FootballFixtureDetail> League_HomeLastFixtures { get; set; }
        public List<FootballFixtureDetail> Leauge_AwayLastFixtures { get; set; }
    }
}