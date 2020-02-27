using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RapidAPI.Models.Football.Odds.BookmakerInfo;

namespace RapidAPI.Converter
{
	public class BetInfoConverter : JsonConverter<BetInfo[]>
	{
		public override BetInfo[] ReadJson(JsonReader reader, Type objectType, BetInfo[] existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if(reader.TokenType == JsonToken.StartArray)
			{
				List<BetInfo> betInfos = new List<BetInfo>();

				JArray items = JArray.Load(reader);
				var tempBetInfos = items.ToObject<IEnumerable<BetInfo>>(serializer);

				foreach(var betInfo in tempBetInfos)
				{
					if(betInfo.LabelType != Models.Football.Enums.OddsLabelType._NONE_
						&& betInfo.LabelType != Models.Football.Enums.OddsLabelType._MAX_)
					{
						if(betInfo.LabelType == Models.Football.Enums.OddsLabelType.Goals_Over_Under)
						{
							var filteredValues = betInfo.BetValues.Where(elem =>
							{
								string str_floatValue = elem.Name.Split(' ')[1];
								var value = double.Parse(str_floatValue);

								return (value % 0.5) == 0 && 1.1 < elem.Odds && elem.Odds < 9.9f;
							});

							betInfo.BetValues = filteredValues.ToArray();
						}

						betInfos.Add(betInfo);
					}
				}

				return betInfos.ToArray();
			}

			return hasExistingValue ? existingValue : new BetInfo[0];
		}

		public override void WriteJson(JsonWriter writer, BetInfo[] value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}

