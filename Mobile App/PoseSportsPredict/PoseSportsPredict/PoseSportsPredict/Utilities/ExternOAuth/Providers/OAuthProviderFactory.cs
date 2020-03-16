using PosePacket.Service.Auth;
using PosePacket.Service.Auth.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
            }

            return null;
        }
    }
}