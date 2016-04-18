using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data.PostObjects
{
    public class UploadRating
    {
        [JsonProperty(PropertyName = "movies")]
        public List<MovieRequestRateJson> Movies { get; set; }

       [JsonProperty(PropertyName = "episodes")]
        public List<EpisodePostRateRequestJson> Episodes { get; set; }

        [JsonProperty(PropertyName = "shows")]
       public List<ShowRateRequestJson> Shows { get; set; }
    }
}