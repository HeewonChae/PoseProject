using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare;
using WebServiceShare.ExternAuthentication.Providers;
using WebServiceShare.ExternAuthentication.Users;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;

namespace Xamarin_Tutorial.Services.Authentication.Providers
{
	public class FacebookOAuth : FacebookOAuthBase
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
		}

		#region Implement Abstract Method

		public override async Task<ExternAuthUser> GetUserInfoAsync(string token)
		{
			// Refresh token
			var refreshInfo = await WebClient.RequestAsync<RefreshResponse>(new RequestContext()
			{
				MethodType = WebConfig.WebMethodType.GET,
				ServiceUrl = RequestTokenUrl,
				QueryParamGroup = "grant_type=fb_exchange_token&client_id={client_id}&client_secret={client_secret}&fb_exchange_token={fb_exchange_token}",
				QueryParamData = new { client_id = ClientId, client_secret = ClientSecret, fb_exchange_token = token },
			},
			false);

			if (refreshInfo == null)
				return null;

			// Get user information
			var facebookUser = await WebClient.RequestAsync<FacebookUser>(new RequestContext()
			{
				MethodType = WebConfig.WebMethodType.GET,
				ServiceUrl = UserInfoUrl,
				QueryParamGroup = "fields={fields}&access_token={access_token}",
				QueryParamData = new { fields = "email,picture,first_name,last_name", access_token = token },
			},
			false);

			if (facebookUser == null)
				return null;

			return new ExternAuthUser
			{
				Id = facebookUser.Id,
				Token = refreshInfo.Token,
				FirstName = facebookUser.FirstName,
				LastName = facebookUser.LastName,
				Email = facebookUser.Email,
				PictureUrl = facebookUser.Picture.Data.Url,
				Provider = SNSProvider.Facebook,
				ExpiresIn = DateTime.UtcNow.Add(new TimeSpan(0, 0, refreshInfo.ExpiresIn)),
			};
		}

		#endregion Implement Abstract Method
	}
}