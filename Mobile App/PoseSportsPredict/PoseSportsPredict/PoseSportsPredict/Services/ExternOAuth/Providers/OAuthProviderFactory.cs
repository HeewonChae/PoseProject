using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using WebServiceShare.ExternAuthentication.Providers;

namespace PoseSportsPredict.Services.ExternOAuth.Providers
{
	public class OAuthProviderFactory
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