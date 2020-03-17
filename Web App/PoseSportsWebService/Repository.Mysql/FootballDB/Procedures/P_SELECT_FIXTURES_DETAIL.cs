using Repository.Mysql.Dapper;
using Repository.Mysql.FootballDB.Tables;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_SELECT_FIXTURES_DETAIL : MysqlQuery<P_SELECT_FIXTURES_DETAIL.Input, IEnumerable<P_SELECT_FIXTURES_DETAIL.Output>>
    {
        public struct Input
        {
            public string WHERE { get; set; }
        }

        public class Output
        {
            public int FixtureId { get; set; }
            public string CountryName { get; set; }
            public string CountryLogo { get; set; }
            public string LeagueName { get; set; }
            public string LeagueLogo { get; set; }
            public string HomeTeamName { get; set; }
            public string HomeTeamLogo { get; set; }
            public short HomeTeamScore { get; set; }
            public string AwayTeamName { get; set; }
            public string AwayTeamLogo { get; set; }
            public short AwayTeamScore { get; set; }
            public string MatchStatus { get; set; }
            public string LeagueType { get; set; }
            public DateTime MatchTime { get; set; }
        }

        public override void OnAlloc()
        {
            base.OnAlloc();
        }

        public override void OnFree()
        {
            base.OnFree();
        }

        public override void BindParameters()
        {
            // if you need Binding Parameters, write here
        }

        public override IEnumerable<Output> OnQuery()
        {
            var sb = new StringBuilder();
            sb.Append($"SELECT c.{nameof(Country.name)} as {nameof(Output.CountryName)}, c.{nameof(Country.logo)} as {nameof(Output.CountryLogo)}, ");
            sb.Append($"l.{nameof(League.name)} as {nameof(Output.LeagueName)}, l.{nameof(League.logo)} as {nameof(Output.LeagueLogo)}, ");
            sb.Append($"ht.{nameof(Team.name)} as {nameof(Output.HomeTeamName)}, ht.{nameof(Team.logo)} as {nameof(Output.HomeTeamLogo)}, ");
            sb.Append($"at.{nameof(Team.name)} as {nameof(Output.AwayTeamName)}, at.{nameof(Team.logo)} as {nameof(Output.AwayTeamLogo)}, ");
            sb.Append($"f.{nameof(Fixture.home_score)} as {nameof(Output.HomeTeamScore)}, f.{nameof(Fixture.away_score)} as {nameof(Output.AwayTeamScore)}, ");
            sb.Append($"f.{nameof(Fixture.id)} as {nameof(Output.FixtureId)}, f.{nameof(Fixture.status)} as {nameof(Output.MatchStatus)}, ");
            sb.Append($"f.{nameof(Fixture.match_time)} as {nameof(Output.MatchTime)}, l.{nameof(League.type)} as {nameof(Output.LeagueType)} ");
            sb.Append("FROM fixture as f ");
            sb.Append($"INNER JOIN league as l on f.{nameof(Fixture.league_id)} = l.{nameof(League.id)} ");
            sb.Append($"INNER JOIN country as c on l.{nameof(League.country_name)} = c.{nameof(Country.name)} ");
            sb.Append($"INNER JOIN team as ht on f.{nameof(Fixture.home_team_id)} = ht.{nameof(Team.id)} ");
            sb.Append($"INNER JOIN team as at on f.{nameof(Fixture.away_team_id)} = at.{nameof(Team.id)} ");
            sb.Append($"WHERE {_input.WHERE};");

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output = footballDB.Query<Output>(sb.ToString());
                    },
                    this.OnError);

            return _output;
        }

        public override void OnError(EntityStatus entityStatus)
        {
            base.OnError(entityStatus);

            // Error Control
        }
    }
}