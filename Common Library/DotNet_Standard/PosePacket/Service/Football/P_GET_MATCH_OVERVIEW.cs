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
        public FootballFixtureDetail[] RecentFixtures { get; set; }

        [Key(1)]
        public FootballFixtureDetail[] League_RecentFixtures { get; set; }

        [Key(2)]
        public FootballStandingsDetail[] StandingsDetails { get; set; }
    }
}