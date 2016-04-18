using System;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Sync;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Sync
{
    class SyncTraktDataService : ISyncTraktDataService
    {
        private readonly ISyncTraktQueryService _queryService;

        public SyncTraktDataService(ISyncTraktQueryService queryService)
        {
            _queryService = queryService;
        }

        public Task<bool> SyncWatchedShows(UserTokenDto userTokenDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.SyncWatchedShows();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentialsToken<bool>(url, userTokenDto.TraktAccessToken);
                    return x;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<bool> SyncWatchedMovies(UserTokenDto userTokenDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.SyncWatchedMovies();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentialsToken<bool>(url, userTokenDto.TraktAccessToken);
                    return x;
                }
                catch (Exception) 
                {
                    return false;
                }
            });    
        }

        public Task<bool> SyncShowRatings(UserTokenDto userTokenDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.SyncShowRatings();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentialsToken<bool>(url, userTokenDto.TraktAccessToken);
                    return x;
                }
                catch (Exception)
                {
                    return false;
                }
            });    
        }

        public Task<bool> SyncSeasonRatings(UserTokenDto userTokenDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.SyncSeasonRatings();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentialsToken<bool>(url, userTokenDto.TraktAccessToken);
                    return x;
                }
                catch (Exception)
                {
                    return false;
                }
            });    
        }

        public Task<bool> SyncEpisodeRatings(UserTokenDto userTokenDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.SyncEpisodeRatings();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentialsToken<bool>(url, userTokenDto.TraktAccessToken);
                    return x;
                }
                catch (Exception)
                {
                    return false;
                }
            });    
        }

        public Task<bool> SyncMovieRatings(UserTokenDto userTokenDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.SyncMovieRatings();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentialsToken<bool>(url, userTokenDto.TraktAccessToken);
                    return x;
                }
                catch (Exception)
                {
                    return false;
                }
            });    
        }

        public Task<bool> UploadRatingsToTrakt(UserTokenDto userTokenDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.UploadRatingsToTrakt();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentialsToken<bool>(url, userTokenDto.TraktAccessToken);
                    return x;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<bool> UploadWatchedEpisodesToTrakt(UserTokenDto userTokenDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.UploadWatchedEpisodesToTrakt();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentialsToken<bool>(url, userTokenDto.TraktAccessToken);
                    return x;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<bool> UploadCommentsToTrakt(UserTokenDto userTokenDto)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.UploadCommentsToTrakt();
                    var x = await TraktDataServiceHelper.GetObjectWithCredentialsToken<bool>(url, userTokenDto.TraktAccessToken);
                    return x;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
    }
}