using PosePacket;
using PosePacket.Service.Auth;
using PosePacket.Service.Auth.Models;
using SportsWebService.Authentication;
using SportsWebService.Authentication.ExternOAuth;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Models.Enums;
using SportsWebService.Services;
using SportsWebService.Utilities;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

using PoseGlobalDB = Repository.Mysql.PoseGlobalDB;

namespace SportsWebService.Commands.Auth
{
    [WebModelType(InputType = typeof(I_CheckVaildOAuthUser), OutputType = typeof(ExternAuthUser))]
    public static class P_E_CheckVaildOAuthUser
    {
        public static class RowCode
        {
            [Description("Invalid input value")]
            public const int Invalid_InputValue = ServiceErrorCode.WebMethod_Auth.P_E_CheckVaildOAuthUser + 1;

            [Description("Invalid access token")]
            public const int Invalid_AccessToken = ServiceErrorCode.WebMethod_Auth.P_E_CheckVaildOAuthUser + 2;

            [Description("Failed OAuth")]
            public const int Failed_OAuth = ServiceErrorCode.WebMethod_Auth.P_E_CheckVaildOAuthUser + 3;

            [Description("Failed save database")]
            public const int DB_Failed_Save = ServiceErrorCode.StoredProcedure_Global.P_E_CheckVaildOAuthUser + 1;
        }

        public static async Task<ExternAuthUser> Execute(I_CheckVaildOAuthUser input)
        {
            if (input == null)
                ErrorHandler.OccurException(RowCode.Invalid_InputValue);

            if (string.IsNullOrEmpty(input.AccessToken))
                ErrorHandler.OccurException(RowCode.Invalid_AccessToken);

            var oAuth = OAuthProviderFactory.CreateProvider(input.SNSProvider);
            var externAuthUser = await oAuth.GetUserInfoAsync(input.AccessToken);

            if (externAuthUser == null)
                ErrorHandler.OccurException(RowCode.Failed_OAuth);

            // Check DB
            using (var P_INSERT_USER_BASE = new PoseGlobalDB.Procedures.P_INSERT_USER_BASE())
            {
                P_INSERT_USER_BASE.SetInput(new PoseGlobalDB.Procedures.P_INSERT_USER_BASE.Input
                {
                    PlatformId = externAuthUser.Id,
                    PlatformType = externAuthUser.SNSProvider.ToString(),
                    PlatformEmail = externAuthUser.Email,
                    RoleType = ServiceRoleType.Regular.ToString(), // default 회원등급
                    CurrentTime = DateTime.UtcNow,
                });

                int queryResult = P_INSERT_USER_BASE.OnQuery();

                if (P_INSERT_USER_BASE.EntityStatus != null || queryResult != 0)
                    ErrorHandler.OccurException(RowCode.DB_Failed_Save);
            }

            return externAuthUser;
        }
    }
}