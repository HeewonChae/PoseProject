using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football.Response
{
	internal class OddsResponse : ResponseBase
	{
		[JsonProperty("odds")]
		public Odds[] OddsList { get; set; }
	}
}
