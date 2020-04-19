using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballStandingsInfo
    {
        public string CountryName { get; set; }
        public string CountryLogo { get; set; }
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public short TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }

        public short Rank { get; set; }
        public short Points { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public short Win { get; set; }
        public short Draw { get; set; }
        public short Lose { get; set; }
        public short GoalFor { get; set; }
        public short GoalAgainst { get; set; }
        public short GoalDifference { get; set; }
        public List<char> Form { get; set; }
    }
}