using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceShare.ExternAuthentication.Users
{
    public class GoogleUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("given_name")]
        public string FirstName { get; set; }

        [JsonProperty("family_name")]
        public string LastName { get; set; }
    }
}