using PosePacket.Service.Football.Models.Enums;
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
    public class P_SELECT_FIXTURES_BY_TEAM : MysqlQuery<P_SELECT_FIXTURES_BY_TEAM.Input, P_SELECT_FIXTURES_BY_TEAM.Output>
    {
        public string FindTeamQuery;
        public string FinishedFixturesQuery;
        public string ScheduledixturesQuery;

        public struct Input
        {
            public SearchFixtureStatusType SearchFixtureStatusType { get; set; }
            public short TeamId { get; set; }
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
            FindTeamQuery = $"SELECT * FROM team WHERE {nameof(Team.id)} = {_input.TeamId};";

            FinishedFixturesQuery = $"{DB_FootballFixtureDetail.SelectQuery} " +
                $"WHERE ( f.{nameof(Fixture.home_team_id)} = {_input.TeamId} OR f.{nameof(Fixture.away_team_id)} = {_input.TeamId} ) " +
                $"AND f.{nameof(Fixture.match_time)} <= @CurDate " +
                $"ORDER BY f.{nameof(Fixture.match_time)} DESC LIMIT 15 ;";

            ScheduledixturesQuery = $"{DB_FootballFixtureDetail.SelectQuery} " +
                $"WHERE ( f.{nameof(Fixture.home_team_id)} = {_input.TeamId} OR f.{nameof(Fixture.away_team_id)} = {_input.TeamId} ) " +
                $"AND f.{nameof(Fixture.match_time)} > @CurDate " +
                $"ORDER BY f.{nameof(Fixture.match_time)} ASC LIMIT 15 ;";
        }

        public override Output OnQuery()
        {
            _output = new Output();

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        var team = footballDB.Query<Team>(FindTeamQuery).FirstOrDefault();
                        if (team == null)
                        {
                            _output.Result = 1;
                            return;
                        }

                        // Finished Fixtures
                        if (_input.SearchFixtureStatusType == SearchFixtureStatusType.Finished)
                        {
                            _output.FixtureDetails = footballDB.Query<DB_FootballFixtureDetail>(FinishedFixturesQuery, new { CurDate = DateTime.UtcNow });
                        }
                        // Scheduled Fixtures
                        else if (_input.SearchFixtureStatusType == SearchFixtureStatusType.Scheduled)
                        {
                            _output.FixtureDetails = footballDB.Query<DB_FootballFixtureDetail>(ScheduledixturesQuery, new { CurDate = DateTime.UtcNow });
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