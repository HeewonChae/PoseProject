using Repository.Mysql.Dapper;
using Repository.Mysql.FootballDB.OutputModels;
using Repository.Mysql.FootballDB.Tables;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_SELECT_FIXTURES_DETAIL_BY_DATE : MysqlQuery<P_SELECT_FIXTURES_DETAIL_BY_DATE.Input, IEnumerable<DB_FootballFixtureDetail>>
    {
        public string queryString;

        public struct Input
        {
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
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
            var sb = new StringBuilder();
            sb.Append($"SELECT c.{nameof(Country.name)} as {nameof(DB_FootballFixtureDetail.CountryName)}, c.{nameof(Country.logo)} as {nameof(DB_FootballFixtureDetail.CountryLogo)}, ");
            sb.Append($"l.{nameof(League.name)} as {nameof(DB_FootballFixtureDetail.LeagueName)}, l.{nameof(League.logo)} as {nameof(DB_FootballFixtureDetail.LeagueLogo)}, ");
            sb.Append($"ht.{nameof(League.id)} as {nameof(DB_FootballFixtureDetail.HomeTeamId)}, ht.{nameof(Team.name)} as {nameof(DB_FootballFixtureDetail.HomeTeamName)}, ht.{nameof(Team.logo)} as {nameof(DB_FootballFixtureDetail.HomeTeamLogo)}, ");
            sb.Append($"at.{nameof(League.id)} as {nameof(DB_FootballFixtureDetail.AwayTeamId)}, at.{nameof(Team.name)} as {nameof(DB_FootballFixtureDetail.AwayTeamName)}, at.{nameof(Team.logo)} as {nameof(DB_FootballFixtureDetail.AwayTeamLogo)}, ");
            sb.Append($"f.{nameof(Fixture.home_score)} as {nameof(DB_FootballFixtureDetail.HomeTeamScore)}, f.{nameof(Fixture.away_score)} as {nameof(DB_FootballFixtureDetail.AwayTeamScore)}, ");
            sb.Append($"f.{nameof(Fixture.id)} as {nameof(DB_FootballFixtureDetail.FixtureId)}, f.{nameof(Fixture.status)} as {nameof(DB_FootballFixtureDetail.MatchStatus)}, ");
            sb.Append($"f.{nameof(Fixture.match_time)} as {nameof(DB_FootballFixtureDetail.MatchTime)}, l.{nameof(League.type)} as {nameof(DB_FootballFixtureDetail.LeagueType)} ");
            sb.Append("FROM fixture as f ");
            sb.Append($"INNER JOIN league as l on f.{nameof(Fixture.league_id)} = l.{nameof(League.id)} ");
            sb.Append($"INNER JOIN country as c on l.{nameof(League.country_name)} = c.{nameof(Country.name)} ");
            sb.Append($"INNER JOIN team as ht on f.{nameof(Fixture.home_team_id)} = ht.{nameof(Team.id)} ");
            sb.Append($"INNER JOIN team as at on f.{nameof(Fixture.away_team_id)} = at.{nameof(Team.id)} ");
            sb.Append($"WHERE f.{nameof(Fixture.match_time)} BETWEEN \"{_input.StartTime.ToString("yyyyMMddTHHmmss")}\" AND \"{_input.EndTime.ToString("yyyyMMddTHHmmss")}\" AND l.{nameof(League.is_predict_coverage)} = 1 ;");

            queryString = sb.ToString();
        }

        public override IEnumerable<DB_FootballFixtureDetail> OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output = footballDB.Query<DB_FootballFixtureDetail>(queryString);
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