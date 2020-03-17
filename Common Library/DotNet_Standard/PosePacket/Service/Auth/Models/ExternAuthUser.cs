using PosePacket.Service.Auth.Models.Enums;
using System;

namespace PosePacket.Service.Auth.Models
{
    public class ExternAuthUser
    {
        public string Id { get; set; }

        public string Token { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PictureUrl { get; set; }

        public SNSProviderType SNSProvider { get; set; }

        public DateTime ExpiresIn { get; set; }
    }
}