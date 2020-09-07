using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Procedures.AdminTool
{
    public class P_SELECT_QUERY<T> : MysqlQuery<P_SELECT_QUERY<T>.Input, IEnumerable<T>>
    {
        public struct Input
        {
            private string _where;
            private string _groupBy;
            private string _orderBy;

            public string Query { get; set; }

            public string Where
            {
                get { return _where ?? ""; }
                set
                {
                    if (!string.IsNullOrEmpty(value))
                        _where = $"WHERE {value}";
                    else
                        _where = value;
                }
            }

            public string GroupBy
            {
                get { return _groupBy ?? ""; }
                set
                {
                    if (!string.IsNullOrEmpty(value))
                        _groupBy = $"GROUP BY {value}";
                    else
                        _groupBy = value;
                }
            }

            public string OrderBy
            {
                get { return _orderBy ?? ""; }
                set
                {
                    if (!string.IsNullOrEmpty(value))
                        _orderBy = $"ORDER BY {value}";
                    else
                        _orderBy = value;
                }
            }
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

        public override IEnumerable<T> OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.PoseGlobalDB globalDB) =>
                    {
                        if (!string.IsNullOrEmpty(_input.Query))
                            _output = globalDB.Query<T>($"{_input.Query} {_input.Where} {_input.GroupBy} {_input.OrderBy}");
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