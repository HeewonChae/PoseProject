using System;

namespace PosePacket.Service.Auth
{
    public class I_Login
    {
        public string PlatformId { get; set; }
    }

    public class O_Login
    {
        public string PoseToken { get; set; }
        public long TokenExpireIn { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}