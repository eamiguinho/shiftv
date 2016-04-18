using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Comments;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.PlatformSpecificServices;
using Shiftv.Global;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Comments
{
    public class CommentTraktDataService : ICommentTraktDataService
    {
        private ICommentTraktQueryService _queryService;
        private IDataBackupService _backupService;

        public CommentTraktDataService(ICommentTraktQueryService queryService, IDataBackupService backupService)
        {
            _queryService = queryService;
            _backupService = backupService;
        }
        public Task<List<IComment>> GetCommentsShowById(IdsDto ids)
        {   
            return Task.Run(async () =>
            {
                try
                {
                    if (ids == null || ids.TraktId == null) return null;
                    var url = await _queryService.GetCommentsShowById(ids.TraktId);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<CommentDto>>(url);
                    return x.Select(dto => CommentDtoFactory.Create(dto)).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<ICommentResult> CommentShow(UserTokenDto userAccount, string comment, ShowDto show, bool isSpoiler, bool isReview)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(comment) || userAccount == null || show == null) return null;
                    var url = await _queryService.CommentShow();
                    var rateReq = new CommentRequestJsonDto()
                    {
                        Show = new ShowRequestJsonDto
                        {
                            Ids = show.Ids,
                            Title = show.Title,
                            Year = show.Year
                        },
                        Comment = comment,
                        Spoiler = isSpoiler,
                        Review = isReview
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials(url, myContent, userAccount.AccessToken);
                    var res = Ioc.Container.Resolve<ICommentResult>();
                    res.Status = x == HttpStatusCode.Created ? RequestResults.Success : RequestResults.Failure;
                    return res;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<ICommentResult> Edit(UserTokenDto userAccount, string comment, int commentId, bool isSpoiler, bool isReview, string type)
        {
            return Task.Run(async () => 
            {
                try
                {
                    if (string.IsNullOrEmpty(comment) || userAccount == null) return null;
                    var url = await _queryService.EditCommentShow();
                    var rateReq = new EditCommentRequestJsonDto()
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Type = type,
                        CommentId = commentId,
                        Comment = comment,
                        Spoiler = isSpoiler,
                        Review = isReview
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    //var x = await TraktDataServiceHelper.PostWithCredentials<CommentResultDto>(url, myContent);
                    //return CommentResultDtoFactory.Create(x);
                    return Ioc.Container.Resolve<ICommentResult>();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IComment>> GetCommentsByEpisode(int tvdbId, int season, int episodeNumber)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (tvdbId < 0 || season < 0 || episodeNumber <0) return null;
                    var url = await _queryService.GetCommentsByEpisode(tvdbId, season, episodeNumber);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<CommentDto>>(url);
                    return x.Select(dto => CommentDtoFactory.Create(dto)).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<ICommentResult> CommentEpisode(UserTokenDto userAccount, string comment, string title, int tvDbId, int year, int season, int episode, bool isSpoiler, bool isReview)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(comment) || userAccount == null || string.IsNullOrEmpty(title) || tvDbId <= -1 || year <= -1 || season <= -1 || episode <= -1) return null;
                    var url = await _queryService.CommentEpisode();
                    var rateReq = new CommentEpisodeRequestJsonDto()
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Title = title,
                        TvDbId = tvDbId,
                        Year = year,
                        Comment = comment,
                        Spoiler = isSpoiler,
                        Review = isReview,
                        Episode = episode,
                        Season = season
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials(url, myContent);
                    return Ioc.Container.Resolve<ICommentResult>();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<IComment>> GetCommentsMovieById(IdsDto ids)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (ids == null || ids.TraktId == null) return null;
                    var url = await _queryService.GetCommentsMovieById(ids.TraktId);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<CommentDto>>(url);
                    return x.Select(dto => CommentDtoFactory.Create(dto)).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<ICommentResult> CommentMovie(UserTokenDto userAccount, string comment, MovieDto movie, bool isSpoiler, bool isReview)
        {   
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(comment) || userAccount == null || movie == null) return null;
                    var url = await _queryService.CommentMovie();
                    var rateReq = new CommentRequestJsonDto()
                    {
                        Movie = new MovieRequestJsonDto{Title = movie.Title,Year = movie.Year,Ids = movie.Ids},
                        Comment = comment,
                        Spoiler = isSpoiler,
                        Review = isReview
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(rateReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials(url, myContent, userAccount.AccessToken);
                    var res = Ioc.Container.Resolve<ICommentResult>();
                    res.Status = x == HttpStatusCode.Created ? RequestResults.Success : RequestResults.Failure;
                    return res;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public async void SaveCommentLocally(IComment test)
        {
            var trendingShows = await _backupService.ReadLocalFile("Comments");
            var objectReceived = new List<CommentDto>();
            if (trendingShows != null) objectReceived = JsonConvert.DeserializeObject<List<CommentDto>>(trendingShows);
            objectReceived.Add(CommentDtoFactory.GetDto(test));
            TraktDataServiceHelper.SaveToLocal(objectReceived, "Comments", _backupService);
        }

        public Task<List<IComment>> GetCommentsLocally()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var trendingShows = await _backupService.ReadLocalFile("Comments");
                    if (trendingShows == null) return null;
                    var objectReceived = JsonConvert.DeserializeObject<List<CommentDto>>(trendingShows);
                    return objectReceived.Select(CommentDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}
