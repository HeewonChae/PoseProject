using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pose_sports_statistics.Models
{
    public class FootballStandings : FootballFixture.TeamInfo
    {
        public class MatchsPlayedInfo
        {
            public int Played { get; set; }

            public int Win { get; set; }

            public int Draw { get; set; }

            public int Lose { get; set; }

            public int GoalsFor { get; set; }

            public int GoalsAgainst { get; set; }
        }

        public int Rank { get; set; }
        public string Forme { get; set; }
        public MatchsPlayedInfo AllPlayedInfo { get; set; }
        public MatchsPlayedInfo HomePlayedInfo { get; set; }
        public MatchsPlayedInfo AwayPlayedInfo { get; set; }
        public int GoalsDiff { get; set; }
        public int Points { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}