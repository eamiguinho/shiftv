using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Shiftv
{
    public interface IMovieShiftvDataService
    {
        Task<Movie> GetMovieById(int id);
        Task SaveMovie(Movie showData);
        Task<List<MiniMovie>> GetTrending(int page, int limit);
        Task SaveTrending(List<MiniMovie> listMini);
        Task<List<MiniMovie>> GetPopular(int page, int limit);
        Task SavePopular(List<MiniMovie> listMini);
        DateTime? GetLastUpdate();
        void SaveLastUpdate(DateTime now);
        Task<People> GetPeople(int movieId);
        Task SavePeople(People peopleData, int movieId);
        Task<List<MiniMovie>> Search(string key);
        Task<List<Comment>> GetComments(int movieId);
        void UpdateWatchlist (List<MiniMovie> id, string token);
        List<MiniMovie> GetUserWatchlist(string token);
    }
}