using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.OutputModels
{
    public class DB_FootballFixtureDetail
    {
        public int FixtureId { get; set; }
        public string CountryName { get; set; }
        public string CountryLogo { get; set; }
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public string Round { get; set; }
        public short HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public short HomeTeamScore { get; set; }
        public short AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamLogo { get; set; }
        public short AwayTeamScore { get; set; }
        public string MatchStatus { get; set; }
        public string LeagueType { get; set; }
        public DateTime MatchTime { get; set; }
    }
}