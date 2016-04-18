using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Helpers;

namespace ShiftvAPI.Contracts.Services.Shows
{
    public interface IShowService
    {
        Task<DataResult<Show>> GetShowById(int id, string token);
        Task<DataResult<List<MiniShow>>> GetTrending(int page, int limit, string token);  
        Task<DataResult<List<MiniShow>>> GetPopular(int page, int limit, string token);
        void UpdateData();
        Task<DataResult<People>> GetPeople(int showId);
        Task<DataResult<List<MiniShow>>> Search(string key);
        Task<DataResult<List<Season>>> GetSeasons(int id, string token);
        Task<DataResult<List<Comment>>> GetComments(int showId, int page, int limit);
        Task<DataResult<List<Comment>>> GetCommentsEpisode(int showId, int season, int episode, int page, int limit);
        Task<DataResult<Season>> GetLastSeason(int showId, string token);
        Task<DataResult<List<ShowProgress>>> GetProgress(string token);
        Task<DataResult<Show>> ForceShowUpdate(int id, string token);
    }
}
