using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Procedures
{
    public class P_INSERT_USER_BASE : MysqlQuery<P_INSERT_USER_BASE.Input, bool>
    {
        public struct Input
        {
            public string PlatformId { get; set; }
            public string PlatformType { get; set; }
            public int RoleType { get; set; }
            public DateTime InsertTime { get; set; }
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

        public override bool OnQuery()
        {
            _output = false;

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.PoseGlobalDB pose_globalDB) =>
                    {
                        var isExist = pose_globalDB.ExecuteScalar<bool>("SELECT IF (EXISTS (SELECT * FROM user_base WHERE platform_id = @PlatformId), 1, 0)",
                                                                                new { _input.PlatformId });

                        if (isExist)
                        {
                            _output = true;
                        }
                        else
                        {
                            var affectedRows = pose_globalDB.Execute("INSERT INTO user_base(platform_id, platform_type, role_type, last_login_date, ipt_date)VALUE(@PlatformId, @PlatformType, @RoleType, @InsertTime, @InsertTime);",
                                                                            _input);

                            if (affectedRows == 1)
                                _output = true;
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