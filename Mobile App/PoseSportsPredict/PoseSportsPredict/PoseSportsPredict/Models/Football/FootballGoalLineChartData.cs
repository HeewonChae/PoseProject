using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballGoalLineChartData
    {
        public int FixtureId { get; set; }
        public int Score { get; set; }
        public DateTime MatchTime { get; set; }
        public string Category => MatchTime.ToString("ddd MM-dd");
    }
}