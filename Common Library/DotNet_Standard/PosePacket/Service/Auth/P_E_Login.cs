using MessagePack;
using System;

namespace PosePacket.Service.Auth
{
    [MessagePackObject]
    public class I_Login
    {
        [Key(0)]
        public string PlatformId { get; set; }
    }

    [MessagePackObject]
    public class O_Login
    {
        [Key(0)]
        public byte[] PoseToken { get; set; }

        [Key(1)]
        public long TokenExpireIn { get; set; }

        [Key(2)]
        public DateTime LastLoginTime { get; set; }
    }
}