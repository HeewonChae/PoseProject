using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Football.Models
{
    [MessagePackObject]
    public class FootballCountryDetail
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public string Logo { get; set; }
    }
}