using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football.Models
{
    public class FootballStandingsDetail
    {
        public class DataInfo
        {
            public string Name { get; set; }
            public string Logo { get; set; }
        }

        public class TeamInfo
        {
            public short Id { get; set; }
            public string Name { get; set; }
            public string Logo { get; set; }
        }

        public DataInfo Country { get; set; }
        public DataInfo League { get; set; }
        public FootballLeagueType LeagueType { get; set; }

        public TeamInfo Team { get; set; }
        public short Rank { get; set; }
        public short Points { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public short Played { get; set; }
        public short Win { get; set; }
        public short Draw { get; set; }
        public short Lose { get; set; }
        public short GoalFor { get; set; }
        public short GoalAgainst { get; set; }
        public short GoalDifference { get; set; }
        public List<char> Form { get; set; }
    }
}