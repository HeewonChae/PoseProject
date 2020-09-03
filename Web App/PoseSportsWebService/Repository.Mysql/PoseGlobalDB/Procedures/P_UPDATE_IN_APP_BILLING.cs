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
    public class P_UPDATE_IN_APP_BILLING : MysqlQuery<P_UPDATE_IN_APP_BILLING.Input, P_UPDATE_IN_APP_BILLING.Output>
    {
        public struct Input
        {
            public long UserNo { get; set; }
            public long TransNo { get; set; }
            public string OrderId { get; set; }
            public string PurchaseToken { get; set; }
            public string PurchaseState { get; set; }
            public string RoleType { get; set; }
            public DateTime RoleExpireTime { get; set; }
            public DateTime CurrentTime { get; set; }
        }

        public struct Output
        {
            public int Result { get; set; }
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
            this.Parmeters.Add("i_user_no", _input.UserNo, DbType.Int64, ParameterDirection.Input);
            this.Parmeters.Add("i_trans_no", _input.TransNo, DbType.Int64, ParameterDirection.Input);
            this.Parmeters.Add("i_order_id", _input.OrderId, DbType.String, ParameterDirection.Input);
            this.Parmeters.Add("i_purchase_token", _input.PurchaseToken, DbType.String, ParameterDirection.Input);
            this.Parmeters.Add("i_purchase_state", _input.PurchaseState, DbType.String, ParameterDirection.Input);
            this.Parmeters.Add("i_role_type", _input.RoleType, DbType.String, ParameterDirection.Input);
            this.Parmeters.Add("i_role_expire", _input.RoleExpireTime, DbType.DateTime, ParameterDirection.Input);
            this.Parmeters.Add("i_cur_time", _input.CurrentTime, DbType.DateTime, ParameterDirection.Input);

            this.Parmeters.Add("o_result", 0, DbType.Int32, ParameterDirection.Output);
        }

        public override Output OnQuery()
        {
            _output.Result = -1;

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.PoseGlobalDB pose_globalDB) =>
                    {
                        var affects = pose_globalDB.ExecuteSP("P_UPDATE_IN_APP_BILLING", this.Parmeters);
                        _output.Result = this.Parmeters.Get<int>("o_result");
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