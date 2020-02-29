using PosePacket.Service.Auth;
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

namespace PoseSportsPredict.Logic.ExternOAuth.Providers
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
				ExpiresIn = DateTime.UtcNow.Add(new TimeSpan(6, 0, 0)), // TODO: 임시 ExpiresIn
			};
		}

		#endregion Implement Abstract Method
	}
}