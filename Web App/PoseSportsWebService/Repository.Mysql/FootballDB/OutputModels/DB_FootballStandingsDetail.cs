using Repository.Mysql.FootballDB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.OutputModels
{
    public class DB_FootballStandingsDetail
    {
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public string LeagueType { get; set; }
        public string League_CountryName { get; set; }
        public string League_CountryLogo { get; set; }

        public short TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public string Team_CountryName { get; set; }

        public short Rank { get; set; }
        public short Points { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public short Played { get; set; }
        public short Win { get; set; }
        public short Draw { get; set; }
        public short Lose { get; set; }
        public short GoalFor { get; set; }
        public short GoalAgainst { get; set; }
        public string Form { get; set; }

        #region Select Query

        private static string _selectQuery;

        public static string SelectQuery
        {
            get
            {
                if (string.IsNullOrEmpty(_selectQuery))
                {
                    var sb = new StringBuilder();
                    sb.Append($"SELECT l.{nameof(League.name)} as {nameof(LeagueName)}, l.{nameof(League.logo)} as {nameof(LeagueLogo)}, l.{nameof(League.type)} as {nameof(LeagueType)}, ");
                    sb.Append($"c.{nameof(Country.name)} as {nameof(League_CountryName)}, c.{nameof(Country.logo)} as {nameof(League_CountryLogo)}, ");
                    sb.Append($"t.{nameof(Team.id)} as {nameof(TeamId)}, t.{nameof(Team.name)} as {nameof(TeamName)}, t.{nameof(Team.logo)} as {nameof(TeamLogo)}, ");
                    sb.Append($"t.{nameof(Team.country_name)} as {nameof(Team_CountryName)}, ");
                    sb.Append($"s.{nameof(Standings.rank)} as '{nameof(Rank)}', s.{nameof(Standings.points)} as {nameof(Points)}, s.{nameof(Standings.group)} as '{nameof(Group)}', ");
                    sb.Append($"s.{nameof(Standings.description)} as '{nameof(Description)}', s.{nameof(Standings.played)} as {nameof(Played)}, s.{nameof(Standings.win)} as {nameof(Win)}, ");
                    sb.Append($"s.{nameof(Standings.draw)} as {nameof(Draw)}, s.{nameof(Standings.lose)} as {nameof(Lose)}, s.{nameof(Standings.goals_for)} as {nameof(GoalFor)}, ");
                    sb.Append($"s.{nameof(Standings.goals_against)} as {nameof(GoalAgainst)}, s.{nameof(Standings.forme)} as {nameof(Form)} ");
                    sb.Append("FROM standings as s ");
                    sb.Append($"INNER JOIN league as l on s.{nameof(Standings.league_id)} = l.{nameof(League.id)} ");
                    sb.Append($"INNER JOIN country as c on l.{nameof(League.country_name)} = c.{nameof(Country.name)} ");
                    sb.Append($"INNER JOIN team as t on s.{nameof(Standings.team_id)} = t.{nameof(Team.id)} ");

                    _selectQuery = sb.ToString();
                }

                return _selectQuery;
            }
        }

        #endregion Select Query
    }
}