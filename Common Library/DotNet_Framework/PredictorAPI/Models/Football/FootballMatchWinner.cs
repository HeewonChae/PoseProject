using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI.Models.Football
{
    public class FootballMatchWinner
    {
        [JsonProperty("knn")]
        public ProbaWinner KNN { get; set; }

        [JsonProperty("sgd")]
        public ProbaWinner SGD { get; set; }

        [JsonProperty("sub")]
        public ProbaWinner Sub { get; set; }
    }
}