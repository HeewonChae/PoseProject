using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RapidAPI.Models.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Converter
{
	public class StandingsConverter : JsonConverter<Standing[]>
	{
		public override Standing[] ReadJson(JsonReader reader, Type objectType, Standing[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartObject)
			{
				List<Standing> standings = new List<Standing>();

				JObject item = JObject.Load(reader);

				Standing[] tempStandings = new Standing[0];
				JToken standingObject;
				for (int i = 0; i < 100; i++)
				{
					string propertyName = $"{i}";
					standingObject = item[propertyName];
					if(standingObject != null)
					{
						tempStandings = standingObject.ToObject<Standing[]>(serializer);
						break;
					}
				}

				foreach (var tempStanding in tempStandings)
				{
					tempStanding.Group = $"Group-{1} {tempStanding.Group}";
					standings.Add(tempStanding);
				}

				return standings.ToArray();
			}
			else if(reader.TokenType == JsonToken.StartArray)
			{
				List<Standing> standings = new List<Standing>();

				JArray items = JArray.Load(reader);
				var tempStandingsGroup = items.ToObject<IEnumerable<Standing[]>>(serializer);

				foreach(var tempStandings in tempStandingsGroup)
				{
					int group = 1;
					foreach(var tempStanding in tempStandings)
					{
						tempStanding.Group = $"Group-{group} {tempStanding.Group}";
						standings.Add(tempStanding);
					}
					group++;
				}

				return standings.ToArray();
			}

			return hasExistingValue ? existingValue : new Standing[0];
		}

		public override void WriteJson(JsonWriter writer, Standing[] value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
