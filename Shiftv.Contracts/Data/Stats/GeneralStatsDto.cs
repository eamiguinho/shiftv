using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Stats
{
    public class GeneralStatsDto
    {
        [JsonProperty(PropertyName = "watchers")]
        public int Watchers { get; set; }
        [JsonProperty(PropertyName = "plays")]
        public int Plays { get; set; }
        [JsonProperty(PropertyName = "scrobbles")]
        public int Scrobbles { get; set; }
        [JsonProperty(PropertyName = "scrobbles_unique")]
        public int ScrobblesUnique { get; set; }
        [JsonProperty(PropertyName = "checkins")]
        public int Checkins { get; set; }
        [JsonProperty(PropertyName = "checkins_unique")]
        public int CheckinsUnique { get; set; }
        [JsonProperty(PropertyName = "collection")]
        public int Collection { get; set; }
        [JsonProperty(PropertyName = "collection_unique")]
        public int CollectionUnique { get; set; }
    }
}