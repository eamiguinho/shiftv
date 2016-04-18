using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Contracts.Services.Stats;
using Shiftv.Global;

namespace Shiftv.DesignServices.Implementation
{
    public class StatisticsDesignService : IStatisticsService
    {
        public Task<DataResult<IStatistics>> GetShowStats(int tvDbId)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.ShowStats.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var statsDto = JsonConvert.DeserializeObject<StatsDto>(jsonString);
                return new DataResult<IStatistics>(StatsDtoFactory.Create(statsDto));
            });
        }

        public Task<DataResult<IStatistics>> GetEpisodeStats(int season, int number)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetEpisodeStats.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var statsDto = JsonConvert.DeserializeObject<StatsDto>(jsonString);
                return new DataResult<IStatistics>(StatsDtoFactory.Create(statsDto));
            });
        }

        public Task<DataResult<IStatistics>> GetMoviewStats(string imdbId)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.ShowStats.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var statsDto = JsonConvert.DeserializeObject<StatsDto>(jsonString);
                return new DataResult<IStatistics>(StatsDtoFactory.Create(statsDto));
            });
        }

        public Task<DataResult<bool>> PingServer()
        {
            return Task.Run(() =>
            {
                return new DataResult<bool>(true);
            });
        }

        public Task<DataResult<double?>> GetImdbRanting(string imdbId)
        {
            throw new NotImplementedException();
        }
    }
}
