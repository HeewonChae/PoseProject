using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tables = Repository.Mysql.PoseGlobalDB.Tables;

namespace Repository.Mysql.PoseGlobalDB.Procedures
{
    public class P_USER_LOGIN : MysqlQuery<string, P_USER_LOGIN.Output> // input = flatform_id
    {
        public struct Output
        {
            public bool Success;
            public long UserNo;
            public int RoleType;
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

        public override Output OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                     (Contexts.PoseGlobalDB pose_globalDB) =>
                     {
                         var (user_no, role_type) = pose_globalDB.QuerySQL<(long? user_no, int? role_type)>("SELECT user_no, role_type FROM user_base WHERE platform_id = @platform_id",
                                                                               new { platform_id = _input }).FirstOrDefault();

                         if (user_no.HasValue)
                         {
                             pose_globalDB.ExecuteSQL("UPDATE user_base SET last_login_date = @last_login_date WHERE platform_id = @platform_id",
                                                                                new { last_login_date = DateTime.UtcNow, platform_id = _input });
                             _output.Success = true;
                             _output.UserNo = user_no.Value;
                             _output.RoleType = role_type.Value;
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