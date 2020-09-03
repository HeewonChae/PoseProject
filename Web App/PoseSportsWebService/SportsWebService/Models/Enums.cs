using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Models.Enums
{
    public enum ServiceRoleType
    {
        _NONE_,
        Regular = 1 << 0,
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