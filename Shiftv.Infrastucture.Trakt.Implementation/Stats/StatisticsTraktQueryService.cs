using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Stats;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Stats
{
    class StatisticsTraktQueryService : IStatisticsTraktQueryService
    {
        public Task<string> GetShowStats(int tvDbId)
        {
            //http://api.trakt.tv/show/stats.json/73a66219d4b25eba8b2ef444c2405352/the-walking-dead
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}/{5}",
            TraktConstants.BaseApiUrl,
            TraktConstants.ShowResource,
            TraktConstants.StatsMethod,
            TraktConstants.QueryType,
            TraktConstants.TraktKey,
            tvDbId));
        }

        public Task<string> GetEpisodeStats(int tvDbId, int season, int number)
        {
            //http://api.trakt.tv/show/episode/stats.json/73a66219d4b25eba8b2ef444c2405352/the-walking-dead/1/1
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}/{7}/{8}",
            TraktConstants.BaseApiUrl,
            TraktConstants.ShowResource,
            TraktConstants.EpisodeAction,
            TraktConstants.StatsMethod,
            TraktConstants.QueryType,
            TraktConstants.TraktKey,
            tvDbId,
            season, number));
        }

        public Task<string> GetMovieStats(string imdbId)
        {
            //http://api.trakt.tv/movie/stats.format/apikey/title
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}/{5}",
     TraktConstants.BaseApiUrl,
     TraktConstants.MovieResource,
     TraktConstants.StatsMethod,
     TraktConstants.QueryType,
     TraktConstants.TraktKey,
     imdbId));
        }

        public Task<string> PingServer()
        {
            //http://api.trakt.tv/server/time.json/73a66219d4b25eba8b2ef444c2405352
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}/",
   TraktConstants.BaseApiUrl,
   TraktConstants.ServerResource,
   TraktConstants.TimeAction,
   TraktConstants.QueryType,
   TraktConstants.TraktKey));
        }

        public Task<string> GetImdbRating(string imdbId)
        {
            //http://www.omdbapi.com/?i=tt2234222
            return Task.Run(() => string.Format("{0}{1}",
        TraktConstants.OmdbApi,
        imdbId));
        }
    }
}