using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Movies
{
    public interface IMovieTraktDataService
    {
        Task<Movie> GetMovieById(int id);
        Task<List<Movie>> GetPopular(int page, int nItems);
        Task<List<MovieTrending>> GetTrending(int page, int nItems);
        Task<List<MovieUpdate>> GetUpdates(int page, int nItems, DateTime startDate);
        Task<People> GetPeople(int movieId);
        Task<List<Comment>> GetComments(int page, int nItems, int movieId);
        Task<List<MovieSearchResult>> Search(string key);
        Task AddToWatchlist(int id, string token);
    }
}