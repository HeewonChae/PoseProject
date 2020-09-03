using Acr.UserDialogs;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using PosePacket.Proxy;
using PosePacket.Service.Billing;
using PosePacket.Service.Billing.Models;
using PosePacket.Service.Billing.Models.Enums;
using PosePacket.Service.Enum;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Resources;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.Services
{
    public class InAppBillingService
    {
        public InAppBillingProduct[] InAppProduct;
        public InAppBillingProduct[] SubscriptionProduct;

        public async void InitializeProduct()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                InAppProduct = (await GetProductInfoAsync(ItemType.InAppPurchase, AppConfig.ANDROID_PRODUCT_IDS[0])).ToArray();
                SubscriptionProduct = (await GetProductInfoAsync(ItemType.Subscription, AppConfig.ANDROID_PRODUCT_IDS[1])).ToArray();
            }
        }

        public async Task<IEnumerable<InAppBillingProduct>> GetProductInfoAsync(ItemType itemType, params string[] productIds)
        {
            IEnumerable<InAppBillingProduct> result = null;

            if (!CrossInAppBilling.IsSupported)
                return result;

            var billing = CrossInAppBilling.Current;
            try
            {
                //You must connect
                var connected = await billing.ConnectAsync(itemType);

                if (!connected)
                    return result;

                result = await billing.GetProductInfoAsync(itemType, productIds);
            }
            catch (InAppBillingPurchaseException)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return result;
        }

        public async Task<InAppBillingPurchase> GetPurchasesFirstOrDefaultAsync(ItemType itemType)
        {
            InAppBillingPurchase result = null;
            if (!CrossInAppBilling.IsSupported)
                return result;

            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync(itemType);
                if (!connected)
                    return result;

                var purchaseItems = await billing.GetPurchasesAsync(itemType);

                result = purchaseItems?.FirstOrDefault();
            }
            catch (InAppBillingPurchaseException)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return result;
        }

        public async Task<PoseBillingResult> PurchaseAsync(string productId, ItemType itemType)
        {
            PoseBillingResult billingResult = null;
            var billing = CrossInAppBilling.Current;
            var webApiService = ShinyHost.Resolve<IWebApiService>();
            var deviceInfoHelper = DependencyService.Resolve<IDeviceInfoHelper>();

            try
            {
                var connected = await billing.ConnectAsync(itemType);
                if (!connected)
                    return new PoseBillingResult { PurchaseErrorType = PosePurchaseErrorType.FailStoreConnect };

                // 서버에서 payload 얻어오기
                var server_payload = await webApiService.RequestAsyncWithToken<O_E_INSERT_IN_APP_BILLING_BY_GOOGLE>(new WebRequestContext
                {
                    SerializeType = SerializeType.MessagePack,
                    MethodType = WebMethodType.POST,
                    BaseUrl = AppConfig.PoseWebBaseUrl,
                    ServiceUrl = BillingProxy.ServiceUrl,
                    SegmentGroup = BillingProxy.P_E_INSERT_IN_APP_BILLING_BY_GOOGLE,
                    NeedEncrypt = true,
                    PostData = new I_E_INSERT_IN_APP_BILLING_BY_GOOGLE
                    {
                        ProductID = productId,
                    }
                });

                if (server_payload == null)
                    return new PoseBillingResult { PurchaseErrorType = PosePurchaseErrorType.ServerError };

                //check purchases
                var googlePurchaseResult = await billing.PurchaseAsync(productId, itemType, server_payload.BillingPayload);

                if (googlePurchaseResult == null)
                    return new PoseBillingResult { PurchaseErrorType = PosePurchaseErrorType.FailStoreConnect };

                // 서버 인증
                using (UserDialogs.Instance.Loading("Loading..."))
                {
                    var server_billingResult = await webApiService.RequestAsyncWithToken<O_E_UPDATE_IN_APP_BILLING_BY_GOOGLE>(new WebRequestContext
                    {
                        SerializeType = SerializeType.MessagePack,
                        MethodType = WebMethodType.POST,
                        BaseUrl = AppConfig.PoseWebBaseUrl,
                        ServiceUrl = BillingProxy.ServiceUrl,
                        SegmentGroup = BillingProxy.P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE,
                        NeedEncrypt = true,
                        PostData = new I_E_UPDATE_IN_APP_BILLING_BY_GOOGLE
                        {
                            AppPackageName = deviceInfoHelper.AppPackageName,
                            ProductID = googlePurchaseResult.ProductId,
                            PurchaseToken = googlePurchaseResult.PurchaseToken,
                        }
                    });

                    billingResult = server_billingResult?.BillingResult ?? new PoseBillingResult { PurchaseErrorType = PosePurchaseErrorType.SuccessPurchaseButServerError };

                    //If we are on iOS we are done, else try to consume the purchase
                    //Device.RuntimePlatform comes from Xamarin.Forms, you can also use a conditional flag or the DeviceInfo plugin
                    //if (Device.RuntimePlatform == Device.iOS)
                    //    return result;

                    // 소비성 상품은 컨슘 처리
                    //if (itemType == ItemType.InAppPurchase
                    //    && billingResult.PurchaseStateType == PosePurchaseStateType.Purchased)
                    //    await CrossInAppBilling.Current.ConsumePurchaseAsync(googlePurchaseResult.ProductId, googlePurchaseResult.PurchaseToken);
                }
            }
            catch (InAppBillingPurchaseException pEx)
            {
                billingResult = new PoseBillingResult();

                if (pEx.PurchaseError == PurchaseError.AlreadyOwned)
                    billingResult.PurchaseErrorType = PosePurchaseErrorType.AlreadyOwned;
            }
            catch (Exception ex)
            {
                billingResult = new PoseBillingResult();
                billingResult.PurchaseErrorType = PosePurchaseErrorType.UnknownError;
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return billingResult;
        }

        /// <summary>
        /// true면 컨슘안된 아이템 없음
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public async Task<bool> RestorePurchasedItem()
        {
            var billing = CrossInAppBilling.Current;
            var webApiService = ShinyHost.Resolve<IWebApiService>();
            var membershipService = ShinyHost.Resolve<MembershipService>();
            var deviceInfoHelper = DependencyService.Resolve<IDeviceInfoHelper>();

            bool returnValue = true;
            try
            {
                var connected = await billing.ConnectAsync(ItemType.InAppPurchase);
                if (!connected)
                    return true;

                //check purchases
                // 소비성 상품
                var inAppItems = await billing.GetPurchasesAsync(ItemType.InAppPurchase);
                if (inAppItems != null)
                {
                    foreach (var item in inAppItems)
                    {
                        // 서버 구매 인증
                        var server_billingResult = await webApiService.RequestAsyncWithToken<O_E_UPDATE_IN_APP_BILLING_BY_GOOGLE>(new WebRequestContext
                        {
                            SerializeType = SerializeType.MessagePack,
                            MethodType = WebMethodType.POST,
                            BaseUrl = AppConfig.PoseWebBaseUrl,
                            ServiceUrl = BillingProxy.ServiceUrl,
                            SegmentGroup = BillingProxy.P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE,
                            NeedEncrypt = true,
                            PostData = new I_E_UPDATE_IN_APP_BILLING_BY_GOOGLE
                            {
                                AppPackageName = deviceInfoHelper.AppPackageName,
                                ProductID = item.ProductId,
                                PurchaseToken = item.PurchaseToken,
                            }
                        });

                        if (server_billingResult == null)
                            returnValue = false;

                        if (server_billingResult.BillingResult.PurchaseStateType == PosePurchaseStateType.Purchased)
                        {
                            await CrossInAppBilling.Current.ConsumePurchaseAsync(item.ProductId, item.PurchaseToken);

                            // MemberShipService 셋팅
                            ClientContext.SetCredentialsFrom(server_billingResult.BillingResult.PoseToken);
                            membershipService.SetMemberRoleType(server_billingResult.BillingResult.MemberRoleType);
                            membershipService.SetRoleExpireTime(server_billingResult.BillingResult.RoleExpireTime);
                        }
                    }
                }

                // 구독 상품
                if (membershipService.MemberRoleType < MemberRoleType.VIP)
                {
                    var subscribeItems = await billing.GetPurchasesAsync(ItemType.Subscription);
                    if (subscribeItems != null)
                    {
                        foreach (var item in subscribeItems)
                        {
                            // 서버 구매 인증
                            var server_billingResult = await webApiService.RequestAsyncWithToken<O_E_UPDATE_IN_APP_BILLING_BY_GOOGLE>(new WebRequestContext
                            {
                                SerializeType = SerializeType.MessagePack,
                                MethodType = WebMethodType.POST,
                                BaseUrl = AppConfig.PoseWebBaseUrl,
                                ServiceUrl = BillingProxy.ServiceUrl,
                                SegmentGroup = BillingProxy.P_E_UPDATE_IN_APP_BILLING_BY_GOOGLE,
                                NeedEncrypt = true,
                                PostData = new I_E_UPDATE_IN_APP_BILLING_BY_GOOGLE
                                {
                                    AppPackageName = deviceInfoHelper.AppPackageName,
                                    ProductID = item.ProductId,
                                    PurchaseToken = item.PurchaseToken,
                                }
                            });

                            if (server_billingResult == null)
                                returnValue = false;

                            // 정상 구매 됐으면
                            if (server_billingResult.BillingResult.PurchaseStateType == PosePurchaseStateType.Purchased)
                            {
                                // MemberShipService 셋팅
                                ClientContext.SetCredentialsFrom(server_billingResult.BillingResult.PoseToken);
                                membershipService.SetMemberRoleType(server_billingResult.BillingResult.MemberRoleType);
                                membershipService.SetRoleExpireTime(server_billingResult.BillingResult.RoleExpireTime);
                            }
                        }
                    }
                }
            }
            catch (InAppBillingPurchaseException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return returnValue;
        }

        private string ConvertToPosePurchaseStateType(InAppBillingPurchaseException ex)
        {
            var message = string.Empty;
            switch (ex.PurchaseError)
            {
                case PurchaseError.AppStoreUnavailable:
                    message = "Currently the app store seems to be unavailble. Try again later.";
                    break;

                case PurchaseError.BillingUnavailable:
                    message = "Billing seems to be unavailable, please try again later.";
                    break;

                case PurchaseError.PaymentInvalid:
                    message = "Payment seems to be invalid, please try again.";
                    break;

                case PurchaseError.PaymentNotAllowed:
                    message = "Payment does not seem to be enabled/allowed, please try again.";
                    break;

                case PurchaseError.UserCancelled:
                    message = "UserCancelled";
                    break;

                case PurchaseError.AlreadyOwned:
                    message = "AlreadyOwned";
                    break;

                default:
                    message = "에러가 발생했습니다. 잠시 후 다시 시도해 주세요";
                    break;
            }

            return message;
        }
    }
}