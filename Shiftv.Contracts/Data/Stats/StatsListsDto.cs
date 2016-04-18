using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Stats
{
    public class StatsListsDto
    {
        [JsonProperty(PropertyName = "all")]
        public int All { get; set; }
        [JsonProperty(PropertyName = "watchlist")]
        public int Watchlist { get; set; }
        [JsonProperty(PropertyName = "custom")]
        public int Custom { get; set; }
    }
}