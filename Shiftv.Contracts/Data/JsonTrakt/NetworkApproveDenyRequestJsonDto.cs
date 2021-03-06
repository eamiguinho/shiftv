﻿using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class NetworkApproveDenyRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        [JsonProperty(PropertyName = "follow_back")]
        public bool FollowBack { get; set; }
    }
}