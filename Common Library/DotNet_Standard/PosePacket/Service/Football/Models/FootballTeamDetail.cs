using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football.Models
{
    [MessagePackObject]
    public class FootballTeamDetail
    {
        [Key(0)]
        public short Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public string Logo { get; set; }

        [Key(3)]
        public string CountryName { get; set; }
    }
}