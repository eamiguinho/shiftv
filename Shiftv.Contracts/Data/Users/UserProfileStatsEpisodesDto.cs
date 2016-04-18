using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Users
{
    public class UserProfileStatsEpisodesDto
    {
        [JsonProperty(PropertyName = "watched")]
        public int Watched { get; set; }

        [JsonProperty(PropertyName = "scrobbles")]
        public int Scrobbles { get; set; }

        [JsonProperty(PropertyName = "checkins")]
        public int Checkins { get; set; }

        [JsonProperty(PropertyName = "seen")]
        public int Seen { get; set; }

        [JsonProperty(PropertyName = "shouts")]
        public int Shouts { get; set; }

        [JsonProperty(PropertyName = "loved")]
        public int Loved { get; set; }

        [JsonProperty(PropertyName = "hated")]
        public int Hated { get; set; }
    }
}