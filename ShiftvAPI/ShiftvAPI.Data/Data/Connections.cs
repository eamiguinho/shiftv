using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class Connections
    {
        [JsonProperty(PropertyName = "facebook")]
        public bool Facebook { get; set; }

        [JsonProperty(PropertyName = "twitter")]
        public bool Twitter { get; set; }

        [JsonProperty(PropertyName = "google")]
        public bool Google { get; set; }

        [JsonProperty(PropertyName = "tumblr")]
        public bool Tumblr { get; set; }
    }
}