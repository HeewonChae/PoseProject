using LogicCore.Converter;
using LogicCore.Utility;
using PosePacket;
using PosePacket.Service.Auth;
using PosePacket.Service.Enum;
using SportsWebService.Authentication;
using SportsWebService.Infrastructure;
using SportsWebService.Logics;
using SportsWebService.Models.Enums;
using SportsWebService.Services;
using SportsWebService.Utilities;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SportsWebService.Commands.Auth
{
    using PoseGlobalDB = Repository.Mysql.PoseGlobalDB;

    [WebModelType(InputType = typeof(I_Login), OutputType = typeof(O_Login))]
    public static class P_E_LOGIN
    {
        public static class RowCode
        {
            [Description("Invalid input value")]
            public const int Invalid_InputValue = ServiceErrorCode.WebMethod_Auth.P_E_Login + 1;

            [Description("Invalid platform Id")]
            public const int Invalid_PlatformId = ServiceErrorCode.WebMethod_Auth.P_E_Login + 2;

            [Description("Failed user login")]
            public const int DB_Failed_User_Login = ServiceErrorCode.StoredProcedure_Global.P_E_Login + 1;
        }

        public static O_Login Execute(I_Login input)
        {
            if (input == null)
                ErrorHandler.OccurException(RowCode.Invalid_InputValue);

            if (string.IsNullOrEmpty(input.PlatformId))
                ErrorHandler.OccurException(RowCode.Invalid_PlatformId);

            // Check DB
            PoseGlobalDB.Procedures.P_USER_LOGIN.Output db_output;
            using (var P_USER_LOGIN = new PoseGlobalDB.Procedures.P_USER_LOGIN())
            {
                P_USER_LOGIN.SetInput(input.PlatformId);

                db_output = P_USER_LOGIN.OnQuery();

                if (P_USER_LOGIN.EntityStatus != null || db_output == null)
                    ErrorHandler.OccurException(RowCode.DB_Failed_User_Login);
            }

            db_output.RoleType.TryParseEnum(out ServiceRoleType serviceRoleType);
            db_output.RoleType.TryParseEnum(out MemberRoleType memberRoleType);
            return new O_Login
            {
                PoseToken = PoseCredentials.CreateToken(db_output.UserNo, serviceRoleType),
                TokenExpireIn = PoseCredentials.TOKEN_EXPIRE_IN,
                LastLoginTime = db_output.LastLoginTime,
                MemberRoleType = memberRoleType,
                RoleExpireTime = db_output.RoleExpireTime,
                UserNo = db_output.UserNo,
            };
        }
    }
}