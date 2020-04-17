using MessagePack;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Auth.Models.Enums;

namespace PosePacket.Service.Auth
{
    [MessagePackObject]
    public class I_CheckVaildOAuthUser
    {
        [Key(0)]
        public SNSProviderType SNSProvider { get; set; }

        [Key(1)]
        public string AccessToken { get; set; }
    }
}