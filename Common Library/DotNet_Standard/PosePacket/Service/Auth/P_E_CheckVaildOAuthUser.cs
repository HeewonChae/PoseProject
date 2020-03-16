using PosePacket.Service.Auth.Models;

namespace PosePacket.Service.Auth
{
    public class I_CheckVaildOAuthUser
    {
        public SNSProviderType SNSProvider { get; set; }
        public string AccessToken { get; set; }
    }
}