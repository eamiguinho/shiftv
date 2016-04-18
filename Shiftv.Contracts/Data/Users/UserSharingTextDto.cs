using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Users
{
    public class UserSharingTextDto
    {
        [JsonProperty(PropertyName = "watching")]
        public string Watching { get; set; }
        [JsonProperty(PropertyName = "watched")]
        public string Watched { get; set; }
    }
}