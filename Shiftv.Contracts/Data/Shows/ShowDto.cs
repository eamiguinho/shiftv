using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Data.Movies;

namespace Shiftv.Contracts.Data.Shows
{
    public class IdsDto
    {
        [JsonProperty(PropertyName = "trakt")]
        public int? TraktId { get; set; }

        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }

        [JsonProperty(PropertyName = "tvdb")]
        public int? TvDbId { get; set; }

        [JsonProperty(PropertyName = "imdb")]
        public string ImdbId { get; set; }

        [JsonProperty(PropertyName = "tmdb")]
        public int? TmDbId { get; set; }

        [JsonProperty(PropertyName = "tvrage")]
        public int? TvRageId { get; set; }
    }

    public class AirsDto
    {
        [JsonProperty(PropertyName = "day")]
        public string Day { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
    }

    public class ShowDto
    {
        //    [JsonProperty(PropertyName = "title")]
        //    public string Title { get; set; }

        //    [JsonProperty(PropertyName = "year")]
        //    public int Year { get; set; }

        //    [JsonProperty(PropertyName = "url")]
        //    public string Url { get; set; }

        //    [JsonProperty(PropertyName = "first_aired")]
        //    public int FirstAired { get; set; }

        //    [JsonProperty(PropertyName = "first_aired_iso")]
        //    public string FirstAiredIso { get; set; }

        //    [JsonProperty(PropertyName = "first_aired_utc")]
        //    public int FirstAiredUtc { get; set; }

        //    [JsonProperty(PropertyName = "country")]
        //    public string Country { get; set; }

        //    [JsonProperty(PropertyName = "overview")]
        //    public string Overview { get; set; }

        //    [JsonProperty(PropertyName = "runtime")]
        //    public int Runtime { get; set; }

        //    [JsonProperty(PropertyName = "status")]
        //    public string Status { get; set; }

        //    [JsonProperty(PropertyName = "network")]
        //    public string Network { get; set; }

        //    [JsonProperty(PropertyName = "air_day")]
        //    public string AirDay { get; set; }

        //    [JsonProperty(PropertyName = "air_day_utc")]
        //    public string AirDayUtc { get; set; }

        //    [JsonProperty(PropertyName = "air_time")]
        //    public string AirTime { get; set; }

        //    [JsonProperty(PropertyName = "air_time_utc")]
        //    public string AirTimeUtc { get; set; }

        //    [JsonProperty(PropertyName = "certification")]
        //    public string Certification { get; set; }

        //    [JsonProperty(PropertyName = "imdb_id")]
        //    public string ImdbId { get; set; }

        //    [JsonProperty(PropertyName = "tvdb_id")]
        //    public int TvDbId { get; set; }

        //    [JsonProperty(PropertyName = "tvrage_id")]
        //    public int? TvRageId { get; set; }

        //    [JsonProperty(PropertyName = "last_updated")]
        //    public int LastUpdated { get; set; }

        //    [JsonProperty(PropertyName = "poster")]
        //    public string Poster { get; set; }

        //    [JsonProperty(PropertyName = "images", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //    public ImageDto Image { get; set; }

        //    [JsonProperty(PropertyName = "top_watchers", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //    public List<UserProfileDto> TopWatchers { get; set; }

        //    [JsonProperty(PropertyName = "top_episodes", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //    public List<EpisodeDto> TopEpisodes { get; set; }

        //    [JsonProperty(PropertyName = "ratings", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //    public RatingDto Rating { get; set; }

        //    [JsonProperty(PropertyName = "stats", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //    public GeneralStatsDto GeneralStats { get; set; }

        //    [JsonProperty(PropertyName = "people", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //    public PeopleDto People { get; set; }

        //    [JsonProperty(PropertyName = "genres")]
        //    public List<string> Genres { get; set; }

        //    [JsonProperty(PropertyName = "seasons", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //    public List<SeasonDto> Seasons { get; set; }

        //    [JsonProperty(PropertyName = "plays")]
        //    public int Plays { get; set; }
        //    [JsonProperty(PropertyName = "watched")]
        //    public bool Watched { get; set; }

        //    [JsonProperty(PropertyName = "in_watchlist")]
        //    public bool InWatchlist { get; set; }

        //    [JsonProperty(PropertyName = "rating")]
        //    public string UserRating { get; set; }

        //    [JsonProperty(PropertyName = "rating_advanced")]
        //    public bool RatingAdvanced { get; set; }

        //    public IEpisode GetNextEpisode()
        //    {
        //        if (Seasons == null) return null;
        //        var season = Seasons.First();
        //        var list =
        //            season.Episodes.Where(x => x.FirstAiredUtcToDate != null && x.FirstAiredUtcToDate.Value > DateTime.Now)
        //                .ToList();
        //        if (list.Count > 0) return EpisodeDtoFactory.Create(list.First(), Title);
        //        return season.Episodes.All(x => x.FirstAiredUtcToDate != null) ? EpisodeDtoFactory.Create(season.Episodes.Last(), Title) : EpisodeDtoFactory.Create(season.Episodes.First(), Title);
        //    }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int? Year { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "first_aired")]
        public string FirstAired { get; set; }
        [JsonProperty(PropertyName = "airs")]

        public AirsDto Airs { get; set; }
        [JsonProperty(PropertyName = "runtime")]

        public int? Runtime { get; set; }
        [JsonProperty(PropertyName = "certification")]

        public string Certification { get; set; }
        [JsonProperty(PropertyName = "network")]

        public string Network { get; set; }
        [JsonProperty(PropertyName = "country")]

        public string Country { get; set; }
        [JsonProperty(PropertyName = "trailer")]

        public string Trailer { get; set; }
        [JsonProperty(PropertyName = "homepage")]

        public string Homepage { get; set; }
        [JsonProperty(PropertyName = "status")]

        public string Status { get; set; }

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

        [JsonProperty(PropertyName = "aired_episodes")]
        public int? AiredEpisodes { get; set; }

        [JsonProperty(PropertyName = "images")]
        public ImageDto Images { get; set; }

        [JsonProperty(PropertyName = "seasons")]
        public List<SeasonDto> Seasons { get; set; }

        [JsonProperty(PropertyName = "user_rating")]
        public int? UserRating { get; set; }

    }


    public class MiniShowDto
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }

        [JsonProperty(PropertyName = "network")]
        public string Network { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double? Rating { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int? Votes { get; set; }

        [JsonProperty(PropertyName = "fanart")]
        public FanartDto Fanart { get; set; }

        [JsonProperty(PropertyName = "first_aired")]
        public string FirstAired { get; set; }

        [JsonProperty(PropertyName = "user_rating")]
        public int? UserRating { get; set; }

        [JsonProperty(PropertyName = "year")]

        public int? Year { get; set; }

        [JsonProperty(PropertyName = "genres")]
        public List<string> Genres { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }


    public class ListShowsResult
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "privacy")]
        public string Privacy { get; set; }
        [JsonProperty(PropertyName = "show_numbers")]
        public bool ShowNumbers { get; set; }
        [JsonProperty(PropertyName = "allow_shouts")]
        public bool AllowShouts { get; set; }
        [JsonProperty(PropertyName = "items")]
        public List<ListShowItemsResult> Items { get; set; }


    }
    public class ListShowItemsResult
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        //[JsonProperty(PropertyName = "show")]

        //public ShowDto Show { get; set; }

        [JsonProperty(PropertyName = "movie")]
        public MovieDto Movie { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public bool Watched { get; set; }

        [JsonProperty(PropertyName = "in_collection")]
        public bool InCollection { get; set; }

        [JsonProperty(PropertyName = "in_watchlist")]
        public bool InWatchlist { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public string Rating { get; set; }

        [JsonProperty(PropertyName = "rating_advanced")]
        public bool RatingAdvanced { get; set; }



    }

    public class ShiftvUserStatsDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "isGold")]
        public bool IsGold { get; set; }

        [JsonProperty(PropertyName = "isSilver")]
        public bool IsSilver { get; set; }
    }
}
