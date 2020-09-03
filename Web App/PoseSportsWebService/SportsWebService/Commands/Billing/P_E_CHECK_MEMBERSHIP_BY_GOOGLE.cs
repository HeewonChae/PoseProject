using LogicCore.Converter;
using LogicCore.Utility.ThirdPartyLog;
using PosePacket.Service.Billing;
using PosePacket.Service.Billing.Models;
using PosePacket.Service.Billing.Models.Enums;
using PosePacket.Service.Enum;
using SportsWebService.App_Config;
using SportsWebService.Authentication;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Models.Enums;
using SportsWebService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SportsWebService.Commands.Billing
{
    using PoseGlobalDB = Repository.Mysql.PoseGlobalDB;

    [WebModelType(InputType = typeof(I_E_CHECK_MEMBERSHIP_BY_GOOGLE), OutputType = typeof(O_E_CHECK_MEMBERSHIP_BY_GOOGLE))]
    public static class P_E_CHECK_MEMBERSHIP_BY_GOOGLE
    {
        public static class RowCode
        {
            [Description("Invalid in app procut id")]
            public const int Invalid_Product_Id = ServiceErrorCode.WebMethod_Billing.P_E_CHECK_MEMBERSHIP_BY_GOOGLE + 1;

            public const int P_SELECT_LINKED_BILLING = ServiceErrorCode.StoredProcedure_Global.P_SELECT_LINKED_BILLING;

            [Description("Not Found User Membership")]
            public const int DB_Not_Found_User_Role = ServiceErrorCode.StoredProcedure_Global.P_SELECT_LINKED_BILLING + 1;

            [Description("Not Found Billing History")]
            public const int DB_Not_Found_Billing = ServiceErrorCode.StoredProcedure_Global.P_SELECT_LINKED_BILLING + 2;

            [Description("User Role Update Failed")]
            public const int DB_User_Role_Update_Failed = ServiceErrorCode.StoredProcedure_Global.P_UPDATE_USER_ROLE + 1;
        }

        public static async Task<O_E_CHECK_MEMBERSHIP_BY_GOOGLE> Execute(I_E_CHECK_MEMBERSHIP_BY_GOOGLE input, long userNo)
        {
            // Check DB
            PoseGlobalDB.Procedures.P_SELECT_LINKED_BILLING.Output db_output;
            using (var P_SELECT_LINKED_BILLING = new PoseGlobalDB.Procedures.P_SELECT_LINKED_BILLING())
            {
                P_SELECT_LINKED_BILLING.SetInput(userNo);

                db_output = P_SELECT_LINKED_BILLING.OnQuery();

                if (P_SELECT_LINKED_BILLING.EntityStatus != null || db_output.Result != 0)
                    ErrorHandler.OccurException(RowCode.P_SELECT_LINKED_BILLING + db_output.Result);
            }

            // Check ProductID valid
            if (!InAppPurchase.TryGetInAppPurchase(db_output.InAppBilling.product_id, out var inAppPurchase)
                || inAppPurchase.StoreType != StoreType.GooglePlay)
            {
                Log4Net.WriteLog($"Invalid Google ProudctId, UserNo: {userNo}, productId: {db_output.InAppBilling.product_id}", Log4Net.Level.ERROR);
                ErrorHandler.OccurException(RowCode.Invalid_Product_Id);
            }

            PoseBillingResult billingResult = null;
            long linkedTransNo = 0;
            if (inAppPurchase.PurchaseType == InAppPurchaseType.InAppProduct)
            {
                linkedTransNo = db_output.UserRole.linked_trans_no;

                billingResult = new PoseBillingResult();
                billingResult.MemberRoleType = inAppPurchase.OfferRoleType;
                billingResult.RoleExpireTime = db_output.UserRole.expire_time;
                billingResult.ProductId = db_output.InAppBilling.product_id;
                billingResult.PurchaseStateType = db_output.UserRole.expire_time > DateTime.UtcNow ?
                    PosePurchaseStateType.Purchased : PosePurchaseStateType.Unknown;
            }
            else if (inAppPurchase.PurchaseType == InAppPurchaseType.Subscription)
            {
                var process_ret = await P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE.SubscriptionProcess(inAppPurchase, input.AppPackageName, db_output.InAppBilling.purchase_token);
                billingResult = process_ret.BillingResult;
                linkedTransNo = process_ret.Payload;
            }

            // 회원등급 심사
            if (billingResult.PurchaseStateType != PosePurchaseStateType.Purchased
                && billingResult.PurchaseStateType != PosePurchaseStateType.Grace) // 결제 유예기간..
            {
                billingResult.MemberRoleType = MemberRoleType.Regular;
                linkedTransNo = 0;
            }

            // Update DB
            bool db_output2;
            using (var P_UPDATE_USER_ROLE = new PoseGlobalDB.Procedures.P_UPDATE_USER_ROLE())
            {
                P_UPDATE_USER_ROLE.SetInput(new PoseGlobalDB.Procedures.P_UPDATE_USER_ROLE.Input
                {
                    UserNo = userNo,
                    LinkedTransNo = linkedTransNo,
                    RoleType = billingResult.MemberRoleType.ToString(),
                    RoleExpireTime = billingResult.RoleExpireTime,
                    CurrentTime = DateTime.UtcNow,
                });

                db_output2 = P_UPDATE_USER_ROLE.OnQuery();

                if (P_UPDATE_USER_ROLE.EntityStatus != null || db_output2 == false)
                    ErrorHandler.OccurException(RowCode.DB_User_Role_Update_Failed);
            }

            // Refrash PoseToken
            billingResult.MemberRoleType.ToString().TryParseEnum(out ServiceRoleType serviceRoleType);
            billingResult.PoseToken = PoseCredentials.CreateToken(userNo, serviceRoleType);

            return new O_E_CHECK_MEMBERSHIP_BY_GOOGLE
            {
                BillingResult = billingResult,
            };
        }
    }
}