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
    public class P_GET_TEAM_OVERVIEW : MysqlQuery<P_GET_TEAM_OVERVIEW.Input, P_GET_TEAM_OVERVIEW.Output>
    {
        public string ParticipatingLeaguesQueryString;
        public string HomeFixturesQueryString;
        public string AwayFixturesQueryString;

        public struct Input
        {
            public int TeamId { get; set; }
        }

        public class Output
        {
            public IDictionary<DB_FootballLeagueDetail, IEnumerable<DB_FootballFixtureDetail>> DB_FixtureDetailsByLeague { get; set; }

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
            ParticipatingLeaguesQueryString = $"{DB_FootballLeagueDetail.SelectQuery} " +
                $"INNER JOIN fixture as f ON l.{nameof(League.id)} = f.{nameof(Fixture.league_id)} " +
                $"WHERE (f.{nameof(Fixture.home_team_id)} = {_input.TeamId} OR f.{nameof(Fixture.away_team_id)} = {_input.TeamId}) AND l.{nameof(League.is_current)} = 1 " +
                $"GROUP BY l.{nameof(League.id)};";

            HomeFixturesQueryString = $"{DB_FootballFixtureDetail.SelectQuery} " +
                $"WHERE f.{nameof(Fixture.league_id)} = @LeagueId AND f.{nameof(Fixture.home_team_id)} = @TeamId AND f.{nameof(Fixture.is_completed)} = 1 " +
                $"ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 6";

            AwayFixturesQueryString = $"{DB_FootballFixtureDetail.SelectQuery} " +
                $"WHERE f.{nameof(Fixture.league_id)} = @LeagueId AND f.{nameof(Fixture.away_team_id)} = @TeamId AND f.{nameof(Fixture.is_completed)} = 1 " +
                $"ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 6";
        }

        public override P_GET_TEAM_OVERVIEW.Output OnQuery()
        {
            _output = new Output();

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output.DB_FixtureDetailsByLeague = new Dictionary<DB_FootballLeagueDetail, IEnumerable<DB_FootballFixtureDetail>>();

                        var leagueDetails = footballDB.Query<DB_FootballLeagueDetail>(ParticipatingLeaguesQueryString);
                        if (leagueDetails == null || leagueDetails.Count() == 0)
                            return;

                        foreach (var leagueDetail in leagueDetails)
                        {
                            var homeFixtureDetails = footballDB.Query<DB_FootballFixtureDetail>(HomeFixturesQueryString, new { LeagueId = leagueDetail.Id, _input.TeamId });
                            var awayFixtureDetails = footballDB.Query<DB_FootballFixtureDetail>(AwayFixturesQueryString, new { LeagueId = leagueDetail.Id, _input.TeamId });

                            _output.DB_FixtureDetailsByLeague.Add(leagueDetail, homeFixtureDetails.Concat(awayFixtureDetails));
                        }
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