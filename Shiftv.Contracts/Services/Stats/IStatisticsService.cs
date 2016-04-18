using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Stats
{
    public interface IStatisticsService
    {
        Task<DataResult<IStatistics>> GetShowStats(int tvDbId);
        Task<DataResult<IStatistics>> GetEpisodeStats(int season, int number);
        Task<DataResult<IStatistics>> GetMoviewStats(string imdbId);
        Task<DataResult<bool>> PingServer();
        //Task<DataResult<double?>> GetImdbRanting(string imdbId);
    }
}