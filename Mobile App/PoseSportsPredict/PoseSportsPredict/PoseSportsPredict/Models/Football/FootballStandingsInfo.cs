using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Models.Football
{
    public class FootballStandingsInfo
    {
        public FootballLeagueInfo LeagueInfo { get; set; }
        public FootballTeamInfo TeamInfo { get; set; }

        public short Rank { get; set; }
        public Color RankColor { get; set; }
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
        public List<FootballFormInfo> Form { get; set; }
    }
}