using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Helpers;

namespace ShiftvAPI.Contracts.Services.Movies
{
    public interface IMovieService
    {
        Task<DataResult<Movie>> GetMovieById(int id, string token);
        Task<DataResult<List<MiniMovie>>> GetTrending(int page, int limit, string token);
        Task<DataResult<List<MiniMovie>>> GetPopular(int page, int limit, string token);
        void UpdateData();
        Task<DataResult<People>> GetPeople(int movieId);
        Task<DataResult<List<MiniMovie>>> Search(string key);
        Task<DataResult<List<Comment>>> GetComments(int movieId, int page, int limit);
        Task<bool> AddToWatchlist(int id, string token);
        Task<DataResult<List<MiniMovie>>> GetWatchlist(string token);
    }
}