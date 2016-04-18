using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Movies;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Movies
{
    public class MovieTraktQueryService : IMovieTraktQueryService
    {
        public Task<string> GetTredingQuery()
        {
            //"http://api.trakt.tv/movies/trending.json/" + TraktConstants.TraktKey;
            return Task.Run(() => string.Format("{0}/{1}/{2}",
                TraktConstants.ShiftvBaseApiUrl,
                TraktConstants.MoviesResource,
                TraktConstants.TredingAction));
        }

        public Task<string> GetSearchMoviesByKey(string key)
        {
            //http://api.trakt.tv/search/movies.format/apikey?query=query&limit=limit
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
              TraktConstants.ShiftvBaseApiUrl,
              TraktConstants.MoviesResource,
              TraktConstants.SearchResource,
              key.Replace(" ","-")));
        }

        public Task<string> GetRecommendations()
        {
            // http://api.trakt.tv/recommendations/movies/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
              TraktConstants.BaseApiUrl,
              TraktConstants.RecommendationsResource,
              TraktConstants.MoviesResource,
              TraktConstants.TraktKey));

        }

        public Task<string> RateMovie()
        {
            //http://api.trakt.tv/rate/movie/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}",
                TraktConstants.ShiftvBaseApiUrl,
                TraktConstants.MoviesResource,
                TraktConstants.RateResource));
        }

        public Task<string> GetByImdbId(int imdbId)
        {
            //http://api.trakt.tv/movie/summary.format/apikey/title
            return Task.Run(() => string.Format("{0}/{1}/{2}",
                TraktConstants.ShiftvBaseApiUrl,
                TraktConstants.MoviesResource,
                imdbId));
        }

        public Task<string> GetCheckIn()
        {
            //http://api.trakt.tv/movie/checkin/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
                TraktConstants.BaseApiUrl,
                TraktConstants.MovieResource,
                TraktConstants.CheckinMethod,
                TraktConstants.TraktDevKey));
        }

        public Task<string> GetUserWatchlist()
        {
            //http://api.trakt.tv/user/watchlist/movies.json/73a66219d4b25eba8b2ef444c2405352/justin
            return Task.Run(() => string.Format("{0}/{1}/{2}/",
                TraktConstants.ShiftvBaseApiUrl,
                TraktConstants.MoviesResource,
                TraktConstants.WatchlistAction));
        }

        public Task<string> GetLovedByUser(string username)
        {
            //http://api.trakt.tv/user/ratings/movies.json/73a66219d4b25eba8b2ef444c2405352/amiguinho/love/full
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}{4}/{5}/{6}/{7}/{8}",
      TraktConstants.BaseApiUrl,
      TraktConstants.UserResource,
      TraktConstants.RatingsResource,
      TraktConstants.MoviesResource,
      TraktConstants.QueryType,
      TraktConstants.TraktKey,
      username, "love", "full"
      ));
        }

        public Task<string> GetLinks(string imdbId, string language)
        {
            //http://shiftwebservice.azurewebsites.net/api/movies/tt1553656/eng
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}/{4}",
          TraktConstants.ShiftvSubtitlesApiUrl,
          TraktConstants.ApiResource,
          TraktConstants.MoviesResource,
          imdbId, language));
        }

        public Task<string> GetSetAsSeen()
        {
            //http://api.trakt.tv/movie/seen/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}",
            TraktConstants.ShiftvBaseApiUrl,
            TraktConstants.MoviesResource,
            TraktConstants.SeenMethod));
        }

        public Task<string> GetSetAsUnseen()
        {
            //http://api.trakt.tv/movie/unseen/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
            TraktConstants.BaseApiUrl,
            TraktConstants.MovieResource,
            TraktConstants.UnseenMethod,
            TraktConstants.TraktKey));
        }

        public Task<string> AddMovieToWatchList()
        {
            //http://api.trakt.tv/movie/watchlist/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}",
          TraktConstants.ShiftvBaseApiUrl,
          TraktConstants.MoviesResource,
          TraktConstants.WatchlistAction));
        }

        public Task<string> RemoveMovieFromWatchlist()
        {
            //http://api.trakt.tv/movie/unwatchlist/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}",
          TraktConstants.ShiftvBaseApiUrl,
          TraktConstants.MoviesResource,
          TraktConstants.WatchlistAction));
        }

        public Task<string> GetImdbRating(string imdbId)
        {
            //http://www.omdbapi.com/?i=tt2234222
            return Task.Run(() => string.Format("{0}{1}",
        TraktConstants.OmdbApi,
        imdbId));
        }

        public Task<string> GetCancelCheckIn()
        {
            //http://api.trakt.tv/movie/cancelcheckin/apikey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
          TraktConstants.BaseApiUrl,
          TraktConstants.MovieResource,
          TraktConstants.CancelCheckin,
          TraktConstants.TraktDevKey));
        }

        public Task<string> GetTopImdb()
        {
            return Task.Run(() => string.Format("{0}/{1}/jaywen/840408/movie", TraktConstants.ShiftvBaseApiUrl, TraktConstants.Lists));
          

        }

        public Task<string> GetAnimationMovies()
        {
            //animation-movies
            return Task.Run(() => string.Format("{0}/{1}/amiguinho/animation-movies/movie", TraktConstants.ShiftvBaseApiUrl, TraktConstants.Lists));
        }

        public Task<string> GetPopular()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}", TraktConstants.ShiftvBaseApiUrl, TraktConstants.MoviesResource, TraktConstants.PopularAction));
        }

        public Task<string> GetPeople(int imdbId)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}",
            TraktConstants.ShiftvBaseApiUrl,
            TraktConstants.MoviesResource,
            TraktConstants.People,
            imdbId));
        }

        public Task<string> GetOscars()
        {
            return Task.Run(() => string.Format("{0}/{1}/deano88/oscar-predictions-2016/movie", TraktConstants.ShiftvBaseApiUrl, TraktConstants.Lists));
        }

        public Task<string> GetChristmas()
        {
            return Task.Run(() => string.Format("{0}/{1}/littlestella3/christmas/movie", TraktConstants.ShiftvBaseApiUrl, TraktConstants.Lists));
        }
    }
}
