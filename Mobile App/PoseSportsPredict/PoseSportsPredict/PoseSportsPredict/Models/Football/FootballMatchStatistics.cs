using PoseSportsPredict.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballMatchStatistics
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

        //Recent Record

        public RecordInfo HomeRecentRecord { get; set; }
        public RecordInfo AwayRecentRecord { get; set; }
        public double TotalRecordPoints { get; set; }

        // Goal For

        public BaseInfo HomeGoalFor { get; set; }
        public BaseInfo AwayGoalFor { get; set; }
        public double TotalGoalFor { get; set; }

        // Goal Against

        public BaseInfo HomeGoalAgainst { get; set; }
        public BaseInfo AwayGoalAgainst { get; set; }
        public double TotalGoalAgainst { get; set; }

        // H/A Goal For

        public BaseInfo HomeSideGoalFor { get; set; }
        public BaseInfo AwaySideGoalFor { get; set; }
        public double TotalSideGoalFor { get; set; }

        // H/A Goal Against

        public BaseInfo HomeSideGoalAgainst { get; set; }
        public BaseInfo AwaySideGoalAgainst { get; set; }
        public double TotalSideGoalAgainst { get; set; }

        // Rest Period

        public int HomeRestPeriod { get; set; }
        public int AwayRestPeriod { get; set; }
        public int TotalRestPeriod { get; set; }
    }
}