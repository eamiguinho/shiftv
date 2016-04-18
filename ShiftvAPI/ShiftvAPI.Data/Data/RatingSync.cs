using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class RatingSync
    {
        [JsonProperty(PropertyName = "rated_at")]
        public string RatedAt { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public int? Rating { get; set; }

        [JsonProperty(PropertyName = "show")]
        public Show Show { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public Movie Movie { get; set; }

        [JsonProperty(PropertyName = "season")]
        public Season Season { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public Episode Episode { get; set; }    
    }
}
