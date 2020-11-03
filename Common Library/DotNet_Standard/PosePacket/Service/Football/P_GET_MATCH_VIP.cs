using MessagePack;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class O_GET_MATCH_VIP
    {
        [Key(0)]
        public FootballVIPFixtureDetail[] VIPFixtureDetails { get; set; }
    }
}