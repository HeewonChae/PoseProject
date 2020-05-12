using PoseSportsPredict.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballMatchStatistics
    {
        public FootballTeamStatistics HomeTeamStatistics { get; set; }
        public FootballTeamStatistics AwayTeamStatistics { get; set; }

        public double TotalRecordPoints => (HomeTeamStatistics.RecentRecord.Points + AwayTeamStatistics.RecentRecord.Points);
        public double TotalGoalFor => (HomeTeamStatistics.TotalGoalFor.Avg + AwayTeamStatistics.TotalGoalFor.Avg);
        public double TotalGoalAgainst => (HomeTeamStatistics.TotalGoalAgainst.Avg + AwayTeamStatistics.TotalGoalAgainst.Avg);
        public double TotalCampGoalFor => (HomeTeamStatistics.HomeGoalFor.Avg + AwayTeamStatistics.AwayGoalFor.Avg);
        public double TotalCampGoalAgainst => (HomeTeamStatistics.HoemGoalAgainst.Avg + AwayTeamStatistics.AwayGoalAgainst.Avg);

        // Rest Period

        public int HomeRestPeriod { get; set; }
        public int AwayRestPeriod { get; set; }
        public int TotalRestPeriod => (HomeRestPeriod + AwayRestPeriod);
    }
}