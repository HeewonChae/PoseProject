using PosePacket.Service.Football.Models.Enums;
using System;
using MessagePack;

namespace PosePacket.Service.Football.Models
{
    [MessagePackObject]
    public class FootballFixtureDetail
    {
        [MessagePackObject]
        public class DataInfo
        {
            [Key(0)]
            public string Name { get; set; }

            [Key(1)]
            public string Logo { get; set; }
        }

        [MessagePackObject]
        public class TeamInfo
        {
            [Key(0)]
            public short Id { get; set; }

            [Key(1)]
            public string Name { get; set; }

            [Key(2)]
            public string Logo { get; set; }

            [Key(3)]
            public short Score { get; set; }
        }

        [Key(0)]
        public int FixtureId { get; set; }

        [Key(1)]
        public DataInfo Country { get; set; }

        [Key(2)]
        public DataInfo League { get; set; }

        [Key(3)]
        public TeamInfo HomeTeam { get; set; }

        [Key(4)]
        public TeamInfo AwayTeam { get; set; }

        [Key(5)]
        public string Round { get; set; }

        [Key(6)]
        public FootballMatchStatusType MatchStatus { get; set; }

        [Key(7)]
        public FootballLeagueType LeagueType { get; set; }

        [Key(8)]
        public DateTime MatchTime { get; set; }
    }
}