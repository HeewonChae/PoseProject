using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football.Response
{
	internal class StandingsResponse : ResponseBase
	{
		[JsonProperty("standings")]
		public Standing[][] Standings { get; set; }
	}
}
