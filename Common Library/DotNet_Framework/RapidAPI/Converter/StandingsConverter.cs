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
    public class StandingsConverter : JsonConverter<Standings[]>
    {
        public override Standings[] ReadJson(JsonReader reader, Type objectType, Standings[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                List<Standings> standings = new List<Standings>();

                JObject item = JObject.Load(reader);

                Standings[] tempStandings = new Standings[0];
                JToken standingObject;
                for (int i = 0; i < 100; i++)
                {
                    string propertyName = $"{i}";
                    standingObject = item[propertyName];
                    if (standingObject != null)
                    {
                        tempStandings = standingObject.ToObject<Standings[]>(serializer);
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
            else if (reader.TokenType == JsonToken.StartArray)
            {
                List<Standings> standings = new List<Standings>();

                JArray items = JArray.Load(reader);
                var tempStandingsGroup = items.ToObject<IEnumerable<Standings[]>>(serializer);

                int group = 1;
                foreach (var tempStandings in tempStandingsGroup)
                {
                    foreach (var tempStanding in tempStandings)
                    {
                        tempStanding.Group = $"Group-{group} {tempStanding.Group}";
                        standings.Add(tempStanding);
                    }
                    group++;
                }

                return standings.ToArray();
            }

            return hasExistingValue ? existingValue : new Standings[0];
        }

        public override void WriteJson(JsonWriter writer, Standings[] value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}