using MessagePack;
using PosePacket.Service.Football.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football
{
    [MessagePackObject]
    public class I_GET_LEAGUE_OVERVIEW
    {
        [Key(0)]
        public string CountryName { get; set; }

        [Key(1)]
        public string LeagueName { get; set; }
    }

    [MessagePackObject]
    public class O_GET_LEAGUE_OVERVIEW
    {
        [Key(0)]
        public FootballLeagueDetail LeagueDetail { get; set; }

        [Key(1)]
        public FootballTeamDetail[] ParticipatingTeams { get; set; }

        [Key(2)]
        public FootballStandingsDetail[] StandingsDetails { get; set; }
    }
}