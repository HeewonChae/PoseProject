using Repository.Mysql;
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
    public class P_UPDATE_USER_ROLE : MysqlQuery<P_UPDATE_USER_ROLE.Input, bool>
    {
        public struct Input
        {
            public long UserNo { get; set; }
            public long LinkedTransNo { get; set; }
            public string RoleType { get; set; }
            public DateTime RoleExpireTime { get; set; }
            public DateTime CurrentTime { get; set; }
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
        }

        public override bool OnQuery()
        {
            _output = false;

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.PoseGlobalDB pose_globalDB) =>
                    {
                        pose_globalDB.Execute(
                            "UPDATE user_role " +
                            "SET role_type = @RoleType, linked_trans_no = @LinkedTransNo, " +
                            "expire_time = @RoleExpireTime, upt_date = @CurrentTime " +
                            "WHERE user_no = @UserNo"
                            , _input);
                        _output = true;
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