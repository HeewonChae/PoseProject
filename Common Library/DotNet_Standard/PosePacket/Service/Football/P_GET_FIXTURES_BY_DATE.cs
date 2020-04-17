using MessagePack;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_GET_FIXTURES_BY_DATE
    {
        [Key(0)]
        public DateTime StartTime { get; set; }

        [Key(1)]
        public DateTime EndTime { get; set; }
    }

    [MessagePackObject]
    public class O_GET_FIXTURES_BY_DATE
    {
        [Key(0)]
        public List<FootballFixtureDetail> Fixtures { get; set; }
    }
}