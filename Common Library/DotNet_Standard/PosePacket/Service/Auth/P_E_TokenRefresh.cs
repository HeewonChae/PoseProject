using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Auth
{
    public class O_TokenRefresh
    {
        public string PoseToken { get; set; }
        public int TokenExpireIn { get; set; }
    }
}