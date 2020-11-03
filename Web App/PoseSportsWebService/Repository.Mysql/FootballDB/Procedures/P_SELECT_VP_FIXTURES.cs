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
    public class P_SELECT_VP_FIXTURES : MysqlQuery<P_SELECT_VP_FIXTURES.Input, IEnumerable<DB_FootballVIPFixtureDetail>>
    {
        private string _finishedFixturesQuery;
        private string _scheduledixturesQuery;

        public struct Input
        {
            public bool IsSelectHistory { get; set; }
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
            _finishedFixturesQuery = $"{DB_FootballVIPFixtureDetail.SelectQuery} " +
                $"WHERE f.{nameof(Fixture.match_time)} BETWEEN @StartTime AND @EndTime " +
                $"AND f.{nameof(Fixture.is_predicted)} = 1 AND f.{nameof(Fixture.is_completed)} = 1 " +
                $"AND p.{nameof(Prediction.is_vip_pick)} = 1; ";

            _scheduledixturesQuery = $"{DB_FootballVIPFixtureDetail.SelectQuery} " +
                $"WHERE f.{nameof(Fixture.match_time)} BETWEEN @StartTime AND @EndTime " +
                $"AND f.{nameof(Fixture.is_predicted)} = 1 AND f.{nameof(Fixture.is_completed)} = 0 " +
                $"AND p.{nameof(Prediction.is_vip_pick)} = 1; ";
        }

        public override IEnumerable<DB_FootballVIPFixtureDetail> OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        // Finished Fixtures
                        if (_input.IsSelectHistory)
                        {
                            _output = footballDB.Query<DB_FootballVIPFixtureDetail>(_finishedFixturesQuery, new { StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(-48) });
                        }
                        // Scheduled Fixtures
                        else
                        {
                            _output = footballDB.Query<DB_FootballVIPFixtureDetail>(_scheduledixturesQuery, new { StartTime = DateTime.UtcNow.Date, EndTime = DateTime.UtcNow.AddHours(48) });
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