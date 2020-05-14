using MessagePack;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_GET_TEAM_OVERVIEW
    {
        [Key(0)]
        public short TeamId { get; set; }
    }

    [MessagePackObject]
    public class O_GET_TEAM_OVERVIEW
    {
        [Key(0)]
        public FootballFixtureDetail[] FixtureDetails { get; set; }
    }
}