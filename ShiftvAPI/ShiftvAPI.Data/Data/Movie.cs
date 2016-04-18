using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{

    public class MiniMovie
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public Ids Ids { get; set; }
        [JsonProperty(PropertyName = "runtime")]
        public int? Runtime { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double? Rating { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int? Votes { get; set; }

        [JsonProperty(PropertyName = "genres")]
        public List<string> Genres { get; set; }


        [JsonProperty(PropertyName = "fanart")]
        public Fanart Fanart { get; set; }

        [JsonProperty(PropertyName = "released")]
        public string Released { get; set; }

        [JsonProperty(PropertyName = "user_rating")]
        public int? UserRating { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public bool Watched { get; set; }
         [JsonProperty(PropertyName = "in_watchlist")]
        public bool InWatchlist { get; set; }
    }

    public class MovieTrending
    {
        [JsonProperty(PropertyName = "watchers")]
        public int? Watchers { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public Movie Movie { get; set; }
    }

    public class Movie
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int? Year { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public Ids Ids { get; set; }

        [JsonProperty(PropertyName = "tagline")]
        public string Tagline { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "released")]
        public string Released { get; set; }

        [JsonProperty(PropertyName = "runtime")]
        public int? Runtime { get; set; }

        [JsonProperty(PropertyName = "trailer")]
        public string Trailer { get; set; }

        [JsonProperty(PropertyName = "homepage")]
        public string Homepage { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double? Rating { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int? Votes { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "available_translations")]
        public List<string> AvailableTranslations { get; set; }

        [JsonProperty(PropertyName = "genres")]
        public List<string> Genres { get; set; }

        [JsonProperty(PropertyName = "certification")]
        public string Certification { get; set; }

        [JsonProperty(PropertyName = "images")]
        public Images Images { get; set; }

        [JsonProperty(PropertyName = "user_rating")]
        public int? UserRating { get; set; }
        [JsonProperty(PropertyName = "watched")]

        public bool Watched { get; set; }

        [JsonProperty(PropertyName = "in_watchlist")]
        public bool InWatchlist { get; set; }
    }

    public class MovieUpdate
    {
        [JsonProperty(PropertyName = "refreshed_at")]
        public string RefreshDate { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public Movie Movie { get; set; }
    }

    public class MovieSearchResult
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "score")]

        public double Score { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public Movie Movie { get; set; }
    }
}
