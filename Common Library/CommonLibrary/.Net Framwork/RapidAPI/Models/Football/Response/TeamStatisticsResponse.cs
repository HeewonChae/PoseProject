using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football.Response
{
	internal class TeamStatisticsResponse : ResponseBase
	{
		[JsonProperty("statistics")]
		public TeamStatistics TeamStatistics { get; set; }
	}
}
