using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Shows
{
    public class ShowProgressDto
    {
        [JsonProperty(PropertyName = "show")]
        public MiniShowDto Show { get; set; }

        [JsonProperty(PropertyName = "episodes_left")]
        public List<EpisodeDto> EpisodesLeft { get; set; }

        [JsonProperty(PropertyName = "total_episodes")]
        public int? TotalEpisodes { get; set; }
    }
}