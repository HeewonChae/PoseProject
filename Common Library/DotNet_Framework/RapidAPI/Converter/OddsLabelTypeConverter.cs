using Newtonsoft.Json;
using RapidAPI.Models.Football.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Converter
{
    public class OddsLabelTypeConverter : JsonConverter<OddsLabelType>
    {
        public override OddsLabelType ReadJson(JsonReader reader, Type objectType, OddsLabelType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                long value = (Int64)reader.Value;

                if ((int)OddsLabelType.Match_Winner == value)
                {
                    return (OddsLabelType)value;
                }
                else
                    return OddsLabelType._NONE_;
            }

            return hasExistingValue ? existingValue : OddsLabelType._NONE_;
        }

        public override void WriteJson(JsonWriter writer, OddsLabelType value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}