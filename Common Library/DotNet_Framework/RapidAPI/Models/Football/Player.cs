using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidAPI.Models.Football
{
    public class Player
    {
        public class ShotInfo
        {
            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("on")]
            public int On { get; set; }
        }

        public class GoalInfo
        {
            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("conceded")]
            public int Conceded { get; set; }

            [JsonProperty("assists")]
            public int Assists { get; set; }
        }

        public class PassInfo
        {
            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("key")]
            public int Key { get; set; }

            [JsonProperty("accuracy")]
            public int Accuracy { get; set; }
        }

        public class TackleInfo
        {
            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("blocks")]
            public int Blocks { get; set; }

            [JsonProperty("interceptions")]
            public int Interceptions { get; set; }
        }

        public class DribbleInfo
        {
            [JsonProperty("attempts")]
            public int Attempts { get; set; }

            [JsonProperty("success")]
            public int Success { get; set; }
        }

        public class GameInfo
        {
            [JsonProperty("appearences")]
            public int Appearences { get; set; }

            [JsonProperty("minutes_played")]
            public int Minutes { get; set; }

            [JsonProperty("lineups")]
            public int Lineups { get; set; }
        }

        [JsonProperty("player_id")]
        public int PlayerId { get; set; }

        [JsonProperty("lastname")]
        public string PalyerName { get; set; }

        [JsonProperty("number")]
        public int? Number { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("injured")]
        public bool? Injured { get; set; }

        [JsonProperty("rating")]
        public float? Rating { get; set; }

        [JsonProperty("team_id")]
        public int TeamId { get; set; }

        [JsonProperty("team_name")]
        public string TeamName { get; set; }

        [JsonProperty("league")]
        public string League { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("captain")]
        public int Captain { get; set; }

        [JsonProperty("shots")]
        public ShotInfo Shots { get; set; }

        [JsonProperty("goals")]
        public GoalInfo Goals { get; set; }

        [JsonProperty("passes")]
        public PassInfo Passes { get; set; }

        [JsonProperty("tackles")]
        public TackleInfo Tackles { get; set; }

        [JsonProperty("dribbles")]
        public DribbleInfo Dribbles { get; set; }

        [JsonProperty("games")]
        public GameInfo Games { get; set; }
    }
}