using PosePacket.Service.Auth.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceShare.ExternAuthentication.Providers
{
    public abstract class GoogleOAuthBase : OAuthBase
    {
        public GoogleOAuthBase()
        {
            Provider = SNSProviderType.Google;
            Description = "Google Login Provider";
            ClientId = "217594391151-3abvvp6gbboo7mrt9bc5o3uef2epv27v.apps.googleusercontent.com";
            AuthorizationUrl = "https://accounts.google.com/o/oauth2/v2/auth";
            RequestTokenUrl = "https://oauth2.googleapis.com/token";
            RedirectUrl = "com.pose.poseidon.picks:/oauth2redirect";
            UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";
        }
    }
}