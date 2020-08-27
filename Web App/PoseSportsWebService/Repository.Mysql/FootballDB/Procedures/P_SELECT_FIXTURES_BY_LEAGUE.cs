using PosePacket.Service.Enum;
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
    public class P_SELECT_FIXTURES_BY_LEAGUE : MysqlQuery<P_SELECT_FIXTURES_BY_LEAGUE.Input, P_SELECT_FIXTURES_BY_LEAGUE.Output>
    {
        public string FindLeagueQuery;
        public string FinishedFixturesQuery;
        public string ScheduledixturesQuery;

        public struct Input
        {
            public SearchFixtureStatusType SearchFixtureStatusType { get; set; }
            public string CountryName { get; set; }
            public string LeagueName { get; set; }
        }

        public class Output
        {
            public IEnumerable<DB_FootballFixtureDetail> FixtureDetails { get; set; }
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
            FindLeagueQuery = $"SELECT * FROM league WHERE {nameof(League.country_name)} = \'{_input.CountryName}\' AND {nameof(League.name)} = \'{_input.LeagueName}\' AND {nameof(League.is_current)} = 1 ;";
            FinishedFixturesQuery = $"{DB_FootballFixtureDetail.SelectQuery} WHERE f.{nameof(Fixture.league_id)} = @LeagueId AND f.{nameof(Fixture.match_time)} <= @CurDate ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 64 ;";
            ScheduledixturesQuery = $"{DB_FootballFixtureDetail.SelectQuery} WHERE f.{nameof(Fixture.league_id)} = @LeagueId AND f.{nameof(Fixture.match_time)} > @CurDate ORDER BY f.{nameof(Fixture.match_time)} ASC LIMIT 64 ;";
        }

        public override Output OnQuery()
        {
            _output = new Output();

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        var league = footballDB.Query<League>(FindLeagueQuery).FirstOrDefault();
                        if (league == null)
                        {
                            _output.Result = 1;
                            return;
                        }

                        // Finished Fixtures
                        if (_input.SearchFixtureStatusType == SearchFixtureStatusType.Finished)
                        {
                            _output.FixtureDetails = footballDB.Query<DB_FootballFixtureDetail>(FinishedFixturesQuery, new { LeagueId = league.id, CurDate = DateTime.UtcNow });
                        }
                        // Scheduled Fixtures
                        else if (_input.SearchFixtureStatusType == SearchFixtureStatusType.Scheduled)
                        {
                            _output.FixtureDetails = footballDB.Query<DB_FootballFixtureDetail>(ScheduledixturesQuery, new { LeagueId = league.id, CurDate = DateTime.UtcNow });
                        }

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