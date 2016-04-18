using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class CreateUserRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }      
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}