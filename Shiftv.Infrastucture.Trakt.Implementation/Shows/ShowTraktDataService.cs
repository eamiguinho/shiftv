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
using Shiftv.Contracts.Data.Peoples;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Shows;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.PlatformSpecificServices;
using Shiftv.Core.Models.Results;
using Shiftv.Global;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Shows
{
    public class ShowTraktDataService : IShowTraktDataService
    {
        private readonly IShowTraktQueryService _queryService;
        private IDataBackupService _backupService;

        public ShowTraktDataService(IShowTraktQueryService queryService, IDataBackupService backupService)
        {
            _queryService = queryService;
            _backupService = backupService;
        }

        public Task<List<IMiniShow>> GetTrending(UserTokenDto userAccount)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetTrending();
                    List<MiniShowDto> x;
                    if (userAccount != null) x = await TraktDataServiceHelper.GetObjectWithCredentials<List<MiniShowDto>>(url, userAccount);
                    else x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MiniShowDto>>(url);
                    return x != null ? x.Select(MiniShowDtoFactory.CreateShow).ToList() : null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IShow> GetByImdbId(UserTokenDto user, IdsDto ids)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (ids == null) return null;
                    if (ids.TraktId != null)
                    {
                        var url = await _queryService.GetByImdbId(ids.TraktId.Value);
                        ShowDto x;
                        if (user != null) x = await TraktDataServiceHelper.GetObjectWithCredentials<ShowDto>(url, user);
                        else x = await TraktDataServiceHelper.GetObjectWithoutCredentials<ShowDto>(url);
                        return x != null ? ShowDtoFactory.CreateShow(x) : null;
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });

        }


        public Task<List<IShow>> GetRecommendationsByUser(UserTokenDto userAccount)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null) return null;
                    var url = await _queryService.GetRecommendations();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentials<List<ShowDto>>(url, userAccount);
                    return x == null ? null : x.Select(ShowDtoFactory.CreateShow).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IMiniShow>> SearchShowsByKey(string key)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (key == null) return null;
                    var url = await _queryService.GetSearchByKey(key);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MiniShowDto>>(url, true);
                    return x == null ? null : x.Select(MiniShowDtoFactory.CreateShow).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<string>> GetAllShowGenres()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetAllShowGenres();
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<string>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }


        public Task<IGenericPostResult> AddShowToWatchlist(UserTokenDto userAccount, int tvDbId, string title, int year)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || tvDbId <= -1 || year <= -1) return null;
                    var url = await _queryService.AddShowToWatchList();
                    var list = new List<ShowDto>();
                    //list.Add(new ShowDto { Title = title, TvDbId = tvDbId, Year = year });
                    var rateReq = new AddToWatchListRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Shows = list
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
                    return GenericPostResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IGenericPostResult> RemoveShowFromWatchlist(UserTokenDto userAccount, int tvDbId, string title, int year)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || string.IsNullOrEmpty(title) || tvDbId <= -1 || year <= -1) return null;
                    var url = await _queryService.RemoveShowFromWatchList();
                    var list = new List<ShowDto>();
                    //list.Add(new ShowDto { Title = title, R = tvDbId, Year = year });
                    var rateReq = new AddToWatchListRequestJsonDto
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Shows = list
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
                    return GenericPostResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IShow>> GetShowsWatchlistByUser(UserTokenDto user, bool b = false)
        {
            return Task.Run(async () =>
            {
                try
                {
                     if (user == null) return null;
                     var url = await _queryService.GetShowsWatchlistByUser(user.UserSettings.User.Username);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<ShowDto>>(url, b);
                    if (x == null) return null;
                    if (b) TraktDataServiceHelper.SaveToLocal(x, user.UserSettings.User.Username.ToLower() + "Watchlist", _backupService);
                    return x.Select(ShowDtoFactory.CreateShow).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
        public Task<List<IShow>> GetShowsWithEpisodesWatchlistByUser(string username)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(username)) return null;
                    var url = await _queryService.GetShowsWithEpisodesWatchlistByUser(username);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<ShowDto>>(url);
                    return x == null ? null : x.Select(ShowDtoFactory.CreateShow).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IMiniShow>> GetAnimeList(UserTokenDto user)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetAnimeList();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentials<List<MiniShowDto>>(url, user);
                    if (x != null)
                    {
                        return x.Select(MiniShowDtoFactory.CreateShow).ToList();
                    }
                    return null;
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


        public Task<List<IShowProgress>> GetShowProgress(UserTokenDto user)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (user == null) return null;
                    var url = await _queryService.GetShowProgress();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentials<List<ShowProgressDto>>(url, user);
                    if (x == null) return null;
                    return x.Select(ShowProgressDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IShow>> GetLovedByUser(string username)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(username)) return null;
                    var url = await _queryService.GetLovedByUser(username);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<ShowDto>>(url);
                    if (x != null)
                    {
                        TraktDataServiceHelper.SaveToAzure(x, username.ToLower() + "LovedShows", BackupContainerTypes.UserSpecificData, _backupService);
                        return x.Select(ShowDtoFactory.CreateShow).ToList();
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IShow>> GetLovedByUserAzure(string username)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var trendingShows = await _backupService.GetFileFromAzure(username.ToLower() + "LovedShows", BackupContainerTypes.UserSpecificData);
                    if (trendingShows == null) return null;
                    var objectReceived = JsonConvert.DeserializeObject<List<ShowDto>>(trendingShows);
                    return objectReceived.Select(ShowDtoFactory.CreateShowClean).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IMiniShow>> GetTopImdb(UserTokenDto userDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetTopImdb();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentials<List<MiniShowDto>>(url, userDto);
                    return x != null ? x.Select(MiniShowDtoFactory.CreateShow).ToList() : null;
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

        public Task<List<IMiniShow>> GetPopular(UserTokenDto userDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetPopular();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentials<List<MiniShowDto>>(url, userDto);
                    return x != null ? x.Select(MiniShowDtoFactory.CreateShow).ToList() : null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IRateResult> RateShow(UserTokenDto userAccount, int rate, ShowDto show)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (userAccount == null || show == null || rate <= -1) return null;
                    var url = await _queryService.RateShow();
                    var rateReq = new RateRequestJsonDto
                    {
                        Show = new ShowRequestJsonDto
                        {
                            Year = show.Year,
                            Title = show.Title,
                            Ids = show.Ids
                        },
                        Rating = rate,
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

        public Task<IShow> ForceUpdate(IdsDto ids, UserTokenDto user)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (ids == null) return null;
                    if (ids.TraktId != null)
                    {
                        var url = await _queryService.ForceUpdate(ids.TraktId.Value);
                        ShowDto x;
                        if (user != null) x = await TraktDataServiceHelper.GetObjectWithCredentials<ShowDto>(url, user);
                        else x = await TraktDataServiceHelper.GetObjectWithoutCredentials<ShowDto>(url);
                        return x != null ? ShowDtoFactory.CreateShow(x) : null;
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}
