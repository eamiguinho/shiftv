using System;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Shows
{
    public interface IShowTraktQueryService
    {
        Task<string> GetShowById(int id);
        Task<string> GetPopular(int page, int nItems);
        Task<string> GetTrending(int page, int nItems);
        Task<string> GetUpdates(DateTime startDate, int page, int nItems);
        Task<string> GetPeople(int showId);
        Task<string> GetComments(int showId, int page, int nItems);
        Task<string> GetSeasons(int showId);
        Task<string> GetEpisodesBySeason(int showId, int season);
        Task<string> Search(string key);
        Task<string> GetCommentsEpisode(int showId, int season, int episode, int page, int limit);
        Task<string> GetCalendar(DateTime date, int numDays);
    }
}
