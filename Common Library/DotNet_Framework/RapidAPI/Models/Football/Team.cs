using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football
{
    public class Team
    {
        [JsonProperty("team_id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("is_national")]
        public bool IsNational { get; set; }

        [JsonProperty("country")]
        public string CountryName { get; set; }

        [JsonProperty("founded")]
        public int? Founded { get; set; }

        [JsonProperty("venue_name")]
        public string VenueName { get; set; }

        [JsonProperty("venue_address")]
        public string VenueAddr { get; set; }

        [JsonProperty("venue_city")]
        public string VenueCity { get; set; }
    }
}