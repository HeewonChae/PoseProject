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
	public class FixtureStatisticConverter : JsonConverter<FixtureStatistic>
	{
		public override FixtureStatistic ReadJson(JsonReader reader, Type objectType, FixtureStatistic existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartObject)
			{
				JObject item = JObject.Load(reader);
				var fixtureStatistic = item.ToObject<FixtureStatistic>(serializer);

				return fixtureStatistic;
			}

			return hasExistingValue ? existingValue : null;
		}

		public override void WriteJson(JsonWriter writer, FixtureStatistic value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
