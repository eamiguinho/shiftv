using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Stats
{
    public interface IStatisticsTraktQueryService
    {
        Task<string> GetShowStats(int tvDbId);
        Task<string> GetEpisodeStats(int tvDbId, int season, int number);
        Task<string> GetMovieStats(string imdbId);
        Task<string> PingServer();
        Task<string> GetImdbRating(string imdbId);
    }
}
