using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class LoginRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}