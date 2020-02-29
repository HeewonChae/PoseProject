using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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