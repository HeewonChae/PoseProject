using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.OutputModels
{
    using Repository.Mysql.FootballDB.Tables;

    public class DB_FootballVIPFixtureDetail : DB_FootballFixtureDetail
    {
        public short MainLabel { get; set; }

        public short SubLabel { get; set; }

        public short Grade { get; set; }

        public bool IsHit { get; set; }

        private static string _selectQuery;

        public static new string SelectQuery
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
                    sb.Append($"f.{nameof(Fixture.is_predicted)} as {nameof(IsPredicted)}, f.{nameof(Fixture.is_recommended)} as {nameof(IsRecommended)}, ");
                    sb.Append($"f.{nameof(Fixture.has_odds)} as {nameof(HasOdds)}, f.{nameof(Fixture.max_grade)} as {nameof(MaxGrade)}, ");
                    sb.Append($"p.{nameof(Prediction.main_label)} as {nameof(MainLabel)}, p.{nameof(Prediction.sub_label)} as {nameof(SubLabel)}, ");
                    sb.Append($"p.{nameof(Prediction.grade)} as {nameof(Grade)}, p.{nameof(Prediction.is_hit)} as {nameof(IsHit)} ");
                    sb.Append("FROM fixture as f ");
                    sb.Append($"INNER JOIN league as l on f.{nameof(Fixture.league_id)} = l.{nameof(League.id)} ");
                    sb.Append($"INNER JOIN country as c on l.{nameof(League.country_name)} = c.{nameof(Country.name)} ");
                    sb.Append($"INNER JOIN team as ht on f.{nameof(Fixture.home_team_id)} = ht.{nameof(Team.id)} ");
                    sb.Append($"INNER JOIN team as at on f.{nameof(Fixture.away_team_id)} = at.{nameof(Team.id)} ");
                    sb.Append($"RIGHT OUTER JOIN prediction as p on f.{nameof(Fixture.id)} = p.{nameof(Prediction.fixture_id)} ");

                    _selectQuery = sb.ToString();
                }

                return _selectQuery;
            }
        }
    }
}