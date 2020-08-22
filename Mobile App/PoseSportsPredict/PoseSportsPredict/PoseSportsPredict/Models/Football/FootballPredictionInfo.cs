using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballPredictionInfo
    {
        public FootballPredictionType MainLabel { get; set; }
        public byte SubLabel { get; set; }
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public int Value3 { get; set; }
        public int Value4 { get; set; }
        public byte Grade { get; set; }
        public bool IsRecommended { get; set; }
        public bool IsHit { get; set; }
    }
}