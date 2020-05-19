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
    public class P_SELECT_FIXTURES_BY_DATE : MysqlQuery<P_SELECT_FIXTURES_BY_DATE.Input, IEnumerable<DB_FootballFixtureDetail>>
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
            queryString = $"{DB_FootballFixtureDetail.SelectQuery} INNER JOIN league_coverage as lc on lc.{nameof(LeagueCoverage.league_id)} = l.{nameof(League.id)} " +
                $"WHERE f.{nameof(Fixture.match_time)} BETWEEN \"{_input.StartTime.ToString("yyyyMMddTHHmmss")}\" AND \"{_input.EndTime.ToString("yyyyMMddTHHmmss")}\" AND lc.{nameof(LeagueCoverage.predictions)} = 1 ;";
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