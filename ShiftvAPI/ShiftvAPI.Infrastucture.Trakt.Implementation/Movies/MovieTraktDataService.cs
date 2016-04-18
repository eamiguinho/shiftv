using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Movies;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Shows;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Movies
{
   public class MovieTraktDataService : IMovieTraktDataService
    {
       private IMovieTraktQueryService _queryService
            ;

        public MovieTraktDataService(IMovieTraktQueryService queryService)
        {
            _queryService = queryService;
        }

        public Task<Movie> GetMovieById(int id)
       {
           return Task.Run(async () =>
           {
               try
               {
                   //throw new Exception("BOOM");
                   var url = await _queryService.GetMoviebyId(id);
                   Movie x = await TraktDataServiceHelper.GetObjectWithoutCredentials<Movie>(url);
                   return x;
               }
               catch (Exception)
               {
                   return null;
               }
           });
       }

       public Task<List<Movie>> GetPopular(int page, int nItems)
       {
           return Task.Run(async () =>
           {
               try
               {
                   var url = await _queryService.GetPopular(page, nItems);
                   List<Movie> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<Movie>>(url);
                   return x;
               }
               catch (Exception)
               {
                   return null;
               }
           });
       }

       public Task<List<MovieTrending>> GetTrending(int page, int nItems)
       {
           return Task.Run(async () =>
           {
               try
               {
                   var url = await _queryService.GetTrending(page, nItems);
                   var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MovieTrending>>(url);
                   return x;
               }
               catch (Exception)
               {
                   return null;
               }
           });
       }

       public Task<List<MovieUpdate>> GetUpdates(int page, int nItems, DateTime startDate)
       {
           return Task.Run(async () =>
           {
               try
               {
                   var url = await _queryService.GetUpdates(startDate, page, nItems);
                   List<MovieUpdate> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MovieUpdate>>(url);
                   return x;
               }
               catch (Exception)
               {
                   return null;
               }
           });
       }

       public Task<People> GetPeople(int movieId)
       {
           return Task.Run(async () =>
           {
               try
               {
                   var url = await _queryService.GetPeople(movieId);
                   People x = await TraktDataServiceHelper.GetObjectWithoutCredentials<People>(url);
                   return x;
               }
               catch (Exception)
               {
                   return null;
               }
           });
       }

       public Task<List<Comment>> GetComments(int page, int nItems, int movieId)
       {
           return Task.Run(async () =>
           {
               try
               {
                   var url = await _queryService.GetComments(movieId, page, nItems);
                   List<Comment> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<Comment>>(url);
                   return x;
               }
               catch (Exception)
               {
                   return null;
               }
           });
       }

       public Task<List<MovieSearchResult>> Search(string key)
       {
           return Task.Run(async () =>
           {
               try
               {
                   var url = await _queryService.Search(key);
                   List<MovieSearchResult> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<MovieSearchResult>>(url);
                   return x;
               }
               catch (Exception)
               {
                   return null;
               }
           });
       }

       public Task AddToWatchlist(int id, string token)
       {
           throw new NotImplementedException();
       }
    }
}
