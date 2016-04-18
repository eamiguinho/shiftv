using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Shows;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Shows
{
    public class ShowTraktQueryService : IShowTraktQueryService
    {
        public Task<string> GetTrending()
        {
            //"http://api.trakt.tv/shows/trending.json/" + TraktConstants.TraktKey;
            return Task.Run(() => string.Format("{0}/{1}/{2}", 
               TraktConstants.ShiftvBaseApiUrl,
               TraktConstants.ShowsResource,
               TraktConstants.TredingAction));
        }

        public Task<string> GetByImdbId(int imdbId)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}",
                TraktConstants.ShiftvBaseApiUrl, 
                TraktConstants.ShowsResource,
                imdbId));
        }

        public Task<string> GetRecommendations()
        {
            //http://api.trakt.tv/recommendations/shows/apikey
             return Task.Run(() => string.Format("{0}/{1}/{2}/{3}", 
               TraktConstants.BaseApiUrl, 
               TraktConstants.RecommendationsResource,
               TraktConstants.ShowsAction,
               TraktConstants.TraktKey));
        }

        public Task<string> GetSearchByKey(string key)
        {
            //http://api.trakt.tv/search/shows.format/apikey?query=query&limit=limit&seasons=seasons
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
              TraktConstants.ShiftvBaseApiUrl,
              TraktConstants.ShowsAction,
              TraktConstants.SearchResource,
              key.Replace(" ", "-")));
        }

        public Task<string> GetAllShowGenres()
        {
            //http://api.trakt.tv/genres/shows.json/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}",
              TraktConstants.BaseApiUrl,
              TraktConstants.GenresResource,
              TraktConstants.ShowsAction,
              TraktConstants.QueryType,
              TraktConstants.TraktKey));
        }

        public Task<string> RateShow()
        {
            //http://api.trakt.tv/rate/show/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/",
          TraktConstants.ShiftvBaseApiUrl,
          TraktConstants.ShowsAction,
          TraktConstants.RateResource));
        }

        public Task<string> AddShowToWatchList()
        {
            //http://api.trakt.tv/show/watchlist/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
        TraktConstants.BaseApiUrl,
        TraktConstants.ShowResource,
        TraktConstants.WatchlistAction,
        TraktConstants.TraktKey));
        }

        public Task<string> RemoveShowFromWatchList()
        {
            //http://api.trakt.tv/show/unwatchlist/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
        TraktConstants.BaseApiUrl,
        TraktConstants.ShowResource,
        TraktConstants.UnwatchlistAction,
        TraktConstants.TraktKey));
        }

        public Task<string> GetShowsWatchlistByUser(string username)
        {
            //http://api.trakt.tv/user/watchlist/shows.json/73a66219d4b25eba8b2ef444c2405352/justin
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}",
        TraktConstants.BaseApiUrl,
        TraktConstants.UserResource,
        TraktConstants.WatchlistAction,
        TraktConstants.ShowsAction,
        TraktConstants.QueryType,
        TraktConstants.TraktKey,
        username));
        }

        public Task<string> GetShowsWithEpisodesWatchlistByUser(string username)
        {
            //http://api.trakt.tv/user/watchlist/episodes.json/73a66219d4b25eba8b2ef444c2405352/justin
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}",
        TraktConstants.BaseApiUrl,
        TraktConstants.UserResource,
        TraktConstants.WatchlistAction,
        TraktConstants.EpisodesAction,
        TraktConstants.QueryType,
        TraktConstants.TraktKey,
        username));
        }

        public Task<string> GetAnimeList()
        {
            return Task.Run( () => string.Format("{0}/{1}/amiguinho/anime/show", TraktConstants.ShiftvBaseApiUrl, TraktConstants.Lists));
        }

        public Task<string> GetImdbRating(string imdbId)
        {
            //http://www.omdbapi.com/?i=tt2234222
            return Task.Run(() => string.Format("{0}{1}",
        TraktConstants.OmdbApi,
        imdbId));
        }

        public Task<string> GetShowProgress()
        {
            //http://api.trakt.tv/user/progress/watched.json/f6b5b84c5eadfe18f57dbdc7d489f85b/amiguinho/all/title/normal
            return Task.Run(() => string.Format("{0}/{1}/{2}",
        TraktConstants.ShiftvBaseApiUrl,
        TraktConstants.ShowsAction,
        TraktConstants.ProgressAction
        ));
        }

        public Task<string> GetLovedByUser(string username)
        {
            //http://api.trakt.tv/user/ratings/shows.json/73a66219d4b25eba8b2ef444c2405352/amiguinho/love/full
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}/{7}/{8}",
       TraktConstants.BaseApiUrl,
       TraktConstants.UserResource,
       TraktConstants.RatingsResource,
       TraktConstants.ShowsAction,
       TraktConstants.QueryType,
       TraktConstants.TraktKey,
       username, "love", "full"
       ));
        }

        public Task<string> GetTrendingV2()
        {
            //http://api.v2.trakt.tv/shows/popular?extended={full,images}
            return Task.Run(() => string.Format("{0}/{1}/{2}",
              TraktConstants.BaseApiV2Url,
              TraktConstants.ShowsResource,
              TraktConstants.PopularAction));
        }

        public Task<string> GetTopImdb()
        {
            return Task.Run(() => string.Format("{0}/{1}/schumi2007/857937/show", TraktConstants.ShiftvBaseApiUrl, TraktConstants.Lists));
            //return Task.Run(() => string.Format("{0}/{1}/list.json/{2}/mmounirou/imdb-highest-rated-tv-series", TraktConstants.BaseApiUrl, TraktConstants.UserResource, TraktConstants.TraktKey));
        }

        public Task<string> GetPeople(int imdbId)
        {
            //http://api.v2.trakt.tv/shows/popular?extended={full,images}
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
              TraktConstants.ShiftvBaseApiUrl,
              TraktConstants.ShowsResource,
              TraktConstants.People,
              imdbId));
        }

        public Task<string> GetPopular()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}",
              TraktConstants.ShiftvBaseApiUrl,
              TraktConstants.ShowsResource,
              TraktConstants.PopularAction));
        }

        public Task<string> ForceUpdate(int value)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
       TraktConstants.ShiftvBaseApiUrl,
       TraktConstants.ShowsResource,
       TraktConstants.ForceResource,
       value));
        }
    }
}
