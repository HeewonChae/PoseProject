using PosePacket.Service.Football.Models.Enums;
using System;
using MessagePack;

namespace PosePacket.Service.Football.Models
{
    [MessagePackObject]
    public class FootballFixtureDetail
    {
        [Key(0)]
        public int FixtureId { get; set; }

        [Key(1)]
        public FootballLeagueDetail League { get; set; }

        [Key(2)]
        public FootballTeamDetail HomeTeam { get; set; }

        [Key(3)]
        public FootballTeamDetail AwayTeam { get; set; }

        [Key(4)]
        public string Round { get; set; }

        [Key(5)]
        public DateTime MatchTime { get; set; }

        [Key(6)]
        public FootballMatchStatusType MatchStatus { get; set; }

        [Key(7)]
        public short HomeTeamScore { get; set; }

        [Key(8)]
        public short AwayTeamScore { get; set; }

        [Key(9)]
        public bool IsPredicted { get; set; }

        [Key(10)]
        public bool IsRecommended { get; set; }

        [Key(11)]
        public byte MaxGrade { get; set; }

        [Key(12)]
        public bool HasOdds { get; set; }
    }
}