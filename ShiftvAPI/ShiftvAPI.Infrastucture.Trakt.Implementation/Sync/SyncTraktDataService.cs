using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Data.Sync;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Sync;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Sync
{
    class SyncTraktDataService : ISyncTraktDataService
    {
        private ISyncTraktQueryService _queryService;

        public SyncTraktDataService(ISyncTraktQueryService queryService)
        {
            _queryService = queryService;
        }
        public Task<List<RatingSync>> GetRatings(string token, RequestType requestType)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetRatings(requestType);
                    List<RatingSync> x = await TraktDataServiceHelper.GetObjectWithCredentials<List<RatingSync>>(url, token);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<SyncStats> GetStats(string token)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetStats();
                    SyncStats x = await TraktDataServiceHelper.GetObjectWithCredentials<SyncStats>(url, token);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<SyncWatched>> GetWatched(string token, RequestType requestType)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetWatched(requestType);
                    List<SyncWatched> x = await TraktDataServiceHelper.GetObjectWithCredentials<List<SyncWatched>>(url, token);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<bool> UploadRatings(UploadRating ratings, string token)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetSyncRatings();
                    var serializeObject = JsonConvert.SerializeObject(ratings);
                    await TraktDataServiceHelper.PostObjectWithCredentials(url, token, serializeObject);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<bool> UploadWatched(UploadWatched listWatched, string token)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetSyncWatched();
                    var serializeObject = JsonConvert.SerializeObject(listWatched);
                    await TraktDataServiceHelper.PostObjectWithCredentials(url, token, serializeObject);
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        public Task<bool> UploadComment(string commentRequestJsonDto, string token)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetUploadComment();
                    await TraktDataServiceHelper.PostObjectWithCredentials(url, token, commentRequestJsonDto, true);
                    return true;
                }
                catch (Exception)
                {
                    return true;
                }
            });
        }

        public Task<bool> UploadUnwatched(UploadWatched listWatched, string token)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetSyncUnwatched();
                    var serializeObject = JsonConvert.SerializeObject(listWatched);
                    await TraktDataServiceHelper.PostObjectWithCredentials(url, token, serializeObject);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
    }
}