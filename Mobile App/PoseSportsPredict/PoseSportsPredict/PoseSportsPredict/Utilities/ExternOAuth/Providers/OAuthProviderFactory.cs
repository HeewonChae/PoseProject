using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;
using WebServiceShare.ExternAuthentication.Providers;

namespace PoseSportsPredict.Utilities.ExternOAuth.Providers
{
    public static class OAuthProviderFactory
    {
        public static OAuthBase CreateProvider(SNSProviderType provider)
        {
            switch (provider)
            {
                case SNSProviderType.Facebook:
                    return FacebookOAuth.Instance;

                case SNSProviderType.Google:
                    return GoogleOAuth.Instance;
            }

            return null;
        }
    }
}