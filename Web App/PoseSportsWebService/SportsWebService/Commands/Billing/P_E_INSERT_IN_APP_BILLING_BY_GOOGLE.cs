using PosePacket.Service.Billing;
using PosePacket.Service.Billing.Models.Enums;
using PosePacket.Service.Enum;
using SportsWebService.App_Config;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Windows.Input;

namespace SportsWebService.Commands.Billing
{
    using PoseGlobalDB = Repository.Mysql.PoseGlobalDB;

    [WebModelType(InputType = typeof(I_E_INSERT_IN_APP_BILLING_BY_GOOGLE), OutputType = typeof(O_E_INSERT_IN_APP_BILLING_BY_GOOGLE))]
    public static class P_E_INSERT_IN_APP_BILLING_BY_GOOGLE
    {
        public static class RowCode
        {
            [Description("Invalid input value")]
            public const int Invalid_InputValue = ServiceErrorCode.WebMethod_Billing.P_E_INSERT_IN_APP_BILLING_BY_GOOGLE + 1;

            [Description("Invalid in app procut id")]
            public const int Invalid_Product_Id = ServiceErrorCode.WebMethod_Billing.P_E_INSERT_IN_APP_BILLING_BY_GOOGLE + 2;

            [Description("Failed insert database")]
            public const int DB_Failed_Insert_Billing = ServiceErrorCode.StoredProcedure_Global.P_INSERT_IN_APP_BILLING + 1;
        }

        public static O_E_INSERT_IN_APP_BILLING_BY_GOOGLE Execute(I_E_INSERT_IN_APP_BILLING_BY_GOOGLE input, long userNo)
        {
            if (input == null
                || string.IsNullOrEmpty(input.ProductID))
                ErrorHandler.OccurException(RowCode.Invalid_InputValue);

            // Check ProductID valid
            if (!InAppPurchase.TryGetInAppPurchase(input.ProductID, out var inAppProduct)
                || inAppProduct.StoreType != StoreType.GooglePlay)
                ErrorHandler.OccurException(RowCode.Invalid_Product_Id);

            // Check DB
            PoseGlobalDB.Procedures.P_INSERT_IN_APP_BILLING.Output db_output;
            using (var P_INSERT_IN_APP_BILLING = new PoseGlobalDB.Procedures.P_INSERT_IN_APP_BILLING())
            {
                P_INSERT_IN_APP_BILLING.SetInput(new PoseGlobalDB.Procedures.P_INSERT_IN_APP_BILLING.Input
                {
                    UserNo = userNo,
                    ProductID = input.ProductID,
                    PurchaseState = PosePurchaseStateType.Canceled.ToString(),
                    StoreType = StoreType.GooglePlay.ToString(),
                    CurrentTime = DateTime.UtcNow,
                });

                db_output = P_INSERT_IN_APP_BILLING.OnQuery();

                if (P_INSERT_IN_APP_BILLING.EntityStatus != null || db_output.Result != 0)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Insert_Billing);
            }

            return new O_E_INSERT_IN_APP_BILLING_BY_GOOGLE
            {
                BillingPayload = db_output.TransNo.ToString(),
            };
        }
    }
}