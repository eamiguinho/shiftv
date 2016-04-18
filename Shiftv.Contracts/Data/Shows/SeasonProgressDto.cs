using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Shows
{
    public class SeasonProgressDto
    {
        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "percentage")]
        public int Percentage { get; set; }

        [JsonProperty(PropertyName = "aired")]
        public int Aired { get; set; }

        [JsonProperty(PropertyName = "completed")]
        public int Completed { get; set; }

        [JsonProperty(PropertyName = "left")]
        public int Left { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public EpisodesProgressDto Episodes { get; set; }
    }
}