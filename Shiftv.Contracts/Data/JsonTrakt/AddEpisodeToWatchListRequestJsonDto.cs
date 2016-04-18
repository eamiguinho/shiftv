using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class AddEpisodeToWatchListRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "imdb_id")]
        public string ImdbId { get; set; }

        [JsonProperty(PropertyName = "tvdb_id")]
        public int TvdbId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public List<EpisodeData> Episodes { get; set; }
    }

    public class EpisodeData
    {
        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public int Episode { get; set; }
    }
}