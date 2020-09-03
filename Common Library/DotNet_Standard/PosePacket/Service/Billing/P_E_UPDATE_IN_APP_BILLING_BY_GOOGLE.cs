using MessagePack;
using PosePacket.Service.Billing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Billing
{
    [MessagePackObject]
    public class I_E_UPDATE_IN_APP_BILLING_BY_GOOGLE
    {
        [Key(0)]
        public string ProductID { get; set; }

        [Key(1)]
        public string AppPackageName { get; set; }

        [Key(2)]
        public string PurchaseToken { get; set; }
    }

    [MessagePackObject]
    public class O_E_UPDATE_IN_APP_BILLING_BY_GOOGLE
    {
        [Key(0)]
        public PoseBillingResult BillingResult { get; set; }
    }
}