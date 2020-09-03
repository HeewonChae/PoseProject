using MessagePack;
using PosePacket.Service.Billing.Models.Enums;
using PosePacket.Service.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PosePacket.Service.Billing.Models
{
    [MessagePackObject]
    public class PoseBillingResult
    {
        [Key(0)]
        public PosePurchaseStateType PurchaseStateType { get; set; }

        [Key(1)]
        public PosePurchaseErrorType PurchaseErrorType { get; set; }

        [Key(2)]
        public MemberRoleType MemberRoleType { get; set; }

        [Key(3)]
        public DateTime RoleExpireTime { get; set; }

        [Key(4)]
        public string ProductId { get; set; }

        [Key(5)]
        public byte[] PoseToken { get; set; }
    }
}