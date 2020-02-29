using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football.Response
{
	internal class LeaguesResponse : ResponseBase
	{
		[JsonProperty("leagues")]
		public LeagueDetatil[] Leagues { get; set; }
	}
}
