using System;
using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Episodes;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Episodes
{
    class EpisodeTraktQueryService : IEpisodeTraktQueryService
    {
        public Task<string> GetEpisodeBySeason(int tvDbId, int season)
        {
            //http://api.trakt.tv/show/season.format/apikey/title/season
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}/{5}/{6}",
            TraktConstants.BaseApiUrl,
            TraktConstants.ShowResource,
            TraktConstants.SeasonAction,
            TraktConstants.QueryType,
            TraktConstants.TraktKey,
            tvDbId,
            season));
        }

        public Task<string> GetWatchingNow(int tvDbId, int season, int episode)
        {
            //http://api.trakt.tv/show/episode/watchingnow.json/73a66219d4b25eba8b2ef444c2405352/the-walking-dead/1/1
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}/{7}/{8}",
             TraktConstants.BaseApiUrl,
             TraktConstants.ShowResource,
             TraktConstants.EpisodeAction,
             TraktConstants.WatchingnowAction,
             TraktConstants.QueryType,
             TraktConstants.TraktKey,
             tvDbId,
             season,episode));
        }

        public Task<string> GetSetAsSeen()
        {
            //http://api.trakt.tv/show/episode/seen/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/",
            TraktConstants.ShiftvBaseApiUrl,
            TraktConstants.EpisodesAction,
            TraktConstants.SeenMethod));
        }

        public Task<string> GetSetAsUnseen()
        {
            //http://api.trakt.tv/show/episode/unseen/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/",
          TraktConstants.ShiftvBaseApiUrl,
            TraktConstants.EpisodesAction,
            TraktConstants.SeenMethod));
        }

        public Task<string> GetRateEpisode()
        {
            //http://api.trakt.tv/rate/episode/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}",
            TraktConstants.ShiftvBaseApiUrl,
            TraktConstants.EpisodesAction,
            TraktConstants.RateResource));
        }

        public Task<string> GetCheckIn()
        {
            //http://api.trakt.tv/show/checkin/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
            TraktConstants.BaseApiUrl,
            TraktConstants.ShowAction,
            TraktConstants.CheckinMethod,
            TraktConstants.TraktDevKey));
        }

        public Task<string> AddEpisodeToWatchList()
        {
            //http://api.trakt.tv/show/episode/watchlist/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}",
             TraktConstants.BaseApiUrl,
             TraktConstants.ShowAction,
             TraktConstants.EpisodeAction,
             TraktConstants.WatchlistAction,
             TraktConstants.TraktKey));
        }

        public Task<string> RemoveEpisodeFromWatchlist()
        {
              //http://api.trakt.tv/show/episode/watchlist/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}",
             TraktConstants.BaseApiUrl,
             TraktConstants.ShowAction,
             TraktConstants.EpisodeAction,
             TraktConstants.UnwatchlistAction,
             TraktConstants.TraktKey));
        }

        public Task<string> GetRateEpisodes()
        {
            //http://api.trakt.tv/rate/episodes/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
            TraktConstants.BaseApiUrl,
            TraktConstants.RateResource,
            TraktConstants.EpisodesAction,
            TraktConstants.TraktKey));
        }

        public Task<string> GetLinks(string imdbId, int season, int episode, string lang)
        {
            //http://shiftwebservice.azurewebsites.net/api/shows/tt1553656/2/3/eng
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}",
          TraktConstants.ShiftvSubtitlesApiUrl,
          TraktConstants.ApiResource,
          TraktConstants.ShowsAction,
          imdbId, season, episode, lang));
        }

        public Task<string> GetFullEpisodeInfo(int imdbId, int season, int episode)
        {
            //http://api.trakt.tv/show/episode/summary.json/73a66219d4b25eba8b2ef444c2405352/the-league/1/1
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}/{7}/{8}",
           TraktConstants.BaseApiUrl,
           TraktConstants.ShowResource,
           TraktConstants.EpisodeAction,
           TraktConstants.SummaryAction,
           TraktConstants.QueryType,
           TraktConstants.TraktKey, imdbId, season, episode));
        }

        public Task<string> GetAbsoluteNumberFromTvDb(int showTvDbId, DateTime episodeAirDate)
        {
            //http://thetvdb.com/api/GetEpisodeByAirDate.php?apikey=45E75355C2EA5807&seriesid=79824&airdate=2014-09-04
            return Task.Run(() => string.Format("{0}/{1}?apikey={2}&seriesid={3}&airdate={4}",
           TraktConstants.BaseTvDbApiUrl,
           TraktConstants.GetEpisodeByAirDate,
           TraktConstants.TvDbKey,
           showTvDbId,
           episodeAirDate.ToString("yyy-MM-dd")));
        }

        public Task<string> GetCancelCheckIn()
        {
            //http://api.trakt.tv/show/cancelcheckin/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
           TraktConstants.BaseApiUrl,
           TraktConstants.ShowResource,
           TraktConstants.CancelCheckin,
           TraktConstants.TraktDevKey));
        }
    }
}
