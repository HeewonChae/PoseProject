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
        [Key(0)]
        public FootballLeagueDetail League { get; set; }

        [Key(1)]
        public FootballTeamDetail Team { get; set; }

        [Key(2)]
        public short Rank { get; set; }

        [Key(3)]
        public short Points { get; set; }

        [Key(4)]
        public string Group { get; set; }

        [Key(5)]
        public string Description { get; set; }

        [Key(6)]
        public short Played { get; set; }

        [Key(7)]
        public short Win { get; set; }

        [Key(8)]
        public short Draw { get; set; }

        [Key(9)]
        public short Lose { get; set; }

        [Key(10)]
        public short GoalFor { get; set; }

        [Key(11)]
        public short GoalAgainst { get; set; }

        [Key(12)]
        public short GoalDifference { get; set; }

        [Key(13)]
        public List<char> Form { get; set; }
    }
}