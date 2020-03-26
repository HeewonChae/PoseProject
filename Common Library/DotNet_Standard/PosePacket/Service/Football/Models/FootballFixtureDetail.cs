using PosePacket.Service.Football.Models.Enums;
using System;

namespace PosePacket.Service.Football.Models
{
    public class FootballFixtureDetail
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
            public short Score { get; set; }
        }

        public int FixtureId { get; set; }
        public DataInfo Country { get; set; }
        public DataInfo League { get; set; }
        public TeamInfo HomeTeam { get; set; }
        public TeamInfo AwayTeam { get; set; }
        public FootballMatchStatusType MatchStatus { get; set; }
        public FootballLeagueType LeagueType { get; set; }
        public DateTime MatchTime { get; set; }
    }
}