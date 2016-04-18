using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.Movies
{
    public class MovieDto
    {
        //[JsonProperty(PropertyName = "title")]
        //public string Title { get; set; }
        //[JsonProperty(PropertyName = "year")]
        //public int Year { get; set; }

        //[JsonProperty(PropertyName = "released")]
        //public int Released { get; set; }

        //[JsonProperty(PropertyName = "url")]
        //public string Url { get; set; }
        //[JsonProperty(PropertyName = "trailer")]
        //public string Trailer { get; set; }
        //[JsonProperty(PropertyName = "runtime")]
        //public int Runtime { get; set; }
        //[JsonProperty(PropertyName = "tagline")]
        //public string Tagline { get; set; }
        //[JsonProperty(PropertyName = "overview")]
        //public string Overview { get; set; }
        //[JsonProperty(PropertyName = "certification")]
        //public string Certification { get; set; }
        //[JsonProperty(PropertyName = "imdb_id")]
        //public string ImdbId { get; set; }
        //[JsonProperty(PropertyName = "tmdb_id")]
        //public int TmdbId { get; set; }
        //[JsonProperty(PropertyName = "rt_id")]
        //public int RtId { get; set; }
        //[JsonProperty(PropertyName = "last_updated")]
        //public int LastUpdated { get; set; }
        //[JsonProperty(PropertyName = "poster")]
        //public string Poster { get; set; }

        //[JsonProperty(PropertyName = "images")]
        //public ImageDto Image { get; set; }

        //[JsonProperty(PropertyName = "top_watchers")]
        //public List<UserProfileDto> TopWatchers { get; set; }

        //[JsonProperty(PropertyName = "ratings")]
        //public RatingDto Ratings { get; set; }

        //[JsonProperty(PropertyName = "stats")]
        //public GeneralStatsDto Stats { get; set; }

        //[JsonProperty(PropertyName = "people")]
        //public PeopleDto People { get; set; }
        //[JsonProperty(PropertyName = "genres")]
        //public List<string> Genres { get; set; }

        //[JsonProperty(PropertyName = "in_watchlist")]
        //public bool InWatchlist { get; set; }
        //[JsonProperty(PropertyName = "rating")]
        //public string UserRating { get; set; }
        //[JsonProperty(PropertyName = "watched")]
        //public bool Watched { get; set; }
        //[JsonProperty(PropertyName = "rating_advanced")]
        //public bool RatingAdvanced { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int? Year { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }

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
        public ImageDto Images { get; set; }

        [JsonProperty(PropertyName = "user_rating")]
        public int? UserRating { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public bool Watched { get; set; }
        [JsonProperty(PropertyName = "in_watchlist")]
        public bool InWatchlist { get; set; }
    }


    public class MiniMovieDto
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }
        [JsonProperty(PropertyName = "runtime")]
        public int? Runtime { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double? Rating { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int? Votes { get; set; }

        [JsonProperty(PropertyName = "genres")]
        public List<string> Genres { get; set; }


        [JsonProperty(PropertyName = "fanart")]
        public FanartDto Fanart { get; set; }

        [JsonProperty(PropertyName = "released")]
        public string Released { get; set; }

        [JsonProperty(PropertyName = "user_rating")]
        public int? UserRating { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public bool Watched { get; set; }
        [JsonProperty(PropertyName = "in_watchlist")]
        public bool InWatchlist { get; set; }
    }
}
