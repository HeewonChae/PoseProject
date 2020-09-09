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
        public string FixturesQueryString;

        public struct Input
        {
            public int TeamId { get; set; }
        }

        public class Output
        {
            public List<DB_FootballFixtureDetail> DB_FixtureDetails { get; set; }

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
                $"GROUP BY l.{nameof(League.id)}, c.{nameof(Country.logo)};";

            FixturesQueryString = $"{DB_FootballFixtureDetail.SelectQuery} " +
                $"WHERE f.{nameof(Fixture.league_id)} = @LeagueId AND ( f.{nameof(Fixture.home_team_id)} = {_input.TeamId} OR  f.{nameof(Fixture.away_team_id)} = {_input.TeamId} ) AND f.{nameof(Fixture.is_completed)} = 1 " +
                $"ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 18";
        }

        public override P_GET_TEAM_OVERVIEW.Output OnQuery()
        {
            _output = new Output();

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output.DB_FixtureDetails = new List<DB_FootballFixtureDetail>();

                        var leagueDetails = footballDB.Query<DB_FootballLeagueDetail>(ParticipatingLeaguesQueryString).ToList();
                        if (leagueDetails == null || leagueDetails.Count() == 0)
                            return;

                        // 중복 리그 제거
                        var leagues = new List<DB_FootballLeagueDetail>();
                        var leagueByGroups = leagueDetails.GroupBy(elem => $"{elem.CountryName}:{elem.LeagueType}:{elem.Name}");
                        foreach (var leagueByGroup in leagueByGroups)
                        {
                            if (leagueByGroup.Count() == 1)
                            {
                                leagues.Add(leagueByGroup.First());
                            }
                            else
                            {
                                leagues.Add(leagueByGroup.OrderByDescending(elem => elem.SeasonStartDate).First());
                            }
                        }

                        foreach (var league in leagues)
                        {
                            var fixtureDetails = footballDB.Query<DB_FootballFixtureDetail>(FixturesQueryString, new { LeagueId = league.Id });
                            _output.DB_FixtureDetails.AddRange(fixtureDetails);
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