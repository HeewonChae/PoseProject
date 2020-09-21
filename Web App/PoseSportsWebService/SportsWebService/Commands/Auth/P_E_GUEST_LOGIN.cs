using PosePacket.Service.Auth;
using PosePacket.Service.Enum;
using SportsWebService.Authentication;
using SportsWebService.Infrastructure;
using SportsWebService.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsWebService.Commands.Auth
{
    [WebModelType(OutputType = typeof(O_Login))]
    public static class P_E_GUEST_LOGIN
    {
        public static O_Login Execute()
        {
            return new O_Login
            {
                PoseToken = PoseCredentials.CreateToken(0, ServiceRoleType.Guest),
                TokenExpireIn = PoseCredentials.TOKEN_EXPIRE_IN,
                LastLoginTime = DateTime.MinValue,
                MemberRoleType = MemberRoleType.Guest,
                RoleExpireTime = DateTime.MinValue,
                UserNo = 0,
            };
        }
    }
}