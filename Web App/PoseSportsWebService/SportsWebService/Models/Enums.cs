using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Models.Enums
{
    public enum ServiceRoleType
    {
        Guest = 0,
        Regular = 1 << 0,
        Promotion = 1 << 7,
        Diamond = 1 << 15,
        VIP = 1 << 22,
        VVIP = 1 << 29,
        Admin = 1 << 30,
    }

    public enum InAppPurchaseType
    {
        InAppProduct,
        Subscription,
    }
}