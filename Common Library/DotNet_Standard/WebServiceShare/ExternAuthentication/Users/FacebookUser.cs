using Newtonsoft.Json;

namespace WebServiceShare.ExternAuthentication.Users
{
    public class FacebookUser
    {
        public class PictureInfo
        {
            public class DataInfo
            {
                [JsonProperty("url")]
                public string Url { get; set; }
            }

            [JsonProperty("data")]
            public DataInfo Data { get; set; }
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("picture")]
        public PictureInfo Picture { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }

    public class RefreshResponse
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}