using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Data.Sync;
using ShiftvAPI.Contracts.Helpers;

namespace ShiftvAPI.Contracts.Services.Sync
{
    public interface ISyncService
    {
        Task<DataResult<List<RatingSync>>> GetRatings(string token, RequestType episodes);
        Task<DataResult<SyncStats>> GetStats(string token);
        Task<DataResult<List<SyncWatched>>> GetWatchedShows(string token);
        Task<DataResult<List<SyncWatched>>> GetWatchedMovies(string token);
        bool Rate(RateRequestJsonDto rateRequest, string token, RequestType type);
        bool RateFireForget(RateRequestJsonDto rateRequest, string token, RequestType type);
        List<RateResultJsonDto> RateFireForget(List<RateRequestJsonDto> rateRequest, string token, RequestType type);

        bool Comment(CommentRequestJsonDto commentRequest, string token, RequestType shows);
        bool CommentFireForge(CommentRequestJsonDto commentRequest, string token, RequestType type);
        bool SetAsSeen(SetAsSeenJson setAsSeenRequest, string token, RequestType movies);
        bool SetAsSeenFireForget(SetAsSeenJson setAsSeenRequest, string token, RequestType type);
        List<RateResultJsonDto> Rate(List<RateRequestJsonDto> rateRequest, string token, RequestType episodes);
        List<SetAsSeenResultJson> SetAsSeen(List<SetAsSeenJson> setAsSeenRequest, string token, RequestType type);

        List<SetAsSeenResultJson> SetAsSeenFireForget(List<SetAsSeenJson> setAsSeenRequest, string token,
            RequestType type);
        Task<DataResult> GetUploadRatings(string token);
        Task<DataResult> GetUploadWatchedEpisodes(string token);
        Task<DataResult> GetUploadComments(string token);
    }
}   