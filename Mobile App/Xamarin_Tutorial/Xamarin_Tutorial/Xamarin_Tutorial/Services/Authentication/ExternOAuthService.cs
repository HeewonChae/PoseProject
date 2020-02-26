using Acr.UserDialogs;
using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ExternAuthentication.Users;
using Xamarin.Auth;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Services.Authentication.Providers;
using Xamarin_Tutorial.Utilities;

namespace Xamarin_Tutorial.Services.Authentication
{
	public class ExternOAuthService : IAuthenticationService, Singleton.INode
	{
		private bool _isAuthenticated;
		private ExternAuthUser _authenticatedUser;

		public bool IsAuthenticated => _isAuthenticated;
		public ExternAuthUser AuthenticatedUser => _authenticatedUser;

		public async void OAuthLoginAsync(SNSProvider provider)
		{
			try
			{
				if (!ApiService.CheckInternetConnected())
				{
					await UserDialogs.Instance.AlertAsync("Please check internet connection");
				}

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

						LocalStorage.Storage.AddOrUpdateValue(StorageKey.ExternAuthUserInfo, _authenticatedUser);

						Singleton.Get<ViewLocator>().Login.PoseLoginAsync();
					}
				};

				// Error
				authenticator.Error += async (sender, eventArgs) =>
				{
					_isAuthenticated = false;
					await UserDialogs.Instance.AlertAsync($"Authentication error: {eventArgs.Message}");
				};

				var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
				presenter.Login(authenticator);

				// 로그인 폼 닫힘
				presenter.Completed += (sender, eventArgs) =>
				{
					UserDialogs.Instance.HideLoading();
				};
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Login Error : " + ex.Message);
			}
		}

		public async Task<bool> IsAuthenticatedAndValid()
		{
			var authUserInfo = LocalStorage.Storage.GetValueOrDefault<ExternAuthUser>(StorageKey.ExternAuthUserInfo);
			if (authUserInfo == null)
			{
				_isAuthenticated = false;
				return false;
			}

			// 기간 만료
			if (authUserInfo.ExpiresIn < DateTime.UtcNow)
			{
				_isAuthenticated = false;
				return false;
			}

			var oAuth = OAuthProviderFactory.CreateProvider(authUserInfo.Provider);
			_authenticatedUser = await oAuth.GetUserInfoAsync(authUserInfo.Token);
			// 유효하지 않은 토큰
			if (_authenticatedUser == null)
			{
				_isAuthenticated = false;
				return false;
			}

			_isAuthenticated = true;
			return true;
		}

		public void Logout()
		{
			_isAuthenticated = false;
			LocalStorage.Storage.Remove(StorageKey.ExternAuthUserInfo);
		}
	}
}