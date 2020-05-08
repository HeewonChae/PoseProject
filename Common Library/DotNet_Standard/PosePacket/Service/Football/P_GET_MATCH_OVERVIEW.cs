using MessagePack;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_GET_MATCH_OVERVIEW
    {
        [Key(0)]
        public int FixtureId { get; set; }
    }

    [MessagePackObject]
    public class O_GET_MATCH_OVERVIEW
    {
        [Key(0)]
        public List<FootballFixtureDetail> HomeRecentFixtures { get; set; }

        [Key(1)]
        public List<FootballFixtureDetail> AwayRecentFixtures { get; set; }

        [Key(2)]
        public List<FootballFixtureDetail> League_HomeRecentFixtures { get; set; }

        [Key(3)]
        public List<FootballFixtureDetail> League_AwayRecentFixtures { get; set; }

        [Key(4)]
        public List<FootballStandingsDetail> StandingsDetails { get; set; }
    }
}