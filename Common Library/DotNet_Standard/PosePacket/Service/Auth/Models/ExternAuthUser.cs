using MessagePack;
using PosePacket.Service.Auth.Models.Enums;
using System;

namespace PosePacket.Service.Auth.Models
{
    [MessagePackObject]
    public class ExternAuthUser
    {
        [Key(0)]
        public string Id { get; set; }

        [Key(1)]
        public string Token { get; set; }

        [Key(2)]
        public string FirstName { get; set; }

        [Key(3)]
        public string LastName { get; set; }

        [Key(4)]
        public string Email { get; set; }

        [Key(5)]
        public string PictureUrl { get; set; }

        [Key(6)]
        public SNSProviderType SNSProvider { get; set; }

        [Key(7)]
        public DateTime ExpiresIn { get; set; }
    }
}