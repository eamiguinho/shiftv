using System;
using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.JsonTrakt
{
    public class SetMovieInfoRequestJsonDto
    {
        [JsonProperty(PropertyName = "imdb_id")]
        public string ImdbId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "last_played")]
        public string LastPlayed { get; set; }
    }


    public class SetAsSeenJsonDto
    {
        [JsonProperty(PropertyName = "movie")]
        public MovieRequestJsonDto Movie { get; set; }

        [JsonProperty(PropertyName = "show")]
        public ShowRequestJsonDto Show { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public EpisodeRequestJsonDto Episode { get; set; }  

        [JsonProperty(PropertyName = "watched_at")]
        public DateTime WatchedAt { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public bool Watched { get; set; }
    }
}