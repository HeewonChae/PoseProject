using Newtonsoft.Json;
using RapidAPI.Converter;
using RapidAPI.Models.Football.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football
{
    public class Fixture
    {
        public class LeagueInfo
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("logo")]
            public string Logo { get; set; }

            [JsonProperty("flag")]
            public string Flag { get; set; }
        }

        public class TeamInfo
        {
            [JsonProperty("team_id")]
            public int TeamId { get; set; }

            [JsonProperty("team_name")]
            public string TeamName { get; set; }

            [JsonProperty("logo")]
            public string Logo { get; set; }
        }

        public class ScoreInfo
        {
            [JsonProperty("halftime")]
            public string HalfTime { get; set; }

            [JsonProperty("fulltime")]
            public string FullTime { get; set; }

            [JsonProperty("extratime")]
            public string Extratime { get; set; }

            [JsonProperty("penalty")]
            public string Penalty { get; set; }
        }

        public class LineupInfo
        {
            public class LineupDetail
            {
                public class PlayerInfo
                {
                    [JsonProperty("team_id")]
                    public int TeamId { get; set; }

                    [JsonProperty("player_id")]
                    public int PlayerId { get; set; }
                }

                [JsonProperty("formation")]
                public string Formation { get; set; }

                [JsonProperty("startXI")]
                public PlayerInfo[] Players { get; set; }
            }

            public LineupDetail HomeTeam { get; set; }
            public LineupDetail AwayTeam { get; set; }
        }

        [JsonProperty("fixture_id")]
        public int FixtureId { get; set; }

        [JsonProperty("league_id")]
        public int LeagueId { get; set; }

        [JsonProperty("league")]
        public LeagueInfo League { get; set; }

        [JsonProperty("event_date")]
        public DateTime MatchTime { get; set; }

        [JsonProperty("event_timestamp")]
        public long? EventTimeStamp { get; set; }

        [JsonProperty("firstHalfStart")]
        public long? FirstHalfStart { get; set; }

        [JsonProperty("secondHalfStart")]
        public long? SecondHalfStart { get; set; }

        [JsonProperty("round")]
        public string Round { get; set; }

        [JsonProperty("statusShort")]
        [JsonConverter(typeof(FixtureStatusConverter))]
        public FixtureStatusType Status { get; set; }

        [JsonProperty("elapsed")]
        public int? Elapsed { get; set; }

        [JsonProperty("venue")]
        public string Venue { get; set; }

        [JsonProperty("homeTeam")]
        public TeamInfo HomeTeam { get; set; }

        [JsonProperty("awayTeam")]
        public TeamInfo AwayTeam { get; set; }

        [JsonProperty("goalsHomeTeam")]
        public int? GoalsHomeTeam { get; set; }

        [JsonProperty("goalsAwayTeam")]
        public int? GoalsAwayTeam { get; set; }

        [JsonProperty("statistics")]
        public FixtureStatistic Statistic { get; set; }

        [JsonProperty("score")]
        public ScoreInfo Score { get; set; }

        [JsonProperty("lineups")]
        public Dictionary<string, LineupInfo> Lineup { get; set; }
    }
}