using System;
using System.Threading.Tasks;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Movies
{
    public interface IMovieTraktQueryService
    {
        Task<string> GetMoviebyId(int id);
        Task<string> GetPopular(int page, int nItems);
        Task<string> GetTrending(int page, int nItems);
        Task<string> GetUpdates(DateTime startDate, int page, int nItems);
        Task<string> GetPeople(int movieId);
        Task<string> GetComments(int movieId, int page, int nItems);
        Task<string> Search(string key);
    }
}