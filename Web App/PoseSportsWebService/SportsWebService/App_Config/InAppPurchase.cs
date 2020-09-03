using LogicCore.Debug;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PosePacket.Service.Enum;
using SportsWebService.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.App_Config
{
    public class InAppPurchase
    {
        public string ProductId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InAppPurchaseType PurchaseType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public StoreType StoreType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public MemberRoleType OfferRoleType { get; set; }

        public int OfferPeriod { get; set; }
        public string Description { get; set; }

        public InAppPurchase()
        {
        }

        public static Dictionary<string, InAppPurchase> InAppPurchases { get; } = new Dictionary<string, InAppPurchase>();

        public static void PostProcess(params InAppPurchase[] purchases)
        {
            foreach (var purchase in purchases)
            {
                Dev.Assert(!InAppPurchases.ContainsKey(purchase.ProductId), $"Alread exist key in InAppProducts key: {purchase.ProductId}");

                InAppPurchases.Add(purchase.ProductId, purchase);
            }
        }

        public static bool TryGetInAppPurchase(string productId, out InAppPurchase product)
        {
            return InAppPurchases.TryGetValue(productId, out product); ;
        }
    }
}