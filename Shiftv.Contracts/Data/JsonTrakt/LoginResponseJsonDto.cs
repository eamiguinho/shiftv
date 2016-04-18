using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class LoginResponseJsonDto
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}