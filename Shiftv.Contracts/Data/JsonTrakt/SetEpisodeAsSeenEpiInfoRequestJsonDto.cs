using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class SetEpisodeAsSeenEpiInfoRequestJsonDto
    {
        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public int Episode { get; set; }

        [JsonProperty(PropertyName = "last_played")]
        public string LastPlayed { get; set; }
    }
}