using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class Ids
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

    public class Airs
    {
        [JsonProperty(PropertyName = "day")]
        public string Day { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
    }

    public class Fanart
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }

    public class Poster
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }

    public class Headshot
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }

    public class Screenshot
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }


    public class Logo
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }

    public class Clearart
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }

    public class Banner
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }

    public class Thumb
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }

    public class Avatar
    {

        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }

    public class Images
    {
        [JsonProperty(PropertyName = "fanart")]
        public Fanart Fanart { get; set; }

        [JsonProperty(PropertyName = "poster")]
        public Poster Poster { get; set; }

        [JsonProperty(PropertyName = "logo")]
        public Logo Logo { get; set; }

        [JsonProperty(PropertyName = "clearart")]
        public Clearart Clearart { get; set; }

        [JsonProperty(PropertyName = "banner")]
        public Banner Banner { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public Thumb Thumb { get; set; }

        [JsonProperty(PropertyName = "headshot")]
        public Headshot Headshot { get; set; }

        [JsonProperty(PropertyName = "avatar")]
        public Avatar Avatar { get; set; }

        [JsonProperty(PropertyName = "screenshot")]
        public Screenshot Screenshot { get; set; }

    }

    public class MiniShow
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public Ids Ids { get; set; }

        [JsonProperty(PropertyName = "network")]
        public string Network { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double? Rating { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public int? Votes { get; set; }

        [JsonProperty(PropertyName = "fanart")]
        public Fanart Fanart { get; set; }

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

    public class ShowTrending
    {
        [JsonProperty(PropertyName = "watchers")]
        public int? Watchers { get; set; }

        [JsonProperty(PropertyName = "show")]
        public Show Show { get; set; }
    }

    public class Show
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int? Year { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public Ids Ids { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "first_aired")]
        public string FirstAired { get; set; }
        [JsonProperty(PropertyName = "airs")]

        public Airs Airs { get; set; }
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
        public Images Images { get; set; }

        [JsonProperty(PropertyName = "seasons")]
        public List<Season> Seasons { get; set; }

        [JsonProperty(PropertyName = "user_rating")]
        public int? UserRating { get; set; }
    }

    public class ShowUpdate
    {
        [JsonProperty(PropertyName = "refreshed_at")]
        public string RefreshDate { get; set; }

        [JsonProperty(PropertyName = "show")]
        public Show Show { get; set; }
    }


    public class ShowSearchResult
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "score")]

        public double Score { get; set; }

        [JsonProperty(PropertyName = "show")]
        public Show Show { get; set; }
    }
}
