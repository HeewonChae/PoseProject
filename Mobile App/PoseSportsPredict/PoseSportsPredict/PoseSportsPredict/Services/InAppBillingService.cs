using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.Services
{
    public class InAppBillingService
    {
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

                //check purchases

                result = await billing.GetProductInfoAsync(itemType, productIds);
            }
            catch (InAppBillingPurchaseException pEx)
            {
                var message = PurchaseExceptionMessage(pEx);
            }
            catch (Exception ex)
            {
                //Something has gone wrong
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return result;
        }

        public async Task<InAppBillingPurchase> PurchaseAsync(string productId, ItemType itemType, string payload, IInAppBillingVerifyPurchase verifyPurchase = null)
        {
            InAppBillingPurchase result = null;
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync(ItemType.InAppPurchase);
                if (!connected)
                    return result;

                //check purchases
                result = await billing.PurchaseAsync(productId, itemType, payload);

                //possibility that a null came through.
                if (result == null)
                {
                    //did not purchase
                }
                else if (result.State == PurchaseState.Purchased)
                {
                    //purchased, we can now consume the item or do it later

                    //If we are on iOS we are done, else try to consume the purchase
                    //Device.RuntimePlatform comes from Xamarin.Forms, you can also use a conditional flag or the DeviceInfo plugin
                    if (Device.RuntimePlatform == Device.iOS)
                        return result;

                    var consumedItem = await CrossInAppBilling.Current.ConsumePurchaseAsync(result.ProductId, result.PurchaseToken);
                    if (consumedItem != null && consumedItem.ConsumptionState == ConsumptionState.Consumed)
                    {
                        //Consumed!!
                    }
                }
            }
            catch (InAppBillingPurchaseException pEx)
            {
                var message = PurchaseExceptionMessage(pEx);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return result;
        }

        private string PurchaseExceptionMessage(InAppBillingPurchaseException ex)
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
            }

            return message;
        }
    }
}