using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Billing
{
    [MessagePackObject]
    public class I_E_INSERT_IN_APP_BILLING_BY_GOOGLE
    {
        [Key(1)]
        public string ProductID { get; set; }
    }

    [MessagePackObject]
    public class O_E_INSERT_IN_APP_BILLING_BY_GOOGLE
    {
        [Key(0)]
        public string BillingPayload { get; set; }
    }
}