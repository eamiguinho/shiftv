using System;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Movies;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Shows;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers.Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Movies
{
    class MovieTraktQueryService : IMovieTraktQueryService
    {
        public Task<string> GetMoviebyId(int id)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}?{3}",
               TraktConstants.BaseApiUrl,
               TraktConstants.MoviesResource,
               id,
               TraktConstants.ExtendedImagesData));
        }

        public Task<string> GetPopular(int page, int nItems)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}?{3}={4}&{5}={6}&{7}",
                TraktConstants.BaseApiUrl,
                TraktConstants.MoviesResource,
                TraktConstants.PopularAction,
                TraktConstants.Page,
                page,
                TraktConstants.Limit,
                nItems,
                TraktConstants.ExtendedImagesData
                ));
        }

        public Task<string> GetTrending(int page, int nItems)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}?{3}={4}&{5}={6}&{7}",
                TraktConstants.BaseApiUrl,
                TraktConstants.MoviesResource,
                TraktConstants.TredingAction,
                TraktConstants.Page,
                page,
                TraktConstants.Limit,
                nItems,
                TraktConstants.ExtendedImagesData
                ));
        }

        public Task<string> GetUpdates(DateTime startDate, int page, int nItems)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}?{4}={5}&{6}={7}&{8}",
                  TraktConstants.BaseApiUrl,
                  TraktConstants.MoviesResource,
                  TraktConstants.Updates,
                  startDate.Date.ToString("yyyy-MM-dd"),
                  TraktConstants.Page,
                  page,
                  TraktConstants.Limit,
                  nItems,
                  TraktConstants.ExtendedImagesData
                  ));
        }

        public Task<string> GetPeople(int movieId)
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}?{4}",
                  TraktConstants.BaseApiUrl,
                  TraktConstants.MoviesResource,
                 movieId,
                  TraktConstants.People,
                  TraktConstants.ExtendedImagesData
                  ));
        }


        public Task<string> GetComments(int showId, int page, int nItems)
        {

            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}?{4}&{5}={6}&{7}={8}",
                  TraktConstants.BaseApiUrl,
                  TraktConstants.MoviesResource,
                  showId,
                  TraktConstants.CommentsResource,
                  TraktConstants.ExtendedImagesData,
                  TraktConstants.Page,
                  page,
                  TraktConstants.Limit,
                  nItems
                  ));
        }

        public Task<string> Search(string key)
        {
            //https://api.trakt.tv/search?query=batman&type=type
            return Task.Run(() => string.Format("{0}/{1}?query={2}&type=movie&{3}",
           TraktConstants.BaseApiUrl,
           TraktConstants.SearchResource,
          key, TraktConstants.ExtendedImagesData));
        }
    }
}