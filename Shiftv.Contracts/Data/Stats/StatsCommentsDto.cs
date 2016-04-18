using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Stats
{
    public class StatsCommentsDto
    {
        [JsonProperty(PropertyName = "all")]
        public int All { get; set; }

        [JsonProperty(PropertyName = "reviews")]
        public int Reviews { get; set; }

        [JsonProperty(PropertyName = "shouts")]
        public int Shouts { get; set; }
    }
}