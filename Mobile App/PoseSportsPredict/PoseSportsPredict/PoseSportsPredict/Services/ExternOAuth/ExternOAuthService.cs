using Acr.UserDialogs;
using PosePacket.Service.Auth;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Services.ExternOAuth.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using Xamarin.Auth;

namespace PoseSportsPredict.Services.ExternOAuth
{
	public class ExternOAuthService : IOAuthService
	{
		private bool _isAuthenticated;
		private ExternAuthUser _authenticatedUser;

		public bool IsAuthenticated => _isAuthenticated;
		public ExternAuthUser AuthenticatedUser => _authenticatedUser;

		public async Task OAuthLoginAsync(SNSProvider provider)
		{
			try
			{
				if (WebApiService.CheckInternetConnection())
					_isAuthenticated = false;

				var oAuth = OAuthProviderFactory.CreateProvider(provider);
				var authenticator = new OAuth2Authenticator(
					oAuth.ClientId,
					oAuth.Scope,
					new Uri(oAuth.AuthorizationUrl),
					new Uri(oAuth.RedirectUrl));

				// Completed
				authenticator.Completed += async (sender, eventArgs) =>
				{
					_isAuthenticated = eventArgs.IsAuthenticated;

					if (IsAuthenticated)
					{
						string token = eventArgs.Account.Properties["access_token"];
						_authenticatedUser = await oAuth.GetUserInfoAsync(token);

						await UserDialogs.Instance.AlertAsync("OAuth Completed");
					}
				};

				// Error
				authenticator.Error += async (sender, eventArgs) =>
				{
					_isAuthenticated = false;
					await UserDialogs.Instance.AlertAsync($"OAuth error: {eventArgs.Message}");
				};

				var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
				presenter.Login(authenticator);

				// 로그인 폼 닫힘
				presenter.Completed += (sender, eventArgs) =>
				{
				};
			}
			catch (Exception ex)
			{
				await UserDialogs.Instance.AlertAsync($"OAuth exception: {ex.Message}");
			}
		}

		public Task<bool> IsAuthenticatedAndValid()
		{
			throw new NotImplementedException();
		}

		public void Logout()
		{
			_isAuthenticated = false;
		}
	}
}