using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Shows
{
    public class EpisodeDto
    {
        //[JsonProperty(PropertyName = "season")]
        //public int Season { get; set; }
        //[JsonProperty(PropertyName = "number")]
        //public int Number { get; set; }
        //[JsonProperty(PropertyName = "tvdb_id")]
        //public int TvDbId { get; set; }
        //[JsonProperty(PropertyName = "imdb_id")]
        //public string ImdbId { get; set; }
        //[JsonProperty(PropertyName = "title")]
        //public string Title { get; set; }
        //[JsonProperty(PropertyName = "overview")]
        //public string Overview { get; set; }
        //[JsonProperty(PropertyName = "url")]
        //public string Url { get; set; }
        //[JsonProperty(PropertyName = "first_aired")]
        //public int FirstAired { get; set; }
        //[JsonProperty(PropertyName = "first_aired_iso")]
        //public string FirstAiredIso { get; set; }
        //[JsonProperty(PropertyName = "first_aired_utc")]
        //public long FirstAiredUtc { get; set; }
        //[JsonProperty(PropertyName = "images")]
        //public ImageDto Image { get; set; }

        //[JsonProperty(PropertyName = "ratings")]
        //public RatingDto Rating { get; set; }
        //public DateTime? FirstAiredUtcToDate
        //{
        //    get
        //    {
        //        if (FirstAiredIso != null) return DateTime.Parse(FirstAiredIso).ToUniversalTime(); else return null;
        //    }
        //}

        //[JsonProperty(PropertyName = "screen")]
        //public string Screen { get; set; }

        //[JsonProperty(PropertyName = "watched")]
        //public bool Watched { get; set; }

        //[JsonProperty(PropertyName = "in_collection")]
        //public bool InCollection { get; set; }

        //[JsonProperty(PropertyName = "in_watchlist")]
        //public bool InWatchlist { get; set; }

        //[JsonProperty(PropertyName = "rating")]
        //public string UserRating { get; set; }

        //[JsonProperty(PropertyName = "rating_advanced")]
        //public int UserRatingAdvanced { get; set; }
        //[JsonProperty(PropertyName = "show_name")]
        //public string ShowName { get; set; }
        
        //[JsonProperty(PropertyName = "plays")]
        //public int Plays { get; set; }

        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }

        [JsonProperty(PropertyName = "number_abs")]
        public int? NumberAbs { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "first_aired")]
        public string FirstAired { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double? Rating { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int? Votes { get; set; }

        [JsonProperty(PropertyName = "available_translations")]
        public List<string> AvailableTranslations { get; set; }

        [JsonProperty(PropertyName = "images")]
        public ImageDto Images { get; set; }

        [JsonProperty(PropertyName = "user_rating")]
        public int? UserRating { get; set; }   
        
        [JsonProperty(PropertyName = "watched")]
        public bool Watched { get; set; }
    }

    public class FullEpisodeDto
    {
        [JsonProperty(PropertyName = "show")]
        public ShowDto Show { get; set; }
        [JsonProperty(PropertyName = "episode")]
        public EpisodeDto Episode { get; set; }
    }


}