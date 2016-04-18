using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Data.Media;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Data.Peoples;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Movies;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.PlatformSpecificServices;
using Shiftv.Global;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Movies
{
    public class MovieTraktDataService : IMovieTraktDataService
    {
        private IMovieTraktQueryService _queryService;
        private IDataBackupService _backupService;

        public MovieTraktDataService(IMovieTraktQueryService queryService, IDataBackupService backupService)
        {
            _queryService = queryService;
            _backupService = backupService;
        }

        public Task<List<IMiniMovie>> GetTrending(UserTokenDto userAccount)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetTredingQuery();
                    List<MiniMovieDto> res;
                    if (userAccount != null) res = await TraktDataServiceHelper.GetObjectWithCredentials<List<MiniMovieDto>>(url, userAccount);
                    else res = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MiniMovieDto>>(url);

                    return res.Select(MiniMovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IMiniMovie>> GetPopular(UserTokenDto userAccount)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetPopular();
                    List<MiniMovieDto> res;
                    if (userAccount != null) res = await TraktDataServiceHelper.GetObjectWithCredentials<List<MiniMovieDto>>(url, userAccount);
                    else res = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MiniMovieDto>>(url);

                    return res.Select(MiniMovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IPeople> GetPeople(IdsDto ids)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (ids == null) return null;
                    if (ids.TraktId != null)
                    {
                        var url = await _queryService.GetPeople(ids.TraktId.Value);
                        var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<PeopleDto>(url);
                        return x != null ? PeopleDtoFactory.Create(x) : null;
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IMiniMovie>> GetOscars(UserTokenDto user)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetOscars();
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MiniMovieDto>>(url, true);
                    return x == null ? null : x.Select(MiniMovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IMiniMovie>> GetChristmasMovies(UserTokenDto user)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetChristmas();
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MiniMovieDto>>(url, true);
                    return x == null ? null : x.Select(MiniMovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


        public async Task<List<IMovie>> GetTrendingAzure()
        {
            try
            {
                var trendingShows = await _backupService.GetFileFromAzure("trendingMovies", BackupContainerTypes.GlobalData);
                if (trendingShows == null) return null;
                var objectReceived = JsonConvert.DeserializeObject<List<MovieDto>>(trendingShows);
                return objectReceived.Select(MovieDtoFactory.CreateClean).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }



        public Task<List<IMiniMovie>> SearchMoviesByKey(string key)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (key == null) return null;
                    var url = await _queryService.GetSearchMoviesByKey(key);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MiniMovieDto>>(url, true);
                    return x == null ? null : x.Select(MiniMovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


        public Task<List<IMovie>> GetRecommendations(UserTokenDto userAccount)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetRecommendations();
                    var res = await TraktDataServiceHelper.GetObjectWithCredentials<List<MovieDto>>(url, userAccount);
                    return res.Select(MovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IRateResult> RateMovie(UserTokenDto userAccount, int rate, MovieDto movie)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || movie  == null) return null;
                    var url = await _queryService.RateMovie();
                    var rateReq = new RateRequestJsonDto
                    {
                        Movie = new MovieRequestJsonDto
                        {
                            Ids = movie.Ids,
                            Year = movie.Year,
                            Title = movie.Title
                        },
                        Rating = rate
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials(url, myContent, userAccount.AccessToken);
                    var res = Ioc.Container.Resolve<IRateResult>();
                    res.Status = x == HttpStatusCode.Created;
                    return res;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


        public Task<IMovie> GetByImdbId(UserTokenDto user, IdsDto ids)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (ids == null) return null;
                    if (ids.TraktId != null)
                    {
                        var url = await _queryService.GetByImdbId(ids.TraktId.Value);
                        MovieDto x;
                        if (user != null) x = await TraktDataServiceHelper.GetObjectWithCredentials<MovieDto>(url, user);
                        else x = await TraktDataServiceHelper.GetObjectWithoutCredentials<MovieDto>(url);
                        if (x != null)
                        {
                            return MovieDtoFactory.Create(x);
                        }
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }



        public Task<ICheckinResult> CheckIn(UserTokenDto userAccount, string title, string imdbId, int year)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(imdbId) || year <= -1) return null;
                    var url = await _queryService.GetCheckIn();
                    var rateReq = new CheckInEpisodeRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Title = title,
                        ImdbId = imdbId,
                        Year = year
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<CheckInResultDto>(url, myContent);
                    return CheckInResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<ICheckinResult> CancelCheckIn(UserTokenDto userAccount)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null) return null;
                    var url = await _queryService.GetCancelCheckIn();
                    var rateReq = new CheckInEpisodeRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<CheckInResultDto>(url, myContent);
                    return CheckInResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IMiniMovie>> GetTopImdb(UserTokenDto user)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetTopImdb();
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MiniMovieDto>>(url, true);
                    return x == null ? null : x.Select(MiniMovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public async Task<List<IMovie>> GetTopImdbAzure()
        {
            try
            {
                var trendingShows = await _backupService.GetFileFromAzure("topImdbMovies", BackupContainerTypes.GlobalData);
                if (trendingShows == null) return null;
                var objectReceived = JsonConvert.DeserializeObject<ListShowsResult>(trendingShows);
                var list = new List<MovieDto>();
                if (objectReceived != null)
                {
                    list.AddRange(objectReceived.Items.Select(listShowsResult => listShowsResult.Movie));
                    return list.Select(MovieDtoFactory.CreateClean).ToList();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<List<IMiniMovie>> GetAnimationMovies(UserTokenDto user)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetAnimationMovies();
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MiniMovieDto>>(url, true);
                    return x == null ? null : x.Select(MiniMovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public async Task<List<IMovie>> GetAnimationMoviesAzure()
        {
            try
            {
                var trendingShows = await _backupService.GetFileFromAzure("animationMovies", BackupContainerTypes.GlobalData);
                if (trendingShows == null) return null;
                var objectReceived = JsonConvert.DeserializeObject<ListShowsResult>(trendingShows);
                var list = new List<MovieDto>();
                if (objectReceived != null)
                {
                    list.AddRange(objectReceived.Items.Select(listShowsResult => listShowsResult.Movie));
                    return list.Select(MovieDtoFactory.CreateClean).ToList();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public Task<List<IMiniMovie>> GetMoviesWatchlistByUser(UserTokenDto user)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetUserWatchlist();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentials<List<MiniMovieDto>>(url, user);
                    return x.Select(MiniMovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IMovie>> GetLovedByUser(string username)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(username)) return null;
                    var url = await _queryService.GetLovedByUser(username);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MovieDto>>(url);
                    if (x != null)
                    {
                        TraktDataServiceHelper.SaveToAzure(x, username + "LovedMovies", BackupContainerTypes.UserSpecificData, _backupService);
                    }
                    return x.Select(MovieDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public async Task<List<IMovie>> GetLovedByUserAzure(string username)
        {
            try
            {
                var trendingShows = await _backupService.GetFileFromAzure(username + "LovedMovies", BackupContainerTypes.UserSpecificData);
                if (trendingShows == null) return null;
                var objectReceived = JsonConvert.DeserializeObject<List<MovieDto>>(trendingShows);
                return objectReceived.Select(MovieDtoFactory.CreateClean).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }



        public Task<IMediaStream> GetLinks(string imdbId, string language)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(imdbId)) return null;
                    var url = await _queryService.GetLinks(imdbId, language);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<MediaStreamDto>(url, 30);
                    if (x != null && x.Subtitles != null)
                    {
                        SendSubtitlesToAzure(x, language, imdbId);
                    }
                    return MediaStreamDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        private void SendSubtitlesToAzure(MediaStreamDto mediaStreamDto, string language, string imdbId)
        {
            var languagesplit = language.Split(',');
            foreach (var s in languagesplit)
            {
                var t = new MediaStreamDto { Subtitles = new List<SubtitlesInfoDto>() };
                var s1 = s;
                t.Subtitles.AddRange(mediaStreamDto.Subtitles.Where(x => x.LanguageId == s1));
                TraktDataServiceHelper.SaveToAzure(t, imdbId + s1 + "subtitles", BackupContainerTypes.Subtitles, _backupService, true);
            }
        }

        public async Task<IMediaStream> GetSubtitlesFromAzure(string imdbId, string language)
        {
            try
            {
                var listM = new List<MediaStreamDto>();
                var languagesplit = language.Split(',');
                foreach (var s in languagesplit)
                {
                    var trendingShows = await _backupService.GetFileFromAzure(imdbId + s + "subtitles", BackupContainerTypes.Subtitles);
                    if (trendingShows == null) return MediaStreamDtoFactory.Create(null);
                    var objectReceived = JsonConvert.DeserializeObject<MediaStreamDto>(trendingShows);
                    listM.Add(objectReceived);
                }
                if (listM.Count > 0)
                {
                    var t = new MediaStreamDto { Subtitles = new List<SubtitlesInfoDto>() };
                    foreach (var mediaStreamDto in listM)
                    {
                        t.Subtitles.AddRange(mediaStreamDto.Subtitles);
                    }
                    return MediaStreamDtoFactory.Create(t);
                }
                return MediaStreamDtoFactory.Create(null);
            }
            catch (Exception)
            {
                return MediaStreamDtoFactory.Create(null);
            }
        }



        public Task<IGenericPostResult> SetAsSeen(UserTokenDto userAccount, MovieDto movie)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || movie == null) return null;
                    var url = await _queryService.GetSetAsSeen();
                    var movieAsSeenRequest = new SetAsSeenJsonDto
                    {
                        Movie = new MovieRequestJsonDto
                        {
                            Ids = movie.Ids,
                            Title = movie.Title,
                            Year = movie.Year,
                        },
                        WatchedAt = DateTime.Now,
                        Watched = true
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(movieAsSeenRequest));
                    var x = await TraktDataServiceHelper.PostWithCredentials(url, myContent, userAccount.AccessToken);
                    var res = Ioc.Container.Resolve<IGenericPostResult>();
                    res.Status = x == HttpStatusCode.Created ? RequestResults.Success : RequestResults.Failure;
                    return res;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> SetAsUnseen(UserTokenDto userAccount, MovieDto movie)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || movie == null) return null;
                    var url = await _queryService.GetSetAsSeen();
                    var movieAsSeenRequest = new SetAsSeenJsonDto
                    {
                        Movie = new MovieRequestJsonDto
                        {
                            Ids = movie.Ids,
                            Title = movie.Title,
                            Year = movie.Year,
                        },
                        WatchedAt = DateTime.Now,
                        Watched = false
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(movieAsSeenRequest));
                    var x = await TraktDataServiceHelper.PostWithCredentials(url, myContent, userAccount.AccessToken);
                    var res = Ioc.Container.Resolve<IGenericPostResult>();
                    res.Status = x == HttpStatusCode.Created ? RequestResults.Success : RequestResults.Failure;
                    return res;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> AddMovieToWatchlist(UserTokenDto userAccount, int? traktId, string title, int year)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || year <= -1 || traktId == null) return null;
                    var url = await _queryService.AddMovieToWatchList();
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(traktId));
                    var x = await TraktDataServiceHelper.PostWithCredentials<bool>(url, myContent, userAccount.AccessToken);
                    return GenericPostResultDtoFactory.Create(new GenericPostResultDto { Status = x ? "success" : "failed" });
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> RemoveMovieFromWatchlist(UserTokenDto userAccount, int? traktId, string title, int year)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || year <= -1) return null;
                    var url = await _queryService.RemoveMovieFromWatchlist();
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(traktId));
                    var x = await TraktDataServiceHelper.PostWithCredentials<bool>(url, myContent, userAccount.AccessToken);
                    return GenericPostResultDtoFactory.Create(new GenericPostResultDto{Status = x ? "success" : "failed" });
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<double?> GetImdbRanting(string imdbId)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(imdbId)) return null;
                    var url = await _queryService.GetImdbRating(imdbId);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<ImdbRatingDto>(url);
                    return x == null ? (double?)null : x.ImdbRating;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


    }


}
