using Newtonsoft.Json;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.Users
{
    public class UserProfileWatchedDto
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public long Watched { get; set; }

        [JsonProperty(PropertyName = "show")]
        public ShowDto Show { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public EpisodeDto Episode { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public MovieDto Movie { get; set; }
    }
}