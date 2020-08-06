using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI.Models
{
    public class ProbaWinner
    {
        [JsonProperty("win_proba")]
        public double WinProba { get; set; }

        [JsonProperty("draw_proba")]
        public double DrawProba { get; set; }

        [JsonProperty("lose_proba")]
        public double LoseProba { get; set; }
    }
}