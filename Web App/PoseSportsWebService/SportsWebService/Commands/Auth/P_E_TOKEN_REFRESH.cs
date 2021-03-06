﻿using LogicCore.Utility;
using PosePacket;
using PosePacket.Service.Auth;
using SportsWebService.Authentication;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Services;
using SportsWebService.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SportsWebService.Commands.Auth
{
    [WebModelType(OutputType = typeof(O_TokenRefresh))]
    public static class P_E_TOKEN_REFRESH
    {
        public static class RowCode
        {
            [Description("Invalid credentials")]
            public const int Invalid_Credentials = ServiceErrorCode.WebMethod_Auth.P_E_TokenRefresh + 1;
        }

        public static O_TokenRefresh Execute()
        {
            if (ServerContext.Current.Credentials == PoseCredentials.Default)
                ErrorHandler.OccurException(RowCode.Invalid_Credentials);

            return new O_TokenRefresh
            {
                PoseToken = PoseCredentials.CreateToken(ServerContext.Current.Credentials.UserNo, ServerContext.Current.Credentials.ServiceRoleType),
                TokenExpireIn = PoseCredentials.TOKEN_EXPIRE_IN,
            };
        }
    }
}