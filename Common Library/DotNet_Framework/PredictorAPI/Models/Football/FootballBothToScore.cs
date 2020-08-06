using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI.Models.Football
{
    public class FootballBothToScore
    {
        [JsonProperty("knn")]
        public ProbaYN KNN { get; set; }

        [JsonProperty("sgd")]
        public ProbaYN SGD { get; set; }

        [JsonProperty("sub")]
        public ProbaYN Sub { get; set; }
    }
}