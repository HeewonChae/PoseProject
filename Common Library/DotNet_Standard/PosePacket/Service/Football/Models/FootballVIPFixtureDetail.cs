using MessagePack;
using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football.Models
{
    [MessagePackObject]
    public class FootballVIPFixtureDetail : FootballFixtureDetail
    {
        [Key(13)]
        public FootballPredictionType MainLabel { get; set; }

        [Key(14)]
        public byte SubLabel { get; set; }

        [Key(15)]
        public byte Grade { get; set; }

        [Key(16)]
        public bool IsHit { get; set; }
    }
}