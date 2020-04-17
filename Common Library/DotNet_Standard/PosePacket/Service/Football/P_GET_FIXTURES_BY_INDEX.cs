using MessagePack;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_GET_FIXTURES_BY_INDEX
    {
        [Key(0)]
        public int[] FixtureIds { get; set; }
    }

    [MessagePackObject]
    public class O_GET_FIXTURES_BY_INDEX
    {
        [Key(0)]
        public List<FootballFixtureDetail> Fixtures { get; set; }
    }
}