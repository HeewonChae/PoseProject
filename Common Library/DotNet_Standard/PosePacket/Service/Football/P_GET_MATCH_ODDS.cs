﻿using MessagePack;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_GET_MATCH_ODDS
    {
        [Key(0)]
        public int FixtureId { get; set; }
    }

    [MessagePackObject]
    public class O_GET_MATCH_ODDS
    {
        [Key(0)]
        public FootballOddsDetail[] OddsDetails { get; set; }
    }
}