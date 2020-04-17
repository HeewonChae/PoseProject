﻿using PosePacket.Service.Auth.Models;
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

        public override async Task<ExternAuthUser> GetUserInfoAsync(string token)
        {
            // Get user information
            var facebookUser = await _webService.RequestAsync<FacebookUser>(new WebRequestContext()
            {
                SerializeType = SerializeType.Json,
                MethodType = WebMethodType.GET,
                BaseUrl = UserInfoUrl,
                QueryParamGroup = "fields={fields}&access_token={access_token}",
                QueryParamData = new { fields = "email,picture,first_name,last_name", access_token = token },
            });

            if (facebookUser == null)
                return null;

            return new ExternAuthUser
            {
                Id = facebookUser.Id,
                Token = token,
                FirstName = facebookUser.FirstName,
                LastName = facebookUser.LastName,
                Email = facebookUser.Email,
                PictureUrl = facebookUser.Picture.Data.Url,
                SNSProvider = SNSProviderType.Facebook,
            };
        }

        #endregion Implement Abstract Method
    }
}