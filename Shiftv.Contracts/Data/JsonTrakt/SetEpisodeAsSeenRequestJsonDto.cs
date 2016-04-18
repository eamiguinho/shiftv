using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class SetEpisodeAsSeenRequestJsonDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "imdb_id")]
        public string ImdbId { get; set; }

        [JsonProperty(PropertyName = "tvdb_id")]
        public int TvDbId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public List<SetEpisodeAsSeenEpiInfoRequestJsonDto> Episodes { get; set; }
    }  
    
  
}