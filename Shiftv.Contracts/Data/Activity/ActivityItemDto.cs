using Newtonsoft.Json;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;

namespace Shiftv.Contracts.Data.Activity
{
    public class ActivityItemDto
    {
        [JsonProperty(PropertyName = "timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty(PropertyName = "when")]
        public ActivityWhenDto When { get; set; }

        [JsonProperty(PropertyName = "elapsed")]
        public ActivityElapsedDto Elapsed { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        [JsonProperty(PropertyName = "user")]
        public UserProfileDto User { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public EpisodeDto Episode { get; set; }

        [JsonProperty(PropertyName = "show")]
        public ShowDto Show { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public MovieDto Movie { get; set; }

        [JsonProperty(PropertyName = "Rating")]
        public string Rating { get; set; }

        [JsonProperty(PropertyName = "rating_advanced")]
        public int? RatingAdvanced { get; set; }

        [JsonProperty(PropertyName = "use_rating_advanced")]
        public bool? IsRatingAdvanced { get; set; }
    }
}