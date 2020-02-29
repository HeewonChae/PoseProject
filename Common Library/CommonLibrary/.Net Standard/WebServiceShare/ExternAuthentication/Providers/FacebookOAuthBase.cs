using PosePacket.Service.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceShare.ExternAuthentication.Providers
{
	public abstract class FacebookOAuthBase : OAuthBase
	{
		public FacebookOAuthBase()
		{
			Provider = SNSProviderType.Facebook;
			Description = "Facebook Login Provider";
			ClientId = "842754642909455";
			AuthorizationUrl = "https://www.facebook.com/dialog/oauth";
			RequestTokenUrl = "https://graph.facebook.com/v6.0/oauth/access_token";
			RedirectUrl = "https://www.facebook.com/connect/login_success.html";
			UserInfoUrl = "https://graph.facebook.com/v6.0/me";
		}
	}
}