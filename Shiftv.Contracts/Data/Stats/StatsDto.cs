using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Stats
{
    public class StatsDto
    {
        [JsonProperty(PropertyName = "ratings")]
        public RatingDto Ratings { get; set; }
        [JsonProperty(PropertyName = "watchers")]
        public int Watchers { get; set; }
        [JsonProperty(PropertyName = "plays")]
        public int Plays { get; set; }
        [JsonProperty(PropertyName = "scrobbles")]
        public StatsScrobblesDto Scrobbles { get; set; }
        [JsonProperty(PropertyName = "checkins")]
        public StatsCheckinsDto Checkins { get; set; }
        [JsonProperty(PropertyName = "collection")]
        public StatsCollectionDto Collection { get; set; }
        [JsonProperty(PropertyName = "lists")]
        public StatsListsDto Lists { get; set; }
        [JsonProperty(PropertyName = "comments")]
        public StatsCommentsDto Comments { get; set; } 
    }

    public class ServerTimeDto
    {
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
    }
}
