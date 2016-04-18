using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Data.Sync;

namespace ShiftvAPI.Contracts.Infrastucture.Shiftv
{
    public interface ISyncShiftvDataService
    {
        List<UserRating> GetShowRatingsByUser(string token);
        UserRating GetShowRatingByUser(string token, Show showData);
        List<UserRating> GetMovieRatingsByUser(string token);
        UserRating GetMovieRatingByUser(string token, Movie showData);
        List<UserRating> GetEpisodeRatingsByUser(string token);
        void SaveRatingsShowsMovies(List<RatingSync> ratingSyncs, string token, RequestType requestType);
        void SaveRatingsEpisodes(List<RatingSync> ratingSyncs, string token, RequestType requestType);
        void SaveWatchedEpisode(List<SyncWatched> seasons, string token, List<SetAsSeenJson> getWatchedEpisodesQueue);
        void SaveWatchedMovies(List<SyncWatched> syncWatcheds, string token, List<SetAsSeenJson> getWatchedMovieQueue);
        List<WatchedEpisodes> GetEpisodeWatchedByUser(string token);
        bool SaveShowRating(RateRequestJsonDto rateRequest, string token);
        bool SaveShowComment(CommentRequestJsonDto commentRequest, string token);
        bool SaveMovieRating(RateRequestJsonDto rateRequest, string token);
        bool SaveMovieComment(CommentRequestJsonDto commentRequest, string token);
        bool SetMovieAsSeen(SetAsSeenJson setAsSeenRequest, string token);
        List<SetAsSeenResultJson> SetEpisodesAsSeen(List<SetAsSeenJson> setAsSeenRequest, string token);
        List<WatchedMovie> GetMoviesWatchedByUser(string token);
        List<RateResultJsonDto> RateEpisodes(List<RateRequestJsonDto> rateRequest, string token);
        void UpdateWatchedEpisode(List<WatchedEpisodes> userWatchedEpisodes, string token);
        void UpdateWatchedMovies(List<WatchedMovie> userWatchedMovies, string token);
        void UpdateRatingUser(List<UserRating> userRatedShows, string token, RequestType type);
        Dictionary<UserRating, string> GetShowRatingsQueue(string token);
        Dictionary<UserRating, string> GetEposideRatingsQueue(string token);
        Dictionary<UserRating, string> GetMovieRatingsQueue(string token);
        void DeleteRatingQueue(string token);
        List<SetAsSeenJson> GetWatchedEpisodesQueue(string token);
        List<SetAsSeenJson> GetWatchedMoviesQueue(string token);
        void DeleteWatchedQueue(string token);
        List<CommentRequestJsonDto> GetMovieComments(string token);
        List<CommentRequestJsonDto> GetShowComments(string token);
        string GetTraktTokenByShiftvToken(string token);
        void DeleteMovieComment(CommentRequestJsonDto commentRequestJsonDto, string token);
        void DeleteShowComment(CommentRequestJsonDto commentRequestJsonDto, string token);
        void DeleteUnwatchedQueue(string token);
    }
}   