using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Shows
{
    public class ProgressStatsDto
    {
        [JsonProperty(PropertyName = "percentage")]
        public int Percentage { get; set; }

        [JsonProperty(PropertyName = "aired")]
        public int Aired { get; set; }

        [JsonProperty(PropertyName = "completed")]
        public int Completed { get; set; }

        [JsonProperty(PropertyName = "left")]
        public int Left { get; set; }
    }
}