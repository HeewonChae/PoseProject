using Repository.Mysql.Dapper;
using Repository.Mysql.PoseGlobalDB.Tables;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Procedures
{
    public class P_USER_LOGIN : MysqlQuery<string, P_USER_LOGIN.Output> // input = flatform_id
    {
        private string UserSelectQuery;

        public class Output
        {
            public long UserNo { get; set; }
            public DateTime LastLoginTime { get; set; }
            public string RoleType { get; set; }
            public DateTime RoleExpireTime { get; set; }
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
            UserSelectQuery = $"SELECT base.{nameof(UserBase.user_no)} as UserNo, base.{nameof(UserBase.last_login_date)} as LastLoginTime, " +
                $"role.{nameof(UserRole.role_type)} as RoleType, role.{nameof(UserRole.expire_time)} as RoleExpireTime " +
                $"FROM user_base as base " +
                $"INNER JOIN user_role as role ON base.{nameof(UserBase.user_no)} = role.{nameof(UserRole.user_no)} " +
                $"WHERE base.{nameof(UserBase.platform_id)} = @platform_id";
        }

        public override Output OnQuery()
        {
            DapperFacade.DoWithDBContext(
                    null,
                     (Contexts.PoseGlobalDB pose_globalDB) =>
                     {
                         _output = pose_globalDB.Query<Output>(UserSelectQuery, new { platform_id = _input }).FirstOrDefault();

                         if (_output != null)
                         {
                             pose_globalDB.Execute("UPDATE user_base SET last_login_date = @last_login_date WHERE platform_id = @platform_id",
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