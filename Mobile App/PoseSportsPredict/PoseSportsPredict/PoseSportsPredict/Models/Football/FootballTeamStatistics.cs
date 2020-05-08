using PoseSportsPredict.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballTeamStatistics
    {
        public class BaseInfo
        {
            public int Value { get; set; }
            public double Avg { get; set; }
            public string Text => ToString();

            public override string ToString()
            {
                return $"{Avg} ({Value})";
            }
        }

        public class RecordInfo
        {
            public int Win { get; set; }
            public int Darw { get; set; }
            public int Lose { get; set; }
            public int Points => (Win * 3) + (Darw * 1);
            public string Text => ToString();

            public override string ToString()
            {
                return $"{Win}{LocalizeString.Win_Initial} {Darw}{LocalizeString.Draw_Initial} {Lose}{LocalizeString.Lose_Initial}";
            }
        }

        public RecordInfo RecentRecord { get; set; }
        public BaseInfo TotalGoalFor { get; set; }
        public BaseInfo TotalAgainst { get; set; }
        public BaseInfo HomeGoalFor { get; set; }
        public BaseInfo HoemGoalAgainst { get; set; }
        public BaseInfo AwayGoalFor { get; set; }
        public BaseInfo AwayGoalAgainst { get; set; }

        public double TotalGoal => (TotalGoalFor?.Value ?? 0 + TotalAgainst?.Value ?? 0);
        public double TotalHomeGoal => (HomeGoalFor?.Value ?? 0 + HoemGoalAgainst?.Value ?? 0);
        public double TotalAwayGoal => (AwayGoalFor?.Value ?? 0 + AwayGoalAgainst?.Value ?? 0);
    }
}