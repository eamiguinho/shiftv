using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Stats;

namespace Shiftv.Contracts.DataServices.Stats
{
    public interface IStatisticsTraktDataService
    {
        Task<IStatistics> GetShowStats(int tvDbId);
        Task<IStatistics> GetEpisodeStats(int tvDbId, int season, int number);
        Task<IStatistics> GetMovieStats(string imdbId);
        Task<bool> PingServer();
        Task<double?> GetImdbRanting(string imdbId);
    }
}