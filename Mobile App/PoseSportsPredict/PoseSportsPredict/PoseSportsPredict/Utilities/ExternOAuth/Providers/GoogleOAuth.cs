using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using PoseSportsPredict.InfraStructure;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication.Providers;
using WebServiceShare.ExternAuthentication.Users;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace PoseSportsPredict.Utilities.ExternOAuth.Providers
{
    public sealed class GoogleOAuth : GoogleOAuthBase
    {
        private static readonly Lazy<GoogleOAuth> lazy = new Lazy<GoogleOAuth>(() => new GoogleOAuth(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static GoogleOAuth Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private IWebApiService _webService;

        private GoogleOAuth() : base()
        {
            this.Scope = "profile email";
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