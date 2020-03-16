using PosePacket.Service.Auth;
using PosePacket.Service.Auth.Models;
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