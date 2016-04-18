using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class Watched
    {
        [JsonProperty(PropertyName = "plays")]
        public int Plays { get; set; }

        [JsonProperty(PropertyName = "last_watched_at")]
        public string LastWatchedAt { get; set; }

        [JsonProperty(PropertyName = "show")]
        public Show Show { get; set; }

        [JsonProperty(PropertyName = "seasons")]
        public List<Season> Seasons { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public Movie Movie { get; set; }
    }
}
