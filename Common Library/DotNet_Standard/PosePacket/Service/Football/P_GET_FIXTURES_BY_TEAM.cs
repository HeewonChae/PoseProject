﻿using MessagePack;
using PosePacket.Service.Enum;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_GET_FIXTURES_BY_TEAM
    {
        [Key(0)]
        public SearchFixtureStatusType SearchFixtureStatusType { get; set; }

        [Key(1)]
        public short TeamId { get; set; }
    }

    [MessagePackObject]
    public class O_GET_FIXTURES_BY_TEAM
    {
        [Key(0)]
        public FootballFixtureDetail[] Fixtures { get; set; }
    }
}