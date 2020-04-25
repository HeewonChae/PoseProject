using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Utilities.JsonConverter
{
    public class JsonColorConverter : JsonConverter<Color[]>
    {
        public override Color[] ReadJson(JsonReader reader, Type objectType, Color[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            var hexStrings = array.ToObject<List<string>>();

            var returnValue = new List<Color>();
            foreach (var hexString in hexStrings)
            {
                returnValue.Add(Color.FromHex(hexString));
            }

            return returnValue.ToArray();
        }

        public override void WriteJson(JsonWriter writer, Color[] value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}