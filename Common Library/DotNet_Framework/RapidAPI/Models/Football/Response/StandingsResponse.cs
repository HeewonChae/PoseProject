using Newtonsoft.Json;
using RapidAPI.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football.Response
{
    internal class StandingsResponse : ResponseBase
    {
        [JsonProperty("standings")]
        [JsonConverter(typeof(StandingsConverter))]
        public Standings[] Standingsies { get; set; }
    }
}