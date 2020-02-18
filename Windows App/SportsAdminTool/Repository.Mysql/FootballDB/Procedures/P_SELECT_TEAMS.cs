using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Procedures
{
    public class P_SELECT_TEAMS : MysqlQuery<P_SELECT_TEAMS.Input, IEnumerable<Table.Team>>
    {
        public struct Input
        {
            string _where;
            string _groupBy;
            string _orderBy;

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

        public override IEnumerable<Table.Team> OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.FootballDB footballDB) =>
                    {
                        _output = footballDB.QuerySQL<Table.Team>($"SELECT * FROM team {_input.Where} {_input.GroupBy} {_input.OrderBy}");
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
