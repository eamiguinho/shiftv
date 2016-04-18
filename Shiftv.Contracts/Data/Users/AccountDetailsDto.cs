using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Users
{
    public class AccountDetailsDto
    {
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
        [JsonProperty(PropertyName = "use_24hr")]
        public bool Use24Hr { get; set; }
        [JsonProperty(PropertyName = "protected")]
        public bool IsProtected { get; set; }
    }
}