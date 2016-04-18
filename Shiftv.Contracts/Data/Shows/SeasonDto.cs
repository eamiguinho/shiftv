using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Images;

namespace Shiftv.Contracts.Data.Shows
{
    public class SeasonDto
    {
        //[JsonProperty(PropertyName = "season")]
        //public int Number { get; set; }
        //[JsonProperty(PropertyName = "episodes")]
        //public List<EpisodeDto> Episodes { get; set; }
        //[JsonProperty(PropertyName = "url")]
        //public string Url { get; set; }
        //[JsonProperty(PropertyName = "poster")]
        //public string Poster { get; set; }
        //[JsonProperty(PropertyName = "images", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //public ImageDto Image { get; set; }
        [JsonProperty(PropertyName = "number")]
        public int? Number { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double? Rating { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int? Votes { get; set; }

        [JsonProperty(PropertyName = "episode_count")]
        public int? EpisodeCount { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "images")]
        public ImageDto Images { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public List<EpisodeDto> Episodes { get; set; }
    }

}
