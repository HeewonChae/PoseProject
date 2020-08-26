using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI.Models.Football
{
    public class FootballUnderOver
    {
        [JsonProperty("over_1_5")]
        public UnderOverProba UO_1_5 { get; set; }

        [JsonProperty("over_2_5")]
        public UnderOverProba UO_2_5 { get; set; }

        [JsonProperty("over_3_5")]
        public UnderOverProba UO_3_5 { get; set; }

        [JsonProperty("over_4_5")]
        public UnderOverProba UO_4_5 { get; set; }
    }

    public class UnderOverProba
    {
        [Obsolete]
        [JsonProperty("knn")]
        public ProbaYN KNN { get; set; }

        [JsonProperty("sgd")]
        public ProbaYN SGD { get; set; }

        [JsonProperty("sub")]
        public ProbaYN Sub { get; set; }
    }
}