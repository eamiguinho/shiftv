using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Shows
{
    public interface IShowTraktDataService
    {
        Task<Show> GetShowById(int id);
        Task<List<Show>> GetPopular(int page, int nItems);
        Task<List<ShowTrending>> GetTrending(int page, int nItems);
        Task<List<ShowUpdate>> GetUpdates(int page, int nItems, DateTime startDate);
        Task<People> GetPeople(int showId);
        Task<List<Comment>> GetComments(int page, int nItems, int showId);
        Task<List<Season>> GetSeasons(int showId);
        Task<List<Episode>> GetEpisodesBySeason(int showId, int season);
        Task<List<ShowSearchResult>> Search(string key);
        Task<List<Comment>> GetCommentsEpisode(int page, int limit, int showId, int season, int episode);
        Task<List<Calendar>> GetCalendar(string token);
    }
}
                                      