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
        public string StandingsQueryString;

        public struct Input
        {
            public int FixtureId { get; set; }
        }

        public class Output
        {
            public IEnumerable<DB_FootballFixtureDetail> HomeRecntFixtures { get; set; }
            public IEnumerable<DB_FootballFixtureDetail> AwayRecentFixtures { get; set; }

            public IEnumerable<DB_FootballFixtureDetail> League_HomeRecentFixtures { get; set; }
            public IEnumerable<DB_FootballFixtureDetail> League_AwayRecentFixtures { get; set; }

            public IEnumerable<DB_FootballStandingsDetail> StandingsDetails { get; set; }

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
            FixturesQueryString = $"{DB_FootballFixtureDetail.SelectQuery} " +
                $"WHERE (f.{nameof(Fixture.home_team_id)} = @TeamId OR f.{nameof(Fixture.away_team_id)} = @TeamId) AND f.{nameof(Fixture.match_time)} < @MatchTime " +
                $"ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 6;";
            LeagueFixturesQueryString = $"{DB_FootballFixtureDetail.SelectQuery} " +
                $"WHERE f.{nameof(Fixture.league_id)} = @LeagueId AND (f.{nameof(Fixture.home_team_id)} = @TeamId OR f.{nameof(Fixture.away_team_id)} = @TeamId) AND f.{nameof(Fixture.match_time)} < @MatchTime " +
                $"ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 10;";
            StandingsQueryString = $"{DB_FootballStandingsDetail.SelectQuery} " +
                $"WHERE s.{nameof(Standings.league_id)} = @LeagueId AND s.{nameof(Standings.team_id)} IN (@HomeTeamId, @AwayTeamId);";
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
                            _output.Result = 2;
                            return;
                        }

                        _output.HomeRecntFixtures = footballDB.Query<DB_FootballFixtureDetail>(FixturesQueryString, new { TeamId = fixture.home_team_id, MatchTime = fixture.match_time });
                        _output.AwayRecentFixtures = footballDB.Query<DB_FootballFixtureDetail>(FixturesQueryString, new { TeamId = fixture.away_team_id, MatchTime = fixture.match_time });

                        _output.League_HomeRecentFixtures = footballDB.Query<DB_FootballFixtureDetail>(LeagueFixturesQueryString, new { LeagueId = fixture.league_id, TeamId = fixture.home_team_id, MatchTime = fixture.match_time });
                        _output.League_AwayRecentFixtures = footballDB.Query<DB_FootballFixtureDetail>(LeagueFixturesQueryString, new { LeagueId = fixture.league_id, TeamId = fixture.away_team_id, MatchTime = fixture.match_time });

                        _output.StandingsDetails = footballDB.Query<DB_FootballStandingsDetail>(StandingsQueryString, new { LeagueId = fixture.league_id, HomeTeamId = fixture.home_team_id, AwayTeamId = fixture.away_team_id });

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