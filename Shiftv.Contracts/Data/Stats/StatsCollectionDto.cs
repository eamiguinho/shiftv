using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Stats
{
    public class StatsCollectionDto
    {
        [JsonProperty(PropertyName = "all")]
        public int All { get; set; }

        [JsonProperty(PropertyName = "users")]
        public int Users { get; set; }
    }
}