using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Hangfire;
using Newtonsoft.Json;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Services;
using ShiftvAPI.Contracts.Services.Movies;
using ShiftvAPI.Contracts.Services.Shows;
using ShiftvAPI.Contracts.Services.Sync;

namespace ShiftvAPI.Controllers
{
    [RoutePrefix("Movies")]
    public class MoviesController : ApiController
    {

        [Route("{id}")]
        public async Task<Movie> Get(int id)
        {
            var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
            var service = Ioc.Container.Resolve<IMovieService>();
            var res = await service.GetMovieById(id, token);
            return res.Data;
        }

        [Route("People/{movieId}")]
        public async Task<People> GetPeople(int movieId)
        {
            var service = Ioc.Container.Resolve<IMovieService>();
            var res = await service.GetPeople(movieId);
            return res.Data;
        }

        [Route("Popular")]
        public async Task<List<MiniMovie>> GetPopular(int page = 1, int limit = 150)
        {
            var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
            var service = Ioc.Container.Resolve<IMovieService>();
            var res = await service.GetPopular(page, limit, token);
            return res.Data;
        }

      
        [Route("Trending")]
        public async Task<List<MiniMovie>> GetTrending(int page = 1, int limit = 150)
        {
            var token = "";
            if(this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
            var service = Ioc.Container.Resolve<IMovieService>();
            var res = await service.GetTrending(page, limit, token);
            return res.Data;
        }
         [HttpGet]
        [Route("Search/{key}")]
        public async Task<List<MiniMovie>> Search(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            var service = Ioc.Container.Resolve<IMovieService>();
            var res = await service.Search(key);
            return res.Data;
        }

          [HttpGet]
        [Route("Update")]
        public void UpdateData()
        {
            var service = Ioc.Container.Resolve<IMovieService>();
            service.UpdateData();
        }

          [Route("Comments/{showId}")]
          public async Task<List<Comment>> GetComments(int showId, int page = 1, int limit = 150)
          {
              var service = Ioc.Container.Resolve<IMovieService>();
              var res = await service.GetComments(showId, page, limit);
              return res.Data;
          }

          [HttpPost]
          [Route("Rate")]
          public async Task<HttpResponseMessage> Rate(HttpRequestMessage request)
          {
              try
              {
                  var token = "";
                  if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
                  if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
                  var jsonString = await request.Content.ReadAsStringAsync();
                  var rateRequest = JsonConvert.DeserializeObject<RateRequestJsonDto>(jsonString);
                  var service = Ioc.Container.Resolve<ISyncService>();
                  var res = service.Rate(rateRequest, token, RequestType.Movies);
                  if (res) BackgroundJob.Enqueue(() => service.RateFireForget(rateRequest, token, RequestType.Movies));
                  if (!res) return new HttpResponseMessage(HttpStatusCode.PreconditionFailed);
                  return new HttpResponseMessage(HttpStatusCode.Created);
              }
              catch (Exception e)
              {
                  return new HttpResponseMessage(HttpStatusCode.InternalServerError);
              }
          }


          [HttpPost]
          [Route("Comment")]
          public async Task<HttpResponseMessage> Comment(HttpRequestMessage request)
          {
              try
              {
                  var token = "";
                  if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
                  if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
                  var jsonString = await request.Content.ReadAsStringAsync();
                  var commentRequest = JsonConvert.DeserializeObject<CommentRequestJsonDto>(jsonString);
                  var service = Ioc.Container.Resolve<ISyncService>();
                  var res = service.Comment(commentRequest, token, RequestType.Movies);
                  if (res) BackgroundJob.Enqueue(() => service.CommentFireForge(commentRequest, token, RequestType.Movies));

                  if (!res) return new HttpResponseMessage(HttpStatusCode.PreconditionFailed);
                  return new HttpResponseMessage(HttpStatusCode.Created);
              }
              catch (Exception e)
              {
                  return new HttpResponseMessage(HttpStatusCode.InternalServerError);
              }
          }

          [HttpPost]
          [Route("Seen")]
          public async Task<HttpResponseMessage> Seen(HttpRequestMessage request)
          {
              try
              {
                  var token = "";
                  if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
                  if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
                  var jsonString = await request.Content.ReadAsStringAsync();
                  var setAsSeenRequest = JsonConvert.DeserializeObject<SetAsSeenJson>(jsonString);
                  var service = Ioc.Container.Resolve<ISyncService>();
                  var res = service.SetAsSeen(setAsSeenRequest, token, RequestType.Movies);
                  if (res) BackgroundJob.Enqueue(() => service.SetAsSeenFireForget(setAsSeenRequest, token, RequestType.Movies));
                  if (!res) return new HttpResponseMessage(HttpStatusCode.PreconditionFailed);
                  return new HttpResponseMessage(HttpStatusCode.Created);
              }
              catch (Exception e)
              {
                  return new HttpResponseMessage(HttpStatusCode.InternalServerError);
              }
          }

          [HttpPost]
          [Route("Watchlist")]
          public async Task<bool> Watchlist(HttpRequestMessage request)
          {
              var token = "";
              if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
              var jsonString = await request.Content.ReadAsStringAsync();
              var id = JsonConvert.DeserializeObject<int>(jsonString);
              var service = Ioc.Container.Resolve<IMovieService>();
               var res = await service.AddToWatchlist(id, token);
              return res;
          }


          [HttpGet]
          [Route("Watchlist")]
          public async Task<List<MiniMovie>> GetWatchlist()
          {
              var token = "";
              if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
              var service = Ioc.Container.Resolve<IMovieService>();
              var res = await service.GetWatchlist(token);
              return res.Data;
          }
    }
}
