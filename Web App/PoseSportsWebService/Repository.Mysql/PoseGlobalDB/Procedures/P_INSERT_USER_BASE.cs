using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Procedures
{
    public class P_INSERT_USER_BASE : MysqlQuery<P_INSERT_USER_BASE.Input, int>
    {
        public struct Input
        {
            public string PlatformId { get; set; }
            public string PlatformType { get; set; }
            public string PlatformEmail { get; set; }
            public string RoleType { get; set; }
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
            this.Parmeters.Add("i_platform_id", _input.PlatformId, DbType.String, ParameterDirection.Input);
            this.Parmeters.Add("i_platform_type", _input.PlatformType, DbType.String, ParameterDirection.Input);
            this.Parmeters.Add("i_platform_email", _input.PlatformEmail, DbType.String, ParameterDirection.Input);
            this.Parmeters.Add("i_role_type", _input.RoleType, DbType.String, ParameterDirection.Input);
            this.Parmeters.Add("i_cur_time", _input.CurrentTime, DbType.DateTime, ParameterDirection.Input);

            this.Parmeters.Add("o_result", 0, DbType.Int32, ParameterDirection.Output);
        }

        public override int OnQuery()
        {
            _output = -1;

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.PoseGlobalDB pose_globalDB) =>
                    {
                        var affects = pose_globalDB.ExecuteSP("P_INSERT_USER_BASE", this.Parmeters);
                        _output = this.Parmeters.Get<int>("o_result");
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