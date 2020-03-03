﻿using LogicCore.Utility;
using PosePacket;
using PosePacket.Service.Auth;
using SportsWebService.Authentication;
using SportsWebService.Infrastructure;
using SportsWebService.Utilities;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using PoseGlobalDB = Repository.Mysql.PoseGlobalDB;

namespace SportsWebService.Commands.Auth
{
    [WebModelType(InputType = typeof(I_Login), OutputType = typeof(O_Login))]
    public static class P_E_Login
    {
        public static class RowCode
        {
            [Description("Invalid input value")]
            public const int Invalid_InputValue = ServiceErrorCode.WebMethod_Auth.P_E_Login + 1;

            [Description("Invalid platform Id")]
            public const int Invalid_PlatformId = ServiceErrorCode.WebMethod_Auth.P_E_Login + 2;

            [Description("Failed user login")]
            public const int Failed_User_Login = ServiceErrorCode.WebMethod_Auth.P_E_Login + 3;
        }

        public async static Task<O_Login> Execute(I_Login input)
        {
            if (input == null)
                ErrorHandler.OccurException(RowCode.Invalid_InputValue);

            if (string.IsNullOrEmpty(input.PlatformId))
                ErrorHandler.OccurException(RowCode.Invalid_PlatformId);

            // Check DB
            PoseGlobalDB.Tables.UserBase db_output;
            using (var P_USER_LOGIN = new PoseGlobalDB.Procedures.P_USER_LOGIN())
            {
                P_USER_LOGIN.SetInput(input.PlatformId);

                db_output = await P_USER_LOGIN.OnQueryAsync();

                if (P_USER_LOGIN.EntityStatus != null || db_output == null)
                    ErrorHandler.OccurException(RowCode.Failed_User_Login);
            }

            var credentials = new PoseCredentials();
            credentials.SetUserNo(db_output.user_no);
            credentials.SetServiceRoleType(db_output.role_type);
            credentials.RefreshExpireTime();

            byte[] eCredential = Singleton.Get<CryptoFacade>().Encrypt_RSA(PoseCredentials.Serialize(credentials));

            return new O_Login
            {
                PoseToken = Convert.ToBase64String(eCredential),
                TokenExpireIn = PoseCredentials.TOKEN_EXPIRE_IN,
            };
        }
    }
}