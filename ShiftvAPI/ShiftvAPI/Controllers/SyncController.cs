using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.Sync;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Services.Sync;

namespace ShiftvAPI.Controllers
{
    [RoutePrefix("Sync")]
    public class SyncController : ApiController
    {
        [HttpGet]
        [Route("ratings/shows")]
        public async Task<bool> GetRatings()
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetRatings(token, RequestType.Shows);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }

        [HttpGet]
        [Route("ratings/seasons")]
        public async Task<bool> GetSeasonRatings()
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetRatings(token, RequestType.Seasons);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }


        [HttpGet]
        [Route("ratings/episodes")]
        public async Task<bool> GetEpisodeRatings()
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetRatings(token, RequestType.Episodes);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }


        [HttpGet]
        [Route("ratings/movies")]
        public async Task<bool> GetMoviesRatings()
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetRatings(token, RequestType.Movies);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }


        [HttpGet]
        [Route("stats")]
        public async Task<bool> GetStats()
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetStats(token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }


        [HttpGet]
        [Route("watched/shows")]
        public async Task<bool> GetWatchedShows()
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetWatchedShows(token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }

        [HttpGet]
        [Route("watched/movies")]
        public async Task<bool> GetWatchedMovies()
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetWatchedMovies(token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }

        [HttpGet]
        [Route("upload/ratings")]
        public async Task<bool> GetUploadRatings()  
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetUploadRatings(token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }


        [HttpGet]
        [Route("upload/watched/episodes")]
        public async Task<bool> GetUploadWatchedEpisodes()
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetUploadWatchedEpisodes(token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }

        [HttpGet]
        [Route("upload/comments")]
        public async Task<bool> GetUploadComments()
        {
            if (Request.Headers.Authorization == null) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var token = Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = await service.GetUploadComments(token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            return true;
        }
    }
}
