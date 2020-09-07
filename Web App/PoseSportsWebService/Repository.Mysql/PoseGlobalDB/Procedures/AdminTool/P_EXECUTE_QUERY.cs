using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Procedures.AdminTool
{
    public class P_EXECUTE_QUERY : MysqlQuery<string, int>
    {
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

        public override int OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.PoseGlobalDB globalDB) =>
                    {
                        if (!string.IsNullOrEmpty(_input))
                            _output = globalDB.Execute(_input);
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