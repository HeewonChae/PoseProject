using Plugin.InAppBilling.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace PoseSportsPredict.Models
{
    public class InAppPurchaseInfo
    {
        public string ProductId { get; set; }
        public ItemType PurchaseType { get; set; }
        public string Title { get; set; }
        public Color TitleColor { get; set; }
        public string ImageResource { get; set; }
        public Color ImageColor { get; set; }
        public string[] Advantages { get; set; }
        public string ButtonText { get; set; }
        public bool IsActivated { get; set; }
        public bool IsAvailable { get; set; }
    }
}