using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballVIPMatchInfo : FootballMatchInfo
    {
        public List<FootballPredictionInfo> Predictions { get; set; }
    }
}