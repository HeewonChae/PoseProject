using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football.Response
{
	internal class PredictionResponse : ResponseBase
	{
		[JsonProperty("predictions")]
		public Prediction[] Predictions { get; set; }
	}
}
