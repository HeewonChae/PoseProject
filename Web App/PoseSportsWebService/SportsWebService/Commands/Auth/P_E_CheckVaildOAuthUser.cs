using PosePacket;
using PosePacket.Service.Auth;
using SportsWebService.Authentication;
using SportsWebService.Authentication.ExternOAuth;
using SportsWebService.Infrastructure;
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

            [Description("Invalid AccessToken")]
            public const int Invalid_AccessToken = ServiceErrorCode.WebMethod_Auth.P_E_CheckVaildOAuthUser + 2;

            [Description("Failed OAuth")]
            public const int Failed_OAuth = ServiceErrorCode.WebMethod_Auth.P_E_CheckVaildOAuthUser + 3;

            [Description("Failed Save DB")]
            public const int Failed_Save_DB = ServiceErrorCode.WebMethod_Auth.P_E_CheckVaildOAuthUser + 4;
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
                    PlatformType = (short)externAuthUser.SNSProvider,
                    RoleType = (int)ServiceRoleType.User,
                    InsertTime = DateTime.UtcNow,
                });

                bool queryResult = await P_INSERT_USER_BASE.OnQueryAsync();

                if (P_INSERT_USER_BASE.EntityStatus != null || !queryResult)
                    ErrorHandler.OccurException(RowCode.Failed_Save_DB);
            }

            return externAuthUser;
        }
    }
}