using MessagePack;
using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football.Models
{
    [MessagePackObject]
    public class FootballOddsDetail
    {
        [Key(0)]
        public int FixtureId { get; set; }

        [Key(1)]
        public FootballBookmakerType BoomakerType { get; set; }

        [Key(2)]
        public FootballOddsLabelType OddsLabelType { get; set; }

        [Key(3)]
        public float Odds1 { get; set; }

        [Key(4)]
        public float Odds2 { get; set; }

        [Key(5)]
        public float Odds3 { get; set; }
    }
}