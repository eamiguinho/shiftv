using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Sync;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Sync
{
    class SyncTraktQueryService : ISyncTraktQueryService
    {
        public Task<string> SyncWatchedShows()
        {
            //"http://api.trakt.tv/shows/trending.json/" + TraktConstants.TraktKey;
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
               TraktConstants.ShiftvBaseApiUrl,
               TraktConstants.SyncResource,
               TraktConstants.WatchedAction,
               TraktConstants.ShowsAction));
        }

        public Task<string> SyncWatchedMovies()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
              TraktConstants.ShiftvBaseApiUrl,
              TraktConstants.SyncResource,
              TraktConstants.WatchedAction,
              TraktConstants.MoviesResource));
        }

        public Task<string> SyncShowRatings()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
            TraktConstants.ShiftvBaseApiUrl,
            TraktConstants.SyncResource,
            TraktConstants.RatingsResource,
            TraktConstants.ShowsAction));
        }

        public Task<string> SyncSeasonRatings()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
                      TraktConstants.ShiftvBaseApiUrl,
                      TraktConstants.SyncResource,
                      TraktConstants.RatingsResource,
                      TraktConstants.SeasonsAction));
        }

        public Task<string> SyncEpisodeRatings()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
                    TraktConstants.ShiftvBaseApiUrl,
                    TraktConstants.SyncResource,
                    TraktConstants.RatingsResource,
                    TraktConstants.EpisodesAction));
        }

        public Task<string> SyncMovieRatings()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
                      TraktConstants.ShiftvBaseApiUrl,
                      TraktConstants.SyncResource,
                      TraktConstants.RatingsResource,
                      TraktConstants.MoviesResource));
        }

        public Task<string> UploadRatingsToTrakt()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
                      TraktConstants.ShiftvBaseApiUrl,
                      TraktConstants.SyncResource,
                      TraktConstants.Upload,
                      TraktConstants.RatingsResource));
        }

        public Task<string> UploadWatchedEpisodesToTrakt()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}",
                        TraktConstants.ShiftvBaseApiUrl,
                        TraktConstants.SyncResource,
                        TraktConstants.Upload,
                        TraktConstants.WatchedAction,
                        TraktConstants.EpisodesAction));
        }

        public Task<string> UploadCommentsToTrakt()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
                   TraktConstants.ShiftvBaseApiUrl,
                   TraktConstants.SyncResource,
                   TraktConstants.Upload,
                   TraktConstants.CommentsAction));
        }
    }
}