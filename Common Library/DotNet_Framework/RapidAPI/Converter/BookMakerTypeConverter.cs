using LogicCore.Converter;
using Newtonsoft.Json;
using RapidAPI.Models.Football.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Converter
{
    public class BookMakerTypeConverter : JsonConverter<BookmakerType>
    {
        public override BookmakerType ReadJson(JsonReader reader, Type objectType, BookmakerType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                long value = (Int64)reader.Value;

                ((int)value).TryParseEnum(out BookmakerType retValue);

                //if ((int)BookmakerType._NONE_ >= value
                //    || value >= (int)BookmakerType._MAX_)
                //{
                //    return BookmakerType._NONE_;
                //}

                return retValue;
            }

            return hasExistingValue ? existingValue : BookmakerType._NONE_;
        }

        public override void WriteJson(JsonWriter writer, BookmakerType value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}