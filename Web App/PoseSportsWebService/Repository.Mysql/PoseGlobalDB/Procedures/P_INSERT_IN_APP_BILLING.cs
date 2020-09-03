using Repository.Mysql.Dapper;
using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Procedures
{
    public class P_INSERT_IN_APP_BILLING : MysqlQuery<P_INSERT_IN_APP_BILLING.Input, P_INSERT_IN_APP_BILLING.Output>
    {
        public struct Input
        {
            public long UserNo { get; set; }
            public string StoreType { get; set; }
            public string ProductID { get; set; }
            public string PurchaseState { get; set; }
            public DateTime CurrentTime { get; set; }
        }

        public struct Output
        {
            public int Result { get; set; }
            public long TransNo { get; set; }
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

        public override Output OnQuery()
        {
            _output.Result = -1;

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.PoseGlobalDB pose_globalDB) =>
                    {
                        var transNo = pose_globalDB.ExecuteScalar<long>(
                            "INSERT INTO in_app_billing(user_no, store_type, product_id, purchase_state, upt_date, ipt_date) VALUES " +
                            "(@UserNo, @StoreType, @ProductID, @PurchaseState, @CurrentTime, @CurrentTime); " +
                            "SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);", _input);

                        if (transNo > 0)
                        {
                            _output.Result = 0;
                            _output.TransNo = transNo;
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