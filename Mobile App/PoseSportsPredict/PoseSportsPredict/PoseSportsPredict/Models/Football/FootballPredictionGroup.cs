using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballPredictionGroup
    {
        public int MatchId { get; set; }
        public FootballPredictionType MainLabel { get; set; }
        public bool IsRecommand { get; set; }
        public double Rate { get; set; }
        public DateTime UnlockedTime { get; set; }
        public FootballPredictionInfo[] Predictions { get; set; }
    }
}