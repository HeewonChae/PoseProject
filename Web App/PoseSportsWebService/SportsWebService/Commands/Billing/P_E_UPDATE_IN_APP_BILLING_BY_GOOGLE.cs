using Google.Apis.AndroidPublisher.v3;
using Google.Apis.AndroidPublisher.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using LogicCore.Converter;
using LogicCore.Utility;
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
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SportsWebService.Commands.Billing
{
    using PoseGlobalDB = Repository.Mysql.PoseGlobalDB;

    [WebModelType(InputType = typeof(I_E_UPDATE_IN_APP_BILLING_BY_GOOGLE), OutputType = typeof(O_E_UPDATE_IN_APP_BILLING_BY_GOOGLE))]
    public static class P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE
    {
        private readonly static AndroidPublisherService _androidPublisher;

        static P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE()
        {
            var keyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/poseidon-picks-dev-681a7a0a4f9a.json");
            using (var stream = new FileStream(keyPath, FileMode.Open, FileAccess.Read))

            {
                var credential = GoogleCredential.FromStream(stream)
                    .CreateScoped("https://www.googleapis.com/auth/androidpublisher")
                    .UnderlyingCredential as ServiceAccountCredential;

                _androidPublisher = new AndroidPublisherService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Poseidon Picks",
                });
            }
        }

        public static class RowCode
        {
            [Description("Invalid input value")]
            public const int Invalid_InputValue = ServiceErrorCode.WebMethod_Billing.P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE + 1;

            [Description("Invalid in app procut id")]
            public const int Invalid_Product_Id = ServiceErrorCode.WebMethod_Billing.P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE + 2;

            [Description("Invalid google receipt")]
            public const int Invalid_Google_Receipt = ServiceErrorCode.WebMethod_Billing.P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE + 3;

            public const int P_UPDATE_IN_APP_BILLING = ServiceErrorCode.StoredProcedure_Global.P_UPDATE_IN_APP_BILLING;

            [Description("Failed update database")]
            public const int DB_Failed_Insert_Billing = ServiceErrorCode.StoredProcedure_Global.P_UPDATE_IN_APP_BILLING + 1;

            [Description("Not found dilling dada")]
            public const int DB_Not_Found_Billing_Data = ServiceErrorCode.StoredProcedure_Global.P_UPDATE_IN_APP_BILLING + 2;

            [Description("Not found user role data")]
            public const int DB_Not_Found_User_Role = ServiceErrorCode.StoredProcedure_Global.P_UPDATE_IN_APP_BILLING + 3;
        }

        public static async Task<O_E_UPDATE_IN_APP_BILLING_BY_GOOGLE> Execute(I_E_UPDATE_IN_APP_BILLING_BY_GOOGLE input, long userNo)
        {
            if (input == null
                || string.IsNullOrEmpty(input.ProductID)
                || string.IsNullOrEmpty(input.AppPackageName)
                || string.IsNullOrEmpty(input.PurchaseToken))
                ErrorHandler.OccurException(RowCode.Invalid_InputValue);

            // Check ProductID valid
            if (!InAppPurchase.TryGetInAppPurchase(input.ProductID, out var inAppPurchase)
                || inAppPurchase.StoreType != StoreType.GooglePlay)
            {
                Log4Net.WriteLog($"Invalid Google ProudctId, UserNo: {userNo}, productId: {input.ProductID}", Log4Net.Level.ERROR);
                ErrorHandler.OccurException(RowCode.Invalid_Product_Id);
            }

            PoseBillingResult billingResult = null;
            long trasNo = 0;
            string orderId = string.Empty;
            switch (inAppPurchase.PurchaseType)
            {
                case InAppPurchaseType.InAppProduct: // 소비성 상품
                    {
                        var process_ret = await InAppProductProcess(inAppPurchase, input.AppPackageName, input.PurchaseToken);
                        billingResult = process_ret.BillingResult;
                        trasNo = process_ret.Payload;
                        orderId = process_ret.OrderId;
                    }
                    break;

                case InAppPurchaseType.Subscription: // 구독 상품
                    {
                        var process_ret = await SubscriptionProcess(inAppPurchase, input.AppPackageName, input.PurchaseToken);
                        billingResult = process_ret.BillingResult;
                        trasNo = process_ret.Payload;
                        orderId = process_ret.OrderId;
                    }
                    break;
            }

            // 유효하지않은 PurchaseToken
            if (billingResult == null)
            {
                Log4Net.WriteLog($"Google PurchaseToken is Invalid, UserNo: {userNo}, productId: {input.ProductID}, purchaseToken: {input.PurchaseToken}", Log4Net.Level.ERROR);
                ErrorHandler.OccurException(RowCode.Invalid_Google_Receipt);
            }

            if (billingResult.PurchaseStateType == PosePurchaseStateType.Purchased)
            {
                // DB Process
                PoseGlobalDB.Procedures.P_UPDATE_IN_APP_BILLING.Output db_output;
                using (var P_UPDATE_IN_APP_BILLING = new PoseGlobalDB.Procedures.P_UPDATE_IN_APP_BILLING())
                {
                    P_UPDATE_IN_APP_BILLING.SetInput(new PoseGlobalDB.Procedures.P_UPDATE_IN_APP_BILLING.Input
                    {
                        UserNo = userNo,
                        TransNo = trasNo,
                        PurchaseState = PosePurchaseStateType.Purchased.ToString(),
                        PurchaseToken = input.PurchaseToken,
                        OrderId = orderId,
                        RoleType = billingResult.MemberRoleType.ToString(),
                        RoleExpireTime = billingResult.RoleExpireTime,
                        CurrentTime = DateTime.UtcNow,
                    });

                    db_output = P_UPDATE_IN_APP_BILLING.OnQuery();

                    if (P_UPDATE_IN_APP_BILLING.EntityStatus != null || db_output.Result != 0)
                        ErrorHandler.OccurException(RowCode.P_UPDATE_IN_APP_BILLING + db_output.Result);
                }

                // Refrash PoseToken
                billingResult.MemberRoleType.ToString().TryParseEnum(out ServiceRoleType serviceRoleType);
                billingResult.PoseToken = PoseCredentials.CreateToken(userNo, serviceRoleType);
            }

            return new O_E_UPDATE_IN_APP_BILLING_BY_GOOGLE
            {
                BillingResult = billingResult,
            };
        }

        public static async Task<(PoseBillingResult BillingResult, long Payload, string OrderId)> InAppProductProcess(InAppPurchase inAppPurchase, string appPackageName, string purchaseToken)
        {
            ProductPurchase poductPurchase = await _androidPublisher.Purchases.Products.Get(appPackageName, inAppPurchase.ProductId, purchaseToken).ExecuteAsync();

            if (!long.TryParse(poductPurchase.DeveloperPayload, out long payLoad))
                return (null, 0, "");

            PoseBillingResult billingResult = new PoseBillingResult();
            switch (poductPurchase.PurchaseState)
            {
                case 0:
                    billingResult.PurchaseStateType = PosePurchaseStateType.Purchased;
                    break;

                case 1:
                    billingResult.PurchaseStateType = PosePurchaseStateType.Canceled;
                    break;

                case 2:
                    billingResult.PurchaseStateType = PosePurchaseStateType.Pending;
                    break;
            }

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime purchaseTime = origin.AddMilliseconds(poductPurchase.PurchaseTimeMillis ?? 0);

            billingResult.MemberRoleType = inAppPurchase.OfferRoleType;
            billingResult.RoleExpireTime = purchaseTime.AddDays(inAppPurchase.OfferPeriod);
            billingResult.ProductId = inAppPurchase.ProductId;

            return (billingResult, payLoad, poductPurchase.OrderId);
        }

        public static async Task<(PoseBillingResult BillingResult, long Payload, string OrderId)> SubscriptionProcess(InAppPurchase inAppPurchase, string appPackageName, string purchaseToken)
        {
            SubscriptionPurchase subscriptionPurchase = await _androidPublisher.Purchases.Subscriptions.Get(appPackageName, inAppPurchase.ProductId, purchaseToken).ExecuteAsync();

            if (!long.TryParse(subscriptionPurchase.DeveloperPayload, out long payLoad))
                return (null, 0, "");

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime expireTiem = origin.AddMilliseconds(subscriptionPurchase.ExpiryTimeMillis ?? 0);

            PoseBillingResult billingResult = new PoseBillingResult();
            billingResult.PurchaseStateType = ParseSubscriptionPurchaseState(subscriptionPurchase);
            billingResult.MemberRoleType = inAppPurchase.OfferRoleType;
            billingResult.RoleExpireTime = expireTiem;
            billingResult.ProductId = inAppPurchase.ProductId;

            return (billingResult, payLoad, subscriptionPurchase.OrderId);
        }

        public static PosePurchaseStateType ParseSubscriptionPurchaseState(SubscriptionPurchase subscriptionPurchase)
        {
            PosePurchaseStateType result = PosePurchaseStateType.Unknown;
            switch (subscriptionPurchase.PaymentState)
            {
                case 0:
                    result = PosePurchaseStateType.Pending;
                    break;

                case 1:
                    result = PosePurchaseStateType.Purchased;
                    break;

                case 2:
                    result = PosePurchaseStateType.Deferred;
                    break;
            }

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime expireTime = origin.AddMilliseconds(subscriptionPurchase.ExpiryTimeMillis ?? 0);
            DateTime resumeTime = origin.AddMilliseconds(subscriptionPurchase.AutoResumeTimeMillis ?? 0);

            if (result == PosePurchaseStateType.Purchased // 구독 일시정지
                && expireTime < DateTime.UtcNow
                && resumeTime > DateTime.UtcNow)
            {
                result = PosePurchaseStateType.Pause;
            }
            else if (result == PosePurchaseStateType.Pending // 구독 유예기간 (3일), 결제에 문제가 있어서 해결할 시간을 주는것, 멤버십은 유지
                && expireTime > DateTime.UtcNow)
            {
                result = PosePurchaseStateType.Grace;
            }
            else if (subscriptionPurchase?.AutoRenewing == false
                && expireTime < DateTime.UtcNow)
            {
                result = PosePurchaseStateType.Canceled;
            }

            return result;
        }
    }
}