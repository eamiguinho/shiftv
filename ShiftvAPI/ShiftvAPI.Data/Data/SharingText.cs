using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class SharingText
    {
        [JsonProperty(PropertyName = "watching")]
        public string Watching { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public string Watched { get; set; }
    }
}
