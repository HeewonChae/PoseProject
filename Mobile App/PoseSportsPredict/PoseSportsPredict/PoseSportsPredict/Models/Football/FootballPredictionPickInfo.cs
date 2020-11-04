using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballPredictionPickInfo
    {
        public FootballPredictionType MainLabel { get; set; }
        public byte SubLabel { get; set; }
        public string Title { get; set; }
        public bool IsRecommend { get; set; }
        public double Rate { get; set; }
        public bool IsHit { get; set; }
    }
}