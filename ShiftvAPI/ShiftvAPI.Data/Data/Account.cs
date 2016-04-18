using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class Account
    {
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }

        [JsonProperty(PropertyName = "CoverImage")]
        public string CoverImage { get; set; }
    }
}