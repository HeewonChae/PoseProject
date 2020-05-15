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
            public int Played => Win + Draw + Lose;
            public int Win { get; set; }
            public int Draw { get; set; }
            public int Lose { get; set; }
            public int Points => (Win * 3) + (Draw * 1);
            public string Text => ToString();
            public string HomeText => $"{Win} {LocalizeString.Win_Initial}";
            public string DrawText => $"{Draw} {LocalizeString.Draw_Initial}";
            public string AwayText => $"{Lose} {LocalizeString.Win_Initial}";

            public override string ToString()
            {
                return $"{Win}{LocalizeString.Win_Initial} {Draw}{LocalizeString.Draw_Initial} {Lose}{LocalizeString.Lose_Initial}";
            }
        }

        public RecordInfo RecentRecord { get; set; }
        public BaseInfo TotalGoalFor { get; set; }
        public BaseInfo TotalGoalAgainst { get; set; }
        public BaseInfo HomeGoalFor { get; set; }
        public BaseInfo HoemGoalAgainst { get; set; }
        public BaseInfo AwayGoalFor { get; set; }
        public BaseInfo AwayGoalAgainst { get; set; }

        public double TotalGoalAvg => TotalGoalFor.Avg + TotalGoalAgainst.Avg;
        public double TotalHomeGoalAvg => HomeGoalFor.Avg + HoemGoalAgainst.Avg;
        public double TotalAwayGoalAvg => AwayGoalFor.Avg + AwayGoalAgainst.Avg;
    }
}