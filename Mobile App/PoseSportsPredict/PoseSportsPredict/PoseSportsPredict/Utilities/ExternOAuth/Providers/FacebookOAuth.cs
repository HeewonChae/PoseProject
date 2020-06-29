using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using PoseSportsPredict.InfraStructure;
using Shiny;
using System;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication.Providers;
using WebServiceShare.ExternAuthentication.Users;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.Utilities.ExternOAuth.Providers
{
    public sealed class FacebookOAuth : FacebookOAuthBase
    {
        private static readonly Lazy<FacebookOAuth> lazy = new Lazy<FacebookOAuth>(() => new FacebookOAuth(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static FacebookOAuth Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private IWebApiService _webService;

        private FacebookOAuth() : base()
        {
            this.Scope = "email";
            _webService = ShinyHost.Resolve<IWebApiService>();
        }

        #region Implement Abstract Method

        public override Task<ExternAuthUser> GetUserInfoAsync(string token)
        {
            return null;
        }

        #endregion Implement Abstract Method
    }
}