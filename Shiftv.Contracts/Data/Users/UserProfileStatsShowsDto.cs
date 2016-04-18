using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Users
{
    public class UserProfileStatsShowsDto
    {
        [JsonProperty(PropertyName = "library")]
        public int Library { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public int Watched { get; set; }

        [JsonProperty(PropertyName = "collection")]
        public int Collection { get; set; }

        [JsonProperty(PropertyName = "shouts")]
        public int Shouts { get; set; }

        [JsonProperty(PropertyName = "loved")]
        public int Loved { get; set; }

        [JsonProperty(PropertyName = "hated")]
        public int Hated { get; set; }
    }
}