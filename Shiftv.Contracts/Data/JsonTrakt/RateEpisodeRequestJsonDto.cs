using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class RateEpisodeRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "tvdb_id")]
        public int TvDbId { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public string Rating { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public int Episode { get; set; }
    }

    public class RateEpisodesRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public List<RateEpisodeRequestJsonDto> Episodes { get; set; }

    }
}