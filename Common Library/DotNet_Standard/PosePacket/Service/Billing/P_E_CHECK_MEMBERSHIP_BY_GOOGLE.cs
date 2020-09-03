using MessagePack;
using PosePacket.Service.Billing.Models;
using PosePacket.Service.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Billing
{
    [MessagePackObject]
    public class I_E_CHECK_MEMBERSHIP_BY_GOOGLE
    {
        [Key(0)]
        public string AppPackageName { get; set; }
    }

    [MessagePackObject]
    public class O_E_CHECK_MEMBERSHIP_BY_GOOGLE
    {
        [Key(0)]
        public PoseBillingResult BillingResult { get; set; }
    }
}