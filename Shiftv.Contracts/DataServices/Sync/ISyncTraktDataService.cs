using System.Threading.Tasks;
using Shiftv.Contracts.Data.Users;

namespace Shiftv.Contracts.DataServices.Sync
{
    public interface ISyncTraktDataService
    {
        Task<bool> SyncWatchedShows(UserTokenDto userTokenDto);
        Task<bool> SyncWatchedMovies(UserTokenDto userTokenDto);
        Task<bool> SyncShowRatings(UserTokenDto userTokenDto);
        Task<bool> SyncSeasonRatings(UserTokenDto userTokenDto);
        Task<bool> SyncEpisodeRatings(UserTokenDto userTokenDto);
        Task<bool> SyncMovieRatings(UserTokenDto userTokenDto);
        Task<bool> UploadRatingsToTrakt(UserTokenDto userTokenDto);
        Task<bool> UploadWatchedEpisodesToTrakt(UserTokenDto userTokenDto);
        Task<bool> UploadCommentsToTrakt(UserTokenDto userTokenDto);
    }
}