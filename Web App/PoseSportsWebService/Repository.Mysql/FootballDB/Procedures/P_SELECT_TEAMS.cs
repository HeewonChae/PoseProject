using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_SELECT_TEAMS : MysqlQuery<P_SELECT_TEAMS.Input, IEnumerable<Tables.Team>>
    {
        public struct Input
        {
            private string _where;
            private string _groupBy;
            private string _orderBy;

            public string Where { get { return _where ?? ""; } set { _where = $"WHERE {value}"; } }
            public string GroupBy { get { return _groupBy ?? ""; } set { _groupBy = $"GROUP BY {value}"; } }
            public string OrderBy { get { return _orderBy ?? ""; } set { _orderBy = $"ORDER BY {value}"; } }
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

        public override IEnumerable<Tables.Team> OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output = footballDB.Query<Tables.Team>($"SELECT * FROM team {_input.Where} {_input.GroupBy} {_input.OrderBy}");
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