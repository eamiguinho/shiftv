using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Sync
{
    public interface ISyncTraktQueryService
    {
        Task<string> SyncWatchedShows();
        Task<string> SyncWatchedMovies();
        Task<string> SyncShowRatings();
        Task<string> SyncSeasonRatings();
        Task<string> SyncEpisodeRatings();
        Task<string> SyncMovieRatings();
        Task<string> UploadRatingsToTrakt();
        Task<string> UploadWatchedEpisodesToTrakt();
        Task<string> UploadCommentsToTrakt();
    }
}