using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorAPI.Models
{
    public class ProbaYN
    {
        [JsonProperty("yes")]
        public double YES { get; set; }

        [JsonProperty("no")]
        public double NO { get; set; }
    }
}