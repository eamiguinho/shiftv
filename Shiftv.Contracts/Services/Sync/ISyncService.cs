using System.Threading.Tasks;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Sync
{
    public interface ISyncService
    {
        Task<DataResult> SyncWatchedShows();
        Task<DataResult> SyncWatchedMovies();
        Task<DataResult> SyncShowRatings();
        Task<DataResult> SyncSeasonRatings();
        Task<DataResult> SyncEpisodeRatings();
        Task<DataResult> SyncMovieRatings();
        Task<DataResult> UploadRatingsToTrakt();
        Task<DataResult> UploadWatchedEpisodesToTrakt();
        Task<DataResult> UploadCommentsToTrakt();
    }
}