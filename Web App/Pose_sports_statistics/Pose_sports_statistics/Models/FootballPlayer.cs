using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Models
{
    public class FootballPlayer
    {
        public class ShotInfo
        {
            public int Total { get; set; }
            public int On { get; set; }
        }

        public class GoalInfo
        {
            public int Total { get; set; }
            public int Conceded { get; set; }
            public int Assists { get; set; }
        }

        public class PassInfo
        {
            public int Total { get; set; }
            public int Key { get; set; }
            public int Accuracy { get; set; }
        }

        public class TackleInfo
        {
            public int Total { get; set; }
            public int Blocks { get; set; }
            public int Interceptions { get; set; }
        }

        public class DribbleInfo
        {
            public int Attempts { get; set; }
            public int Success { get; set; }
        }

        public class GameInfo
        {
            public int Appearences { get; set; }
            public int Minutes { get; set; }
            public int Lineups { get; set; }
        }

        public int PlayerId { get; set; }
        public string PalyerName { get; set; }
        public int? Number { get; set; }
        public string Position { get; set; }
        public int? Age { get; set; }
        public string Nationality { get; set; }
        public string Height { get; set; }
        public bool? Injured { get; set; }
        public float? Rating { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string League { get; set; }
        public string Season { get; set; }
        public int Captain { get; set; }
        public ShotInfo Shots { get; set; }
        public GoalInfo Goals { get; set; }
        public PassInfo Passes { get; set; }
        public TackleInfo Tackles { get; set; }
        public DribbleInfo Dribbles { get; set; }
        public GameInfo Games { get; set; }
    }
}