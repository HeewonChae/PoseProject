using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI.Models.Football
{
    public class FootballTeamStat
    {
        [JsonProperty("avg_gf")]
        public double AvgGF { get; set; }

        [JsonProperty("avg_ga")]
        public double AvgGA { get; set; }

        [JsonProperty("att_trend")]
        public double AttTrend { get; set; }

        [JsonProperty("def_trend")]
        public double DefTrend { get; set; }

        [JsonProperty("rest_date")]
        public int RestDate { get; set; }
    }
}