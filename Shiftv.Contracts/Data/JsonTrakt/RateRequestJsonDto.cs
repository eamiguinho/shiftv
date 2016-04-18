using Newtonsoft.Json;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class RateRequestJsonDto
    {
        [JsonProperty(PropertyName = "movie")]
        public MovieRequestJsonDto Movie { get; set; }

        [JsonProperty(PropertyName = "show")]
        public ShowRequestJsonDto Show { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public EpisodeRequestJsonDto Episode { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public int Rating { get; set; }

    }
}