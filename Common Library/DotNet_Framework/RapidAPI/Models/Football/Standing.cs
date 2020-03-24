using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football
{
    public class Standings
    {
        public class MatchsPlayedInfo
        {
            [JsonProperty("matchsPlayed")]
            public int Played { get; set; }

            [JsonProperty("win")]
            public int Win { get; set; }

            [JsonProperty("draw")]
            public int Draw { get; set; }

            [JsonProperty("lose")]
            public int Lose { get; set; }

            [JsonProperty("goalsfor")]
            public int GoalsFor { get; set; }

            [JsonProperty("goalsAgainst")]
            public int GoalsAgainst { get; set; }
        }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("team_id")]
        public int? TeamId { get; set; }

        [JsonProperty("teamName")]
        public string TeamName { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("forme")]
        public string Forme { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("all")]
        public MatchsPlayedInfo AllPlayedInfo { get; set; }

        [JsonProperty("home")]
        public MatchsPlayedInfo HomePlayedInfo { get; set; }

        [JsonProperty("away")]
        public MatchsPlayedInfo AwayPlayedInfo { get; set; }

        [JsonProperty("goalsDiff")]
        public int GoalsDiff { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime LastUpdateTime { get; set; }
    }
}