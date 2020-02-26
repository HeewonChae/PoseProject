using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using WebServiceShare.ExternAuthentication.Providers;

namespace Xamarin_Tutorial.Services.Authentication.Providers
{
	public static class OAuthProviderFactory
	{
		public static OAuthBase CreateProvider(SNSProvider provider)
		{
			switch (provider)
			{
				case SNSProvider.Facebook:
					return FacebookOAuth.Instance;
			}

			return null;
		}
	}
}