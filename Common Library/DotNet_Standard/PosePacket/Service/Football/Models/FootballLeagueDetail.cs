using MessagePack;
using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football.Models
{
    [MessagePackObject]
    public class FootballLeagueDetail
    {
        [Key(0)]
        public FootballCountryDetail Country { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public string Logo { get; set; }

        [Key(3)]
        public FootballLeagueType LeagueType { get; set; }

        [Key(4)]
        public DateTime SeasonStartDate { get; set; }

        [Key(5)]
        public DateTime SeasonEndDate { get; set; }
    }
}