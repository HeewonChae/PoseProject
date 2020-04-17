using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace PosePacket.Service.Football.Models
{
    [MessagePackObject]
    public class FootballStandingsDetail
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
        }

        [Key(0)]
        public DataInfo Country { get; set; }

        [Key(1)]
        public DataInfo League { get; set; }

        [Key(2)]
        public FootballLeagueType LeagueType { get; set; }

        [Key(3)]
        public TeamInfo Team { get; set; }

        [Key(4)]
        public short Rank { get; set; }

        [Key(5)]
        public short Points { get; set; }

        [Key(6)]
        public string Group { get; set; }

        [Key(7)]
        public string Description { get; set; }

        [Key(8)]
        public short Played { get; set; }

        [Key(9)]
        public short Win { get; set; }

        [Key(10)]
        public short Draw { get; set; }

        [Key(11)]
        public short Lose { get; set; }

        [Key(12)]
        public short GoalFor { get; set; }

        [Key(13)]
        public short GoalAgainst { get; set; }

        [Key(14)]
        public short GoalDifference { get; set; }

        [Key(15)]
        public List<char> Form { get; set; }
    }
}