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
    public class P_SELECT_FIXTURES_BY_INDEX : MysqlQuery<P_SELECT_FIXTURES_BY_INDEX.Input, IEnumerable<DB_FootballFixtureDetail>>
    {
        public string queryString;

        public struct Input
        {
            public int[] Indexes { get; set; }
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
            queryString = $"{DB_FootballFixtureDetail.SelectQuery} WHERE f.{nameof(Fixture.id)} in @Ids;";
        }

        public override IEnumerable<DB_FootballFixtureDetail> OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output = footballDB.Query<DB_FootballFixtureDetail>(queryString, new { Ids = _input.Indexes });
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