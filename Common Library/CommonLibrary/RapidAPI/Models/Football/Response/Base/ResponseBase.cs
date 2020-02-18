using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football.Response
{
	public class ResponseBase
	{
		[JsonProperty("results")]
		public int Results { get; set; }
	}
}
