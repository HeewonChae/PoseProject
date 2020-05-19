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
    public class P_GET_MATCH_H2H : MysqlQuery<P_GET_MATCH_H2H.Input, P_GET_MATCH_H2H.Output>
    {
        public string FixturesQueryString;

        public struct Input
        {
            public int FixtureId { get; set; }
            public short HomeTeamId { get; set; }
            public short AwayTeamId { get; set; }
        }

        public class Output
        {
            public IEnumerable<DB_FootballFixtureDetail> H2HFixtures { get; set; }
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
                $"WHERE (f.{nameof(Fixture.home_team_id)} = @HomeTeamId AND f.{nameof(Fixture.away_team_id)} = @AwayTeamId AND f.{nameof(Fixture.match_time)} BETWEEN @SearchStartTime AND @SearchEndTime AND f.{nameof(Fixture.is_completed)} = 1) " +
                $"OR (f.{nameof(Fixture.home_team_id)} = @AwayTeamId AND f.{nameof(Fixture.away_team_id)} = @HomeTeamId AND f.{nameof(Fixture.match_time)} BETWEEN @SearchStartTime AND @SearchEndTime AND f.{nameof(Fixture.is_completed)} = 1) ";
        }

        public override P_GET_MATCH_H2H.Output OnQuery()
        {
            _output = new Output();

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        var fixture = footballDB.Query<Fixture>($"SELECT * FROM fixture WHERE id = {_input.FixtureId}").FirstOrDefault();
                        if (fixture == null
                        || fixture.home_team_id != _input.HomeTeamId
                        || fixture.away_team_id != _input.AwayTeamId)
                        {
                            _output.Result = 1;
                            return;
                        }

                        var SearchStartTime = new DateTime(fixture.match_time.Year - 10, 1, 1);
                        var SearchEndTime = fixture.match_time;

                        _output.H2HFixtures = footballDB.Query<DB_FootballFixtureDetail>(FixturesQueryString,
                            new { _input.HomeTeamId, _input.AwayTeamId, SearchStartTime, SearchEndTime });

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