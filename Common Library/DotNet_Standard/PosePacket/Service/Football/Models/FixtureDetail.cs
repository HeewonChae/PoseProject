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
        public string MatchStatus { get; set; }
        public DateTime MatchTime { get; set; }
    }
}