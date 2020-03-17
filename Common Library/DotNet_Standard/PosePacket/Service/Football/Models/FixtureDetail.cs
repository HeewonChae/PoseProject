using PosePacket.Service.Football.Models.Enums;
using System;

namespace PosePacket.Service.Football.Models
{
    public class FixtureDetail
    {
        public class DataInfo
        {
            public string Name { get; set; }
            public string Logo { get; set; }
        }

        public class TeamInfo
        {
            public string Name { get; set; }
            public string Logo { get; set; }
            public short Score { get; set; }
        }

        public int FixtureId { get; set; }
        public DataInfo Country { get; set; }
        public DataInfo League { get; set; }
        public TeamInfo HomeTeam { get; set; }
        public TeamInfo AwayTeam { get; set; }
        public FootballFixtureStatusType MatchStatus { get; set; }
        public FootballLeagueType LeagueType { get; set; }
        public DateTime MatchTime { get; set; }
    }
}