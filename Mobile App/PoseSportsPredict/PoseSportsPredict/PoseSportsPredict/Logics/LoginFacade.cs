using PosePacket.Proxy;
using PosePacket.Service.Auth;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Services;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.Logics
{
    public static class LoginFacade
    {
        public static async Task<bool> ExternOAuthLogin()
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();
            var OAuthService = ShinyHost.Resolve<IOAuthService>();

            var loginResult = await webApiService.RequestAsync<O_Login>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_E_LOGIN,
                NeedEncrypt = true,
                PostData = new I_Login
                {
                    PlatformId = OAuthService.AuthenticatedUser.Id,
                }
            });

            if (loginResult == null)
                return false;

            PostLoginProcess(loginResult);

            return true;
        }

        public static async Task<bool> GuestLogin()
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();

            var loginResult = await webApiService.RequestAsync<O_Login>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.GET,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = AuthProxy.ServiceUrl,
                SegmentGroup = AuthProxy.P_E_GUEST_LOGIN,
                NeedEncrypt = true,
            });

            if (loginResult == null)
                return false;

            PostLoginProcess(loginResult);

            return true;
        }

        private static void PostLoginProcess(O_Login loginResult)
        {
            // Update PoseToken, Update ExpireTime
            ClientContext.SetCredentialsFrom(loginResult.PoseToken);
            ClientContext.UserNo = loginResult.UserNo;
            ClientContext.TokenExpireIn = DateTime.UtcNow.AddMilliseconds(loginResult.TokenExpireIn);
            ClientContext.LastLoginTime = loginResult.LastLoginTime.ToLocalTime();

            // Set Membership Information
            var membershipService = ShinyHost.Resolve<MembershipService>();
            membershipService.SetMemberRoleType(loginResult.MemberRoleType);
            membershipService.SetRoleExpireTime(loginResult.RoleExpireTime);
        }
    }
}