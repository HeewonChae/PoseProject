using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class ParticipatingLeagueInfo
    {
        public FootballLeagueInfo LeagueInfo { get; set; }
        public List<FootballFormInfo> RecentForm { get; set; }
    }
}