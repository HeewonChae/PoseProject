using Repository.Mysql.Dapper;
using Repository.Mysql.FootballDB.Tables;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_SELECT_PREDICTABLE_FIXTURES : MysqlQuery<P_SELECT_PREDICTABLE_FIXTURES.Input, IEnumerable<Fixture>>
    {
        public struct Input
        {
            public string WHERE { get; set; }
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
            // if you need Binding Parameters, write here
        }

        public override IEnumerable<Fixture> OnQuery()
        {
            var sb = new StringBuilder();
            sb.Append($"SELECT f.* ");
            sb.Append("FROM fixture as f ");
            sb.Append($"INNER JOIN league as l on f.{nameof(Fixture.league_id)} = l.{nameof(League.id)} ");
            sb.Append($"WHERE {_input.WHERE};");

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output = footballDB.Query<Fixture>(sb.ToString());
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