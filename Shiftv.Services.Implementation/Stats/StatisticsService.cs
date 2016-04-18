using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Stats;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Contracts.Services.Movies;
using Shiftv.Contracts.Services.Shows;
using Shiftv.Contracts.Services.Stats;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Stats
{
    class StatisticsService : ServiceHelper, IStatisticsService
    {
        private IStatisticsTraktDataService _statsDataService;
        private IShowService _showService;
        private IMovieService _movieService;

        public StatisticsService(IStatisticsTraktDataService statisticsTraktDataService = null, IShowService showService = null, IMovieService movieService = null)
        {
            _showService = showService;
            _movieService = movieService;
            _statsDataService = statisticsTraktDataService;
        }
            
        public async Task<DataResult<IStatistics>> GetShowStats(int tvDbId)
        {
            if (tvDbId <= -1) return new DataResult<IStatistics>(StandardResults.Error);
           // //if (!await IsInternet()) return new DataResult<IStatistics>(StandardResults.Offline);
            var req = await _statsDataService.GetShowStats(tvDbId);
            if (req == null) return new DataResult<IStatistics>(StandardResults.Error);
            return new DataResult<IStatistics>(req);
        }

        public async Task<DataResult<IStatistics>> GetEpisodeStats(int season, int number)
        {
            if (season <= -1 || season <= -1 || number <= -1) return new DataResult<IStatistics>(StandardResults.Error);
           // //if (!await IsInternet()) return new DataResult<IStatistics>(StandardResults.Offline);
            var show = _showService.GetCurrentShow();
            if (show == null) return new DataResult<IStatistics>(StandardResults.Error);
            var req = await _statsDataService.GetEpisodeStats(show.Ids.TvDbId.Value, season, number);
            return req == null ? new DataResult<IStatistics>(StandardResults.Error) : new DataResult<IStatistics>(req);
        }

        public async Task<DataResult<IStatistics>> GetMoviewStats(string imdbId)
        {
           // //if (!await IsInternet()) return new DataResult<IStatistics>(StandardResults.Offline);
            if (string.IsNullOrEmpty(imdbId)) return new DataResult<IStatistics>(StandardResults.Error);
            var req = await _statsDataService.GetMovieStats(imdbId);
            return req == null ? new DataResult<IStatistics>(StandardResults.Error) : new DataResult<IStatistics>(req);
        }

        public async Task<DataResult<bool>> PingServer()
        {
            return new DataResult<bool>(StandardResults.Ok);
            var req = await _statsDataService.PingServer();
            return req == false ? new DataResult<bool>(StandardResults.Error) : new DataResult<bool>(StandardResults.Ok);
        }

        public async Task<DataResult<IStatistics>> GetEpisodeStats(IShow show, IEpisode episode)
        {
            if (show  == null || episode == null) return new DataResult<IStatistics>(StandardResults.Error);
           // //if (!await IsInternet()) return new DataResult<IStatistics>(StandardResults.Offline);
            var req = await _statsDataService.GetEpisodeStats(show.Ids.TvDbId.Value, episode.Season, episode.Number);
            return req == null ? new DataResult<IStatistics>(StandardResults.Error) : new DataResult<IStatistics>(req);
        }

        public async Task<DataResult<IStatistics>> GetMovieStats()
        {
           // //if (!await IsInternet()) return new DataResult<IStatistics>(StandardResults.Offline);
            var movie = _movieService.GetCurrentMovie();
            if (movie == null) return new DataResult<IStatistics>(StandardResults.Error);
            var req = await _statsDataService.GetMovieStats(movie.Ids.ImdbId);
            return req == null ? new DataResult<IStatistics>(StandardResults.Error) : new DataResult<IStatistics>(req);
        }

        //public async Task<DataResult<double?>> GetImdbRanting(string imdbId)
        //{
        //    //if (!await IsInternet()) return new DataResult<double?>(StandardResults.Offline);
        //    var res = await _statsDataService.GetImdbRanting(imdbId);
        //    if (res == null) return new DataResult<double?>(StandardResults.Error);
        //    return new DataResult<double?>(res);
        //}
    }
}
