using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football
{
    public class LeagueDetatil : Fixture.LeagueInfo
    {
        public class CoverageInfo
        {
            public class FixtureCoverageInfo
            {
                [JsonProperty("lineups")]
                public bool Lineups { get; set; }

                [JsonProperty("statistics")]
                public bool Statistics { get; set; }
            }

            [JsonProperty("standings")]
            public bool Standings { get; set; }

            [JsonProperty("fixtures")]
            public FixtureCoverageInfo FixtureCoverage { get; set; }

            [JsonProperty("players")]
            public bool Players { get; set; }

            [JsonProperty("topScorers")]
            public bool TopScorers { get; set; }

            [JsonProperty("predictions")]
            public bool Predictions { get; set; }

            [JsonProperty("odds")]
            public bool Odds { get; set; }
        }

        [JsonProperty("league_id")]
        public int LeagueId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("season_start")]
        public DateTime SeasonStart { get; set; }

        [JsonProperty("season_end")]
        public DateTime SeasonEnd { get; set; }

        [JsonProperty("is_current")]
        public int IsCurrent { get; set; }

        [JsonProperty("coverage")]
        public CoverageInfo Coverage { get; set; }
    }
}