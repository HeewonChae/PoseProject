using Google.Apis.Auth;
using LogicCore.Utility;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using SportsWebService.Utilities;
using System;
using System.Configuration;
using System.Threading.Tasks;
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
            //var googleUser = await WebClient.RequestAsync<GoogleUser>(new WebRequestContext()
            //{
            //    MethodType = WebMethodType.GET,
            //    BaseUrl = UserInfoUrl,
            //    QueryParamGroup = "id_token={id_token}",
            //    QueryParamData = new { id_token = token },
            //});

            var validPayload = await GoogleJsonWebSignature.ValidateAsync(token);
            if (validPayload == null)
                return null;

            return new ExternAuthUser
            {
                SNSProvider = SNSProviderType.Google,
                Id = Singleton.Get<CryptoFacade>().SHA_256.ComputeHash(validPayload.Subject),
                Email = validPayload.Email,
                FirstName = validPayload.GivenName,
                LastName = validPayload.FamilyName,
                PictureUrl = validPayload.Picture,
            };

            #endregion Implement Abstract Method
        }
    }
}