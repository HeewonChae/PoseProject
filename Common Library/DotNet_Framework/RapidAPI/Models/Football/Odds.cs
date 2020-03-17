using Newtonsoft.Json;
using RapidAPI.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football
{
    public class Odds
    {
        public class FixtureInfoMini
        {
            [JsonProperty("league_id")]
            public int LeagueId { get; set; }

            [JsonProperty("fixture_id")]
            public int FixtureId { get; set; }

            [JsonProperty("updateAt")]
            public long UpdateAt { get; set; }
        }

        public class BookmakerInfo
        {
            public class BetInfo
            {
                public class BetValue
                {
                    [JsonProperty("value")]
                    public string Name;

                    [JsonProperty("odd")]
                    public float Odds;
                }

                [JsonProperty("label_id")]
                [JsonConverter(typeof(OddsLabelTypeConverter))]
                public Enums.OddsLabelType LabelType { get; set; }

                [JsonProperty("label_name")]
                public string LabelName { get; set; }

                [JsonProperty("values")]
                public BetValue[] BetValues { get; set; }
            }

            [JsonProperty("bookmaker_id")]
            [JsonConverter(typeof(BookMakerTypeConverter))]
            public Enums.BookmakerType BookmakerType { get; set; }

            [JsonProperty("bookmaker_name")]
            public string BookmakerName { get; set; }

            [JsonProperty("bets")]
            [JsonConverter(typeof(BetInfoConverter))]
            public BetInfo[] BetInfos { get; set; }
        }

        [JsonProperty("fixture")]
        public FixtureInfoMini FixtureMini { get; set; }

        [JsonProperty("bookmakers")]
        public BookmakerInfo[] Bookmakers { get; set; }
    }
}