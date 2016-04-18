using System;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Users
{
    public class UserTokenDto
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "trakt_access_token")]
        public string TraktAccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int? ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }


        [JsonProperty(PropertyName = "expires_at")]
        public DateTime ExpiresAt { get; set; }

        [JsonProperty(PropertyName = "user_settings")]
        public UserSettingsDto UserSettings { get; set; }
    }
}