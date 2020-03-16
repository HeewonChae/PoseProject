using PosePacket.Service.Auth.Models;
using System.Threading.Tasks;

namespace WebServiceShare.ExternAuthentication.Providers
{
    public abstract class OAuthBase
    {
        public SNSProviderType Provider { get; protected set; }
        public string Description { get; protected set; }
        public string ClientId { get; protected set; }
        public string ClientSecret { get; protected set; }
        public string Scope { get; protected set; }
        public string AuthorizationUrl { get; protected set; }
        public string RedirectUrl { get; protected set; }
        public string RequestTokenUrl { get; protected set; }
        public string UserInfoUrl { get; protected set; }

        public abstract Task<ExternAuthUser> GetUserInfoAsync(string token);
    }
}