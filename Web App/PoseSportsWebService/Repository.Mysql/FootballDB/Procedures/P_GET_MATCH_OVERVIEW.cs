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
    public class P_GET_MATCH_OVERVIEW : MysqlQuery<P_GET_MATCH_OVERVIEW.Input, P_GET_MATCH_OVERVIEW.Output>
    {
        public string FixturesQueryString;
        public string LeagueFixturesQueryString;

        public struct Input
        {
            public int FixtureId { get; set; }
        }

        public class Output
        {
            public IEnumerable<DB_FootballFixtureDetail> HomeLastFixtures { get; set; }
            public IEnumerable<DB_FootballFixtureDetail> AwayLastFixtures { get; set; }

            public IEnumerable<DB_FootballFixtureDetail> League_HomeLastFixtures { get; set; }
            public IEnumerable<DB_FootballFixtureDetail> League_AwayLastFixtures { get; set; }

            public int Result { get; set; }
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
            sb.Append($"l.{nameof(League.name)} as {nameof(DB_FootballFixtureDetail.LeagueName)}, l.{nameof(League.logo)} as {nameof(DB_FootballFixtureDetail.LeagueLogo)}, f.{nameof(Fixture.round)} as {nameof(DB_FootballFixtureDetail.Round)},");
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

            FixturesQueryString = $"{sb} WHERE (f.{nameof(Fixture.home_team_id)} = @TeamId OR f.{nameof(Fixture.away_team_id)} = @TeamId) AND f.{nameof(Fixture.match_time)} < @MatchTime ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 6;";
            LeagueFixturesQueryString = $"{sb} WHERE f.{nameof(Fixture.league_id)} = @LeagueId AND (f.{nameof(Fixture.home_team_id)} = @TeamId OR f.{nameof(Fixture.away_team_id)} = @TeamId) AND f.{nameof(Fixture.match_time)} < @MatchTime ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 15;";
        }

        public override P_GET_MATCH_OVERVIEW.Output OnQuery()
        {
            _output = new Output();

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        var fixture = footballDB.Query<Fixture>($"SELECT * FROM fixture WHERE id={_input.FixtureId}").FirstOrDefault();
                        if (fixture == null)
                        {
                            _output.Result = 1;
                            return;
                        }

                        _output.HomeLastFixtures = footballDB.Query<DB_FootballFixtureDetail>(FixturesQueryString, new { TeamId = fixture.home_team_id, MatchTime = fixture.match_time });
                        _output.AwayLastFixtures = footballDB.Query<DB_FootballFixtureDetail>(FixturesQueryString, new { TeamId = fixture.away_team_id, MatchTime = fixture.match_time });

                        _output.League_HomeLastFixtures = footballDB.Query<DB_FootballFixtureDetail>(LeagueFixturesQueryString, new { LeagueId = fixture.league_id, TeamId = fixture.home_team_id, MatchTime = fixture.match_time });
                        _output.League_AwayLastFixtures = footballDB.Query<DB_FootballFixtureDetail>(LeagueFixturesQueryString, new { LeagueId = fixture.league_id, TeamId = fixture.away_team_id, MatchTime = fixture.match_time });

                        _output.Result = 0;
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