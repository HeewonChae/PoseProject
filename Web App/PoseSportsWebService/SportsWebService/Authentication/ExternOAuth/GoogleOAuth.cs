using Flurl.Http;
using PoseCrypto;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WebServiceShare.ExternAuthentication.Providers;
using WebServiceShare.ExternAuthentication.Users;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace SportsWebService.Authentication.ExternOAuth
{
    public class GoogleOAuth : GoogleOAuthBase
    {
        private static readonly Lazy<GoogleOAuth> lazy = new Lazy<GoogleOAuth>(() => new GoogleOAuth(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static GoogleOAuth Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private GoogleOAuth() : base()
        {
            ClientSecret = ConfigurationManager.AppSettings["Google_ClientSecret"];
        }

        #region Implement Abstract Method

        public override async Task<ExternAuthUser> GetUserInfoAsync(string token)
        {
            // Get user information
            var googleUser = await WebClient.RequestAsync<GoogleUser>(new WebRequestContext()
            {
                MethodType = WebMethodType.GET,
                BaseUrl = UserInfoUrl,
                QueryParamGroup = "access_token={access_token}",
                QueryParamData = new { access_token = token },
            });

            if (googleUser == null)
                return null;

            return new ExternAuthUser
            {
                SNSProvider = SNSProviderType.Google,
                Id = CryptoFacade.Instance.SHA_256.ComputeHash(googleUser.Id),
                Email = googleUser.Email,
                FirstName = googleUser.FirstName,
                LastName = googleUser.LastName,
                PictureUrl = googleUser.Picture,
            };

            #endregion Implement Abstract Method
        }
    }
}