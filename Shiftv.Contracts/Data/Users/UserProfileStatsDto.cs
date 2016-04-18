using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Users
{
    public class UserProfileStatsDto
    {
        [JsonProperty(PropertyName = "friends")]
        public int Friends { get; set; }

        [JsonProperty(PropertyName = "shows")]
        public UserProfileStatsShowsDto Shows { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public UserProfileStatsEpisodesDto Episodes { get; set; }

        [JsonProperty(PropertyName = "movies")]
        public UserProfileStatsMoviesDto Movies { get; set; }
    }
}