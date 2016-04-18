using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Data.Sync;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Sync
{
    public interface ISyncTraktDataService
    {
        Task<List<RatingSync>> GetRatings(string token, RequestType requestType);
        Task<SyncStats> GetStats(string token);
        Task<List<SyncWatched>> GetWatched(string token, RequestType requestType);
        Task<bool> UploadRatings(UploadRating ratings, string token);
        Task<bool> UploadWatched(UploadWatched listWatched, string token);
        Task<bool> UploadComment(string commentRequestJsonDto, string token);
        Task<bool> UploadUnwatched(UploadWatched listWatched, string token);
    }
}