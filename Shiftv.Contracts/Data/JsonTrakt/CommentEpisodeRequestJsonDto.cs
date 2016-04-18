using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class CommentEpisodeRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "tvdb_id")]
        public int TvDbId { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public int Episode { get; set; }

        [JsonProperty(PropertyName = "spoiler")]
        public bool Spoiler { get; set; }

        [JsonProperty(PropertyName = "review")]
        public bool Review { get; set; }
    }
}