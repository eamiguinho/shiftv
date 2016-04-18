using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class UserSettings
    {
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }

        [JsonProperty(PropertyName = "account")]
        public Account Account { get; set; }

        [JsonProperty(PropertyName = "connections")]
        public Connections Connections { get; set; }

        [JsonProperty(PropertyName = "sharing_text")]
        public SharingText SharingText { get; set; }
    }
}