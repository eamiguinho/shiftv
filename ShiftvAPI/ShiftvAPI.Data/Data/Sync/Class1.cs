using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data.Sync
{
    public class SyncMovieStats
    {
        [JsonProperty(PropertyName = "watched_at")]
        public string WatchedAt { get; set; }

        [JsonProperty(PropertyName = "collected_at")]
        public string CollectedAt { get; set; }

        [JsonProperty(PropertyName = "rated_at")]
        public string RatedAt { get; set; }

        [JsonProperty(PropertyName = "watchlisted_at")]
        public string WatchlistedAt { get; set; }

        [JsonProperty(PropertyName = "commented_at")]
        public string CommentedAt { get; set; }

        [JsonProperty(PropertyName = "paused_at")]
        public string PausedAt { get; set; }
    }

    public class SyncEpisodesStats
    {
        [JsonProperty(PropertyName = "watched_at")]
        public string WatchedAt { get; set; }

        [JsonProperty(PropertyName = "collected_at")]
        public string CollectedAt { get; set; }

        [JsonProperty(PropertyName = "rated_at")]
        public string RatedAt { get; set; }

        [JsonProperty(PropertyName = "watchlisted_at")]
        public string WatchlistedAt { get; set; }

        [JsonProperty(PropertyName = "commented_at")]
        public string CommentedAt { get; set; }

        [JsonProperty(PropertyName = "paused_at")]
        public string PausedAt { get; set; }
    }

    public class SyncShowsStats
    {
        [JsonProperty(PropertyName = "rated_at")]
        public string RatedAt { get; set; }

        [JsonProperty(PropertyName = "watchlisted_at")]
        public string WatchlistedAt { get; set; }

        [JsonProperty(PropertyName = "commented_at")]
        public string CommentedAt { get; set; }
    }

    public class SyncSeasonsStats
    {
        [JsonProperty(PropertyName = "rated_at")]
        public string RatedAt { get; set; }

        [JsonProperty(PropertyName = "watchlisted_at")]
        public string WatchlistedAt { get; set; }

        [JsonProperty(PropertyName = "commented_at")]
        public string CommentedAt { get; set; }
    }

    public class SyncCommentsStats
    {
        [JsonProperty(PropertyName = "liked_at")]
        public string LikedAt { get; set; }
    }

    public class SyncListsStats
    {
        [JsonProperty(PropertyName = "liked_at")]
        public string LikedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public string UpdatedAt { get; set; }
    }

    public class SyncStats
    {
        [JsonProperty(PropertyName = "all")]
        public string All { get; set; }

        [JsonProperty(PropertyName = "movies")]
        public SyncMovieStats Movies { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public SyncEpisodesStats Episodes { get; set; }

        [JsonProperty(PropertyName = "shows")]
        public SyncShowsStats Shows { get; set; }

        [JsonProperty(PropertyName = "seasons")]
        public SyncSeasonsStats Seasons { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public SyncCommentsStats Comments { get; set; }

        [JsonProperty(PropertyName = "lists")]
        public SyncListsStats Lists { get; set; }
    }

    public class SyncWatched
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

    public class WatchedEpisodes
    {
        public int TraktShowId { get; set; }
        public int EpisodeNumber { get; set; }
        public int SeasonNumber { get; set; }
    }

    public class WatchedMovie
    {
        public int TraktId { get; set; }
    }
}
