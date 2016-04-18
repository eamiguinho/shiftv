using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data.PostObjects
{
    public class UploadWatched
    {
        [JsonProperty(PropertyName = "episodes")]
        public List<EpisodePostRequestJson> Episodes { get; set; }
              [JsonProperty(PropertyName = "movies")]
        public List<MovieRequestWatchedJson> Movies { get; set; } 
    }
}