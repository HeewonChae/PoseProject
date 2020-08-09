using PosePacket.Service.Enum;
using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballFormInfo
    {
        public int FixtureId { get; set; }
        public string LeagueName { get; set; }
        public string OpposingTeamLogo { get; set; }
        public string OpposingTeamName { get; set; }
        public bool IsHomeMatch { get; set; }
        public short HomeScore { get; set; }
        public short AwayScore { get; set; }
        public MatchResultType Result { get; set; }
        public bool IsLastMatch { get; set; } = false;
    }
}