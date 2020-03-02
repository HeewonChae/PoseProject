using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Procedures
{
    public class P_USER_LOGIN : MysqlQuery<string, long?> // input = flatform_id
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

        public override long? OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                     (Contexts.PoseGlobalDB pose_globalDB) =>
                     {
                         _output = pose_globalDB.ExecuteScalar<long?>("SELECT user_no FROM user_base WHERE platform_id = @platform_id",
                                                                               new { platform_id = _input });

                         if (_output.HasValue)
                         {
                             pose_globalDB.ExecuteSQL("UPDATE user_base SET last_login_date = @last_login_date WHERE platform_id = @platform_id",
                                                                                new { last_login_date = DateTime.UtcNow, platform_id = _input });
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