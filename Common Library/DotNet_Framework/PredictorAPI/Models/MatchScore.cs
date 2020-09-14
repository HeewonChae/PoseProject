using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI.Models
{
    public class MatchScore
    {
        [JsonProperty("lin_home_socre")]
        public double Lin_HomeScore { get; set; }

        [JsonProperty("lin_away_score")]
        public double Lin_AwayScore { get; set; }

        [JsonProperty("knn_home_score")]
        public double Knn_HomeScore { get; set; }

        [JsonProperty("knn_away_score")]
        public double Knn_AwayScore { get; set; }
    }
}