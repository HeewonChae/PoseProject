using Newtonsoft.Json;
using RapidAPI.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football.Response
{
	internal class FixtureStatisticResponse : ResponseBase
	{
		[JsonProperty("statistics")]
		[JsonConverter(typeof(FixtureStatisticConverter))]
		public FixtureStatistic FixtureStatistic { get; set; }
	}
}
