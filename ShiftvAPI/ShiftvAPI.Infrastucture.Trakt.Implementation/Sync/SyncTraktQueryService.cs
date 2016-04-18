using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Sync;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers.Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Sync
{
    class SyncTraktQueryService : ISyncTraktQueryService
    {
        public Task<string> GetRatings(RequestType requestType)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
          TraktConstants.BaseApiUrl,
          TraktConstants.SyncResource,
          TraktConstants.RatingsResource,
          requestType.ToString().ToLower()
          ));
        }

        public Task<string> GetStats()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/",
     TraktConstants.BaseApiUrl,
     TraktConstants.SyncResource,
     TraktConstants.LastActivities
     ));
        }

        public Task<string> GetWatched(RequestType requestType)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
         TraktConstants.BaseApiUrl,
         TraktConstants.SyncResource,
         TraktConstants.WatchedAction,
         requestType.ToString().ToLower()
         ));
        }

        public Task<string> GetSyncRatings()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}",
         TraktConstants.BaseApiUrl,
         TraktConstants.SyncResource,
         TraktConstants.RatingsResource
         ));
        }

        public Task<string> GetSyncWatched()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}",
          TraktConstants.BaseApiUrl,
          TraktConstants.SyncResource,
          TraktConstants.HistoryResource
          ));
        }

        public Task<string> GetUploadComment()
        {
            return Task.Run(() => string.Format("{0}/{1}",
        TraktConstants.BaseApiUrl,
        TraktConstants.CommentsResource
        ));
        }

        public Task<string> GetSyncUnwatched()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
                TraktConstants.BaseApiUrl,
                TraktConstants.SyncResource,
                TraktConstants.HistoryResource,
                TraktConstants.RemoveResource));
        }
    }
}