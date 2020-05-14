using MessagePack;
using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_GET_FIXTURES_BY_LEAGUE
    {
        [Key(0)]
        public SearchFixtureStatusType SearchFixtureStatusType { get; set; }

        [Key(1)]
        public string CountryName { get; set; }

        [Key(2)]
        public string LeagueName { get; set; }
    }

    [MessagePackObject]
    public class O_GET_FIXTURES_BY_LEAGUE
    {
        [Key(0)]
        public FootballFixtureDetail[] Fixtures { get; set; }
    }
}