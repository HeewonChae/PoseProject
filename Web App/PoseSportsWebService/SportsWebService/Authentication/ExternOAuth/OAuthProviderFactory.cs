using PosePacket.Service.Auth;
using WebServiceShare.ExternAuthentication.Providers;

namespace SportsWebService.Authentication.ExternOAuth
{
	public static class OAuthProviderFactory
	{
		public static OAuthBase CreateProvider(SNSProviderType provider)
		{
			switch (provider)
			{
				case SNSProviderType.Facebook:
					return FacebookOAuth.Instance;
			}

			return null;
		}
	}
}