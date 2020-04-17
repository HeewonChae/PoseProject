using MessagePack;

namespace PosePacket.Service.Auth
{
    [MessagePackObject]
    public class O_TokenRefresh
    {
        [Key(0)]
        public byte[] PoseToken { get; set; }

        [Key(1)]
        public long TokenExpireIn { get; set; }
    }
}