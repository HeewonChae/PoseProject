using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI.Models.Football
{
    public class FootballPrediction
    {
        [JsonProperty("score")]
        public MatchScore MatchScore { get; set; }

        [JsonProperty("match_winner")]
        public FootballMatchWinner MatchWinner { get; set; }

        [JsonProperty("both_to_score")]
        public FootballBothToScore BothToScore { get; set; }

        [JsonProperty("under_over")]
        public FootballUnderOver UnderOver { get; set; }

        [JsonProperty("home_stat")]
        public FootballTeamStat HomeStat { get; set; }

        [JsonProperty("away_stat")]
        public FootballTeamStat AwayStat { get; set; }
    }
}