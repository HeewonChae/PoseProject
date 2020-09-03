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
    public class P_SELECT_LINKED_BILLING : MysqlQuery<long, P_SELECT_LINKED_BILLING.Output>
    {
        public struct Output
        {
            public UserRole UserRole;
            public InAppBilling InAppBilling;
            public int Result;
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

        public override P_SELECT_LINKED_BILLING.Output OnQuery()
        {
            _output.Result = -1;

            DapperFacade.DoWithDBContext(
                    null,
                    (Contexts.PoseGlobalDB pose_globalDB) =>
                    {
                        _output.UserRole = pose_globalDB.Query<UserRole>(
                            "SELECT * FROM user_role WHERE user_no = @userNo",
                            new { userNo = _input }).FirstOrDefault();

                        if (_output.UserRole == null)
                        {
                            _output.Result = 1;
                            return;
                        }

                        _output.InAppBilling = pose_globalDB.Query<InAppBilling>(
                            "SELECT * FROM in_app_billing WHERE trans_no = @transNo",
                            new { transNo = _output.UserRole.linked_trans_no }).FirstOrDefault();

                        if (_output.InAppBilling == null)
                        {
                            _output.Result = 2;
                            return;
                        }

                        _output.Result = 0;
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