using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Sync
{
    public interface ISyncTraktQueryService
    {
        Task<string> GetRatings(RequestType requestType);
        Task<string> GetStats();
        Task<string> GetWatched(RequestType requestType);
        Task<string> GetSyncRatings();
        Task<string> GetSyncWatched();
        Task<string> GetUploadComment();
        Task<string> GetSyncUnwatched();
    }
}