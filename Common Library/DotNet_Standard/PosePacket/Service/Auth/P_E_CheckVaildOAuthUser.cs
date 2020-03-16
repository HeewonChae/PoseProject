using PosePacket.Service.Auth.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosePacket.Service.Auth
{
    public class I_CheckVaildOAuthUser
    {
        public SNSProviderType SNSProvider { get; set; }
        public string AccessToken { get; set; }
    }
}