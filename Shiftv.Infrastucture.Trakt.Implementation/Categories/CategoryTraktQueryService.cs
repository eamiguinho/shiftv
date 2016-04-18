using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Categories;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Categories
{
    class CategoryTraktQueryService : ICategoryTraktQueryService
    {
        public Task<string> GetAllForMovies()
        {
            //http://api.trakt.tv/genres/movies.json/73a66219d4b25eba8b2ef444c2405352
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}",
            TraktConstants.BaseApiUrl,
            TraktConstants.GenresResource,
            TraktConstants.MoviesResource,
            TraktConstants.QueryType,
            TraktConstants.TraktKey));
        }

        public Task<string> GetAllForShows()
        {
            //http://api.trakt.tv/genres/shows.json/73a66219d4b25eba8b2ef444c2405352
            return Task.Run(() => string.Format("{0}/{1}/{2}{3}/{4}",
          TraktConstants.BaseApiUrl,
          TraktConstants.GenresResource,
          TraktConstants.ShowsAction,
          TraktConstants.QueryType,
          TraktConstants.TraktKey));
        }
    }
}
