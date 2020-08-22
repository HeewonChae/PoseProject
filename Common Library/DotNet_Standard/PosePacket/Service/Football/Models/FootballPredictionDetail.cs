using MessagePack;
using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football.Models
{
    [MessagePackObject]
    public class FootballPredictionDetail
    {
        [Key(0)]
        public int FixtureId { get; set; }

        [Key(1)]
        public byte Seq { get; set; }

        [Key(2)]
        public FootballPredictionType MainLabel { get; set; }

        [Key(3)]
        public byte SubLabel { get; set; }

        [Key(4)]
        public int Value1 { get; set; }

        [Key(5)]
        public int Value2 { get; set; }

        [Key(6)]
        public int Value3 { get; set; }

        [Key(7)]
        public int Value4 { get; set; }

        [Key(8)]
        public byte Grade { get; set; }

        [Key(9)]
        public bool IsRecommended { get; set; }

        [Key(10)]
        public bool IsHit { get; set; }
    }
}