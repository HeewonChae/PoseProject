using Newtonsoft.Json;
using RapidAPI.Models.Football;
using RapidAPI.Models.Football.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Converter
{
	public class FixtureStatusConverter : JsonConverter<FixtureStatusType>
	{
		public override FixtureStatusType ReadJson(JsonReader reader, Type objectType, FixtureStatusType existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null
				|| reader.Value == null)
			{
				return hasExistingValue ? existingValue : FixtureStatusType._NONE_;
			}

			string value = (string)reader.Value;
			value = value.ToUpper();

			if (value.Equals("1H"))
				return FixtureStatusType.FH;

			if (value.Equals("2H"))
				return FixtureStatusType.SH;


			return (FixtureStatusType)Enum.Parse(typeof(FixtureStatusType), value);
		}

		public override void WriteJson(JsonWriter writer, FixtureStatusType value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
