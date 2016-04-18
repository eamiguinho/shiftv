using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Data.Sync;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Sync;
using ShiftvAPI.Contracts.Services.Sync;

namespace ShiftvAPI.Services.Implementation.Sync
{
    class SyncService : ISyncService
    {
        private ISyncTraktDataService _syncTraktDataService;
        private ISyncShiftvDataService _syncShiftvDataService;
        
        public SyncService(ISyncShiftvDataService syncShiftvDataService, ISyncTraktDataService syncTraktDataService)
        {
            _syncShiftvDataService = syncShiftvDataService;
            _syncTraktDataService = syncTraktDataService;
        }

        public async Task<DataResult<List<RatingSync>>> GetRatings(string token, RequestType requestType)
        {
            var x = await _syncTraktDataService.GetRatings(token, requestType);
            if (x != null)
            {
                switch (requestType)
                {
                    case RequestType.Shows:
                        _syncShiftvDataService.SaveRatingsShowsMovies(x, token, requestType);
                        break;
                    case RequestType.Seasons:
                        break;
                    case RequestType.Episodes:
                        _syncShiftvDataService.SaveRatingsShowsMovies(x, token, requestType);
                        break;
                    case RequestType.Movies:
                        _syncShiftvDataService.SaveRatingsShowsMovies(x, token, requestType);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("requestType");
                }
            }
            return x != null ? new DataResult<List<RatingSync>>(x) : new DataResult<List<RatingSync>>(StandardResults.Error);
        }

        public async Task<DataResult<SyncStats>> GetStats(string token)
        {
            var x = await _syncTraktDataService.GetStats(token);
            return x != null ? new DataResult<SyncStats>(x) : new DataResult<SyncStats>(StandardResults.Error);
        }

        public async Task<DataResult<List<SyncWatched>>> GetWatchedShows(string token)
        {
            var x = await _syncTraktDataService.GetWatched(token, RequestType.Shows);

            if (x != null)
            {
                var getWatchedEpisodesQueue = _syncShiftvDataService.GetWatchedEpisodesQueue(token);
                _syncShiftvDataService.SaveWatchedEpisode(x, token, getWatchedEpisodesQueue);
            }

            return x != null ? new DataResult<List<SyncWatched>>(x) : new DataResult<List<SyncWatched>>(StandardResults.Error);
        }

        public async Task<DataResult<List<SyncWatched>>> GetWatchedMovies(string token)
        {
            var x = await _syncTraktDataService.GetWatched(token, RequestType.Movies);
            if (x != null)
            {
                var getWatchedMovieQueue = _syncShiftvDataService.GetWatchedMoviesQueue(token);
                _syncShiftvDataService.SaveWatchedMovies(x, token, getWatchedMovieQueue);
            }
            return x != null ? new DataResult<List<SyncWatched>>(x) : new DataResult<List<SyncWatched>>(StandardResults.Error);
        }

        public bool Rate(RateRequestJsonDto rateRequest, string token, RequestType type)
        {
            var res = false;
            switch (type)
            {
                case RequestType.Shows:
                    res = _syncShiftvDataService.SaveShowRating(rateRequest, token);
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    break;
                case RequestType.Movies:
                    res = _syncShiftvDataService.SaveMovieRating(rateRequest, token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return res;
        }

        public bool RateFireForget(RateRequestJsonDto rateRequest, string token, RequestType type)
        {
            var res = false;
            switch (type)
            {
                case RequestType.Shows:
                    var userRatedShows = _syncShiftvDataService.GetShowRatingsByUser(token);
                    if (rateRequest.Show.Ids.TraktId != null)
                    {
                        if (userRatedShows.Any(x => x.TraktId == rateRequest.Show.Ids.TraktId.Value))
                        {
                            var userRated =
                                userRatedShows.FirstOrDefault(x => x.TraktId == rateRequest.Show.Ids.TraktId.Value);
                            if (userRated != null) userRatedShows.Remove(userRated);
                        }
                        userRatedShows.Add(new UserRating { TraktId = rateRequest.Show.Ids.TraktId.Value, Rating = rateRequest.Rating });
                    }
                    _syncShiftvDataService.UpdateRatingUser(userRatedShows, token, type);
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    break;
                case RequestType.Movies:
                    var userRatedMovies = _syncShiftvDataService.GetMovieRatingsByUser(token);
                    if (rateRequest.Movie.Ids.TraktId != null)
                    {
                        if (userRatedMovies.Any(x => x.TraktId == rateRequest.Movie.Ids.TraktId.Value))
                        {
                            var userRated =
                                userRatedMovies.FirstOrDefault(x => x.TraktId == rateRequest.Movie.Ids.TraktId.Value);
                            if (userRated != null) userRatedMovies.Remove(userRated);
                        }
                        userRatedMovies.Add(new UserRating { TraktId = rateRequest.Movie.Ids.TraktId.Value, Rating = rateRequest.Rating });
                        _syncShiftvDataService.UpdateRatingUser(userRatedMovies, token, type);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            var traktToken = _syncShiftvDataService.GetTraktTokenByShiftvToken(token);
            GetUploadRatings(traktToken);
            return res;
        }



        public bool Comment(CommentRequestJsonDto commentRequest, string token, RequestType type)
        {
            var res = false;
            switch (type)
            {
                case RequestType.Shows:
                    res = _syncShiftvDataService.SaveShowComment(commentRequest, token);
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    break;
                case RequestType.Movies:
                    res = _syncShiftvDataService.SaveMovieComment(commentRequest, token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return res;
        }

        public bool CommentFireForge(CommentRequestJsonDto commentRequest, string token, RequestType type)
        {
            var res = false;
            var traktToken = _syncShiftvDataService.GetTraktTokenByShiftvToken(token);

            switch (type)
            {
                case RequestType.Shows:

                    GetUploadComments(traktToken);
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    break;
                case RequestType.Movies:

                    GetUploadComments(traktToken);              
                break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return res;
        }

        public bool SetAsSeen(SetAsSeenJson setAsSeenRequest, string token, RequestType type)
        {
            var res = false;
            switch (type)
            {
                case RequestType.Shows:
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    break;
                case RequestType.Movies:
                    res = _syncShiftvDataService.SetMovieAsSeen(setAsSeenRequest, token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return res;
        }

        public bool SetAsSeenFireForget(SetAsSeenJson setAsSeenRequest, string token, RequestType type)
        {
            var res = false;
            switch (type)
            {
                case RequestType.Shows:
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    break;
                case RequestType.Movies:
                    var userWatchedMovies = _syncShiftvDataService.GetMoviesWatchedByUser(token);
                    if (setAsSeenRequest.Movie.Ids.TraktId != null)
                        userWatchedMovies.Add(new WatchedMovie { TraktId = setAsSeenRequest.Movie.Ids.TraktId.Value });
                    _syncShiftvDataService.UpdateWatchedMovies(userWatchedMovies, token);
                    var traktToken = _syncShiftvDataService.GetTraktTokenByShiftvToken(token);
                    GetUploadWatchedEpisodes(traktToken);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return res;
        }

  

        public List<RateResultJsonDto> Rate(List<RateRequestJsonDto> rateRequest, string token, RequestType type)
        {
            var res = new List<RateResultJsonDto>();
            switch (type)
            {
                case RequestType.Shows:
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    res = _syncShiftvDataService.RateEpisodes(rateRequest, token);
                    break;
                case RequestType.Movies:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return res;
        }
        
        public List<RateResultJsonDto> RateFireForget(List<RateRequestJsonDto> rateRequest, string token, RequestType type)
        {
            var res = new List<RateResultJsonDto>();
            switch (type)
            {
                case RequestType.Shows:
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    var userRatedShows = _syncShiftvDataService.GetEpisodeRatingsByUser(token);
                    foreach (var rateRequestJsonDto in rateRequest.Where(rateRequestJsonDto => rateRequestJsonDto.Show.Ids.TraktId != null))
                    {
                        if (userRatedShows.Any(x => rateRequestJsonDto.Show.Ids.TraktId != null && x.TraktId == rateRequestJsonDto.Show.Ids.TraktId.Value))
                        {
                            var userRated =
                                userRatedShows.FirstOrDefault(x => x.TraktId == rateRequestJsonDto.Show.Ids.TraktId.Value && x.Episode.Value == rateRequestJsonDto.Episode.Number && x.Season.Value == rateRequestJsonDto.Episode.Season);
                            if(userRated != null) userRatedShows.Remove(userRated);
                        }
                        userRatedShows.Add(new UserRating { TraktId = rateRequestJsonDto.Show.Ids.TraktId.Value, Rating = rateRequestJsonDto.Rating, Episode = rateRequestJsonDto.Episode.Number, Season = rateRequestJsonDto.Episode.Season});
                    }
                    _syncShiftvDataService.UpdateRatingUser(userRatedShows, token, type);
                    var traktToken = _syncShiftvDataService.GetTraktTokenByShiftvToken(token);
                    GetUploadRatings(traktToken);
                    break;
                case RequestType.Movies:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return res;
        }


        public List<SetAsSeenResultJson> SetAsSeen(List<SetAsSeenJson> setAsSeenRequest, string token, RequestType type)
        {
            var res = new List<SetAsSeenResultJson>();
            switch (type)
            {
                case RequestType.Shows: 
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    res = _syncShiftvDataService.SetEpisodesAsSeen(setAsSeenRequest, token);
                    break;
                case RequestType.Movies:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return res;
        }

        public List<SetAsSeenResultJson> SetAsSeenFireForget(List<SetAsSeenJson> setAsSeenRequest, string token, RequestType type)
        {
            var res = new List<SetAsSeenResultJson>();
            switch (type)
            {
                case RequestType.Shows:
                    break;
                case RequestType.Seasons:
                    break;
                case RequestType.Episodes:
                    var userWatchedEpisodes = _syncShiftvDataService.GetEpisodeWatchedByUser(token);
                    foreach (var setAsSeenJson in setAsSeenRequest)
                    {
                        if (setAsSeenJson.Show.Ids.TraktId != null)
                            userWatchedEpisodes.Add(new WatchedEpisodes { EpisodeNumber = setAsSeenJson.Episode.Number, SeasonNumber = setAsSeenJson.Episode.Season, TraktShowId = setAsSeenJson.Show.Ids.TraktId.Value });
                    }
                    _syncShiftvDataService.UpdateWatchedEpisode(userWatchedEpisodes, token);
                    var traktToken = _syncShiftvDataService.GetTraktTokenByShiftvToken(token);
                    GetUploadWatchedEpisodes(traktToken);
                    break;
                case RequestType.Movies:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return res;
        }


        public async Task<DataResult> GetUploadRatings(string token)
        {
            var getShowRatingsToUpload = _syncShiftvDataService.GetShowRatingsQueue(token);
            var getEpisodeRatingsToUpload = _syncShiftvDataService.GetEposideRatingsQueue(token);
            var getMovieRatingsToUpload = _syncShiftvDataService.GetMovieRatingsQueue(token);
            var uploadRating = new UploadRating();
            uploadRating.Shows = new List<ShowRateRequestJson>();
            uploadRating.Movies = new List<MovieRequestRateJson>();
            uploadRating.Episodes= new List<EpisodePostRateRequestJson>();
            foreach (var showRating in getShowRatingsToUpload)
            {
                var parsed = JsonConvert.DeserializeObject<RateRequestJsonDto>(showRating.Value);
                var ratingParsed = new ShowRateRequestJson();
                ratingParsed.Ids = parsed.Show.Ids;
                ratingParsed.Rating = parsed.Rating;
                uploadRating.Shows.Add(ratingParsed);
            }
            foreach (var movieRating in getMovieRatingsToUpload)
            {
                var parsed = JsonConvert.DeserializeObject<RateRequestJsonDto>(movieRating.Value);
                var ratingParsed = new MovieRequestRateJson();
                ratingParsed.Ids = parsed.Movie.Ids;
                ratingParsed.Rating = parsed.Rating;
                uploadRating.Movies.Add(ratingParsed);
            }
            foreach (var episodeRating in getEpisodeRatingsToUpload)
            {
                var parsed = JsonConvert.DeserializeObject<RateRequestJsonDto>(episodeRating.Value);
                var ratingParsed = new EpisodePostRateRequestJson();
                ratingParsed.Ids = parsed.Episode.Ids;
                ratingParsed.Rating = parsed.Rating;
                uploadRating.Episodes.Add(ratingParsed);
            }
            var res = await _syncTraktDataService.UploadRatings(uploadRating, token);
            if (res)
            {
                _syncShiftvDataService.DeleteRatingQueue(token);
            }
            return res ? new DataResult(StandardResults.Ok) :new DataResult(StandardResults.Error) ;
        }

        public async Task<DataResult> GetUploadWatchedEpisodes(string token)
        {
            try
            {
                var getWatchedEpisodesQueue = _syncShiftvDataService.GetWatchedEpisodesQueue(token);
                var getWatchedMoviesQueue = _syncShiftvDataService.GetWatchedMoviesQueue(token);
                var listWatched = new UploadWatched();
                listWatched.Episodes = new List<EpisodePostRequestJson>();
                listWatched.Movies = new List<MovieRequestWatchedJson>();
                foreach (var setAsSeenJson in getWatchedEpisodesQueue.Where(x => x.Watched))
                {
                    setAsSeenJson.Episode.WatchedAt = setAsSeenJson.WatchedAt;
                    listWatched.Episodes.Add(new EpisodePostRequestJson { Ids = setAsSeenJson.Episode.Ids, WatchedAt = setAsSeenJson.WatchedAt });
                }
                foreach (var setAsSeenJson in getWatchedMoviesQueue.Where(x => x.Watched))
                {
                    setAsSeenJson.Movie.WatchedAt = setAsSeenJson.WatchedAt;
                    listWatched.Movies.Add(setAsSeenJson.Movie);
                }
                var res = await _syncTraktDataService.UploadWatched(listWatched, token);
                if (res)
                {
                    _syncShiftvDataService.DeleteWatchedQueue(token);
                }
                var listUnWatched = new UploadWatched();
                listUnWatched.Episodes = new List<EpisodePostRequestJson>();
                listUnWatched.Movies = new List<MovieRequestWatchedJson>();
                foreach (var setAsSeenJson in getWatchedEpisodesQueue.Where(x => !x.Watched))
                {
                    setAsSeenJson.Episode.WatchedAt = setAsSeenJson.WatchedAt;
                    listUnWatched.Episodes.Add(new EpisodePostRequestJson { Ids = setAsSeenJson.Episode.Ids, WatchedAt = setAsSeenJson.WatchedAt });
                }
                foreach (var setAsSeenJson in getWatchedMoviesQueue.Where(x => !x.Watched))
                {
                    setAsSeenJson.Movie.WatchedAt = setAsSeenJson.WatchedAt;
                    listUnWatched.Movies.Add(setAsSeenJson.Movie);
                }
                var resUnwatched = await _syncTraktDataService.UploadUnwatched(listUnWatched, token);
                if (resUnwatched)
                {
                    _syncShiftvDataService.DeleteUnwatchedQueue(token);
                }
                return res ? new DataResult(StandardResults.Ok) : new DataResult(StandardResults.Error);

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public async Task<DataResult> GetUploadComments(string token)
        {
            var getMovieComments = _syncShiftvDataService.GetMovieComments(token);
            var getShowComments = _syncShiftvDataService.GetShowComments(token);
            foreach (var commentRequestJsonDto in getMovieComments)
            {
                var upload = new CommentMovieRequestJsonDto
                {
                    Comment = commentRequestJsonDto.Comment,
                    Movie = new MovieRequestJson
                    {
                        Ids = commentRequestJsonDto.Movie.Ids,
                        Title = commentRequestJsonDto.Movie.Title,
                        Year = commentRequestJsonDto.Movie.Year
                    },
                    Review = commentRequestJsonDto.Review,
                    Spoiler = commentRequestJsonDto.Spoiler,
                    CommentId = commentRequestJsonDto.CommentId
                };
                var x = JsonConvert.SerializeObject(upload);
                var res = await _syncTraktDataService.UploadComment(x, token);
                if (res)
                {
                    _syncShiftvDataService.DeleteMovieComment(commentRequestJsonDto, token);
                }
            }
            foreach (var commentRequestJsonDto in getShowComments)
            {
                var upload = new CommentShowRequestJsonDto
                {
                    Comment = commentRequestJsonDto.Comment,
                    Show = new ShowRequestJson
                    {
                        Ids = commentRequestJsonDto.Show.Ids,
                        Title = commentRequestJsonDto.Show.Title,
                        Year = commentRequestJsonDto.Show.Year
                    },
                    Review = commentRequestJsonDto.Review,
                    Spoiler = commentRequestJsonDto.Spoiler,
                    CommentId = commentRequestJsonDto.CommentId
                };
                var x = JsonConvert.SerializeObject(upload);
                var res = await _syncTraktDataService.UploadComment(x, token);
                if (res)
                {
                    _syncShiftvDataService.DeleteShowComment(commentRequestJsonDto, token);

                }
            }


            return new DataResult(StandardResults.Ok);
        }


    }
}