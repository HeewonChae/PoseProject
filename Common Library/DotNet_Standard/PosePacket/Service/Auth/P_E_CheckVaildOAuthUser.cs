using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;

namespace PosePacket.Service.Auth
{
    public class I_CheckVaildOAuthUser
    {
        public SNSProviderType SNSProvider { get; set; }
        public string AccessToken { get; set; }
    }
}