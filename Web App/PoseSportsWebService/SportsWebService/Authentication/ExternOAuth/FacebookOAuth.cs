using PosePacket.Service.Auth;
using System;
using System.Configuration;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication.Providers;
using WebServiceShare.ExternAuthentication.Users;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace SportsWebService.Authentication.ExternOAuth
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

		private FacebookOAuth() : base()
		{
			ClientSecret = ConfigurationManager.AppSettings["Facebook_ClientSecret"];
			Scope = "email";
		}

		#region Implement Abstract Method

		public override async Task<ExternAuthUser> GetUserInfoAsync(string token)
		{
			// Refresh token
			var refreshInfo = await WebClient.RequestAsync<RefreshResponse>(new WebRequestContext()
			{
				MethodType = WebMethodType.GET,
				BaseUrl = RequestTokenUrl,
				QueryParamGroup = "grant_type=fb_exchange_token&client_id={client_id}&client_secret={client_secret}&fb_exchange_token={fb_exchange_token}",
				QueryParamData = new { client_id = ClientId, client_secret = ClientSecret, fb_exchange_token = token },
			},
			false);

			if (refreshInfo == null)
				return null;

			// Get user information
			var facebookUser = await WebClient.RequestAsync<FacebookUser>(new WebRequestContext()
			{
				MethodType = WebMethodType.GET,
				BaseUrl = UserInfoUrl,
				QueryParamGroup = "fields={fields}&access_token={access_token}",
				QueryParamData = new { fields = "email,picture,first_name,last_name", access_token = token },
			},
			false);

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
				ExpiresIn = DateTime.UtcNow.AddSeconds(refreshInfo.ExpiresIn),
			};

			#endregion Implement Abstract Method
		}
	}
}