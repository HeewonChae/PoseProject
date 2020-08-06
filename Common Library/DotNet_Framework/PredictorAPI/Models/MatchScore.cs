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
        [JsonProperty("home_socre")]
        public double HomeScore { get; set; }

        [JsonProperty("away_score")]
        public double AwayScore { get; set; }
    }
}