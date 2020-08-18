using Repository.Mysql.FootballDB.Tables;
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

        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public string League_CountryName { get; set; }
        public string League_CountryLogo { get; set; }

        public string Round { get; set; }
        public string LeagueType { get; set; }
        public string MatchStatus { get; set; }
        public DateTime MatchTime { get; set; }

        public short HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string HomeTeam_CountryName { get; set; }

        public short AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamLogo { get; set; }
        public string AwayTeam_CountryName { get; set; }

        public short HomeTeamScore { get; set; }
        public short AwayTeamScore { get; set; }

        public bool IsPredicted { get; set; }
        public bool IsRecommended { get; set; }

        #region Select Query

        private static string _selectQuery;

        public static string SelectQuery
        {
            get
            {
                if (string.IsNullOrEmpty(_selectQuery))
                {
                    var sb = new StringBuilder();
                    sb.Append($"SELECT c.{nameof(Country.name)} as {nameof(League_CountryName)}, c.{nameof(Country.logo)} as {nameof(League_CountryLogo)}, ");
                    sb.Append($"l.{nameof(League.name)} as {nameof(LeagueName)}, l.{nameof(League.logo)} as {nameof(LeagueLogo)}, f.{nameof(Fixture.round)} as {nameof(Round)},");
                    sb.Append($"ht.{nameof(League.id)} as {nameof(HomeTeamId)}, ht.{nameof(Team.name)} as {nameof(HomeTeamName)}, ht.{nameof(Team.logo)} as {nameof(HomeTeamLogo)}, ");
                    sb.Append($"at.{nameof(League.id)} as {nameof(AwayTeamId)}, at.{nameof(Team.name)} as {nameof(AwayTeamName)}, at.{nameof(Team.logo)} as {nameof(AwayTeamLogo)}, ");
                    sb.Append($"ht.{nameof(Team.country_name)} as {nameof(HomeTeam_CountryName)}, at.{nameof(Team.country_name)} as {nameof(AwayTeam_CountryName)}, ");
                    sb.Append($"f.{nameof(Fixture.home_score)} as {nameof(HomeTeamScore)}, f.{nameof(Fixture.away_score)} as {nameof(AwayTeamScore)}, ");
                    sb.Append($"f.{nameof(Fixture.id)} as {nameof(FixtureId)}, f.{nameof(Fixture.status)} as {nameof(MatchStatus)}, ");
                    sb.Append($"f.{nameof(Fixture.match_time)} as {nameof(MatchTime)}, l.{nameof(League.type)} as {nameof(LeagueType)}, ");
                    sb.Append($"f.{nameof(Fixture.is_predicted)} as {nameof(IsPredicted)}, f.{nameof(Fixture.is_recommended)} as {nameof(IsRecommended)} ");
                    sb.Append("FROM fixture as f ");
                    sb.Append($"INNER JOIN league as l on f.{nameof(Fixture.league_id)} = l.{nameof(League.id)} ");
                    sb.Append($"INNER JOIN country as c on l.{nameof(League.country_name)} = c.{nameof(Country.name)} ");
                    sb.Append($"INNER JOIN team as ht on f.{nameof(Fixture.home_team_id)} = ht.{nameof(Team.id)} ");
                    sb.Append($"INNER JOIN team as at on f.{nameof(Fixture.away_team_id)} = at.{nameof(Team.id)} ");

                    _selectQuery = sb.ToString();
                }

                return _selectQuery;
            }
        }

        #endregion Select Query
    }
}