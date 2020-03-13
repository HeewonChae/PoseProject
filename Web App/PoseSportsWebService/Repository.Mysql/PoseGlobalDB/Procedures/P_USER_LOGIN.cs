using Repository.Mysql.Dapper;
using Repository.Mysql.PoseGlobalDB.Tables;
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
        public class Output
        {
            public long UserNo { get; set; }
            public int RoleType { get; set; }
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
                         _output = pose_globalDB.QuerySQL<Output>("SELECT user_no as UserNo, role_type as RoleType FROM user_base WHERE platform_id = @platform_id",
                                                                               new { platform_id = _input }).FirstOrDefault();

                         if (_output != null)
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