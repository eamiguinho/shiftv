using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Autofac;
using Hangfire;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Services.Shows;
using ShiftvAPI.Contracts.Services.Sync;

namespace ShiftvAPI.Controllers
{
    [RoutePrefix("Shows")]
    public class ShowsController : ApiController
    {
        [Route("{id}")]
        public async Task<Show> Get(int id)
        {   var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
            var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.GetShowById(id, token);
            if (res.IsOk && res.Data != null)
            {
         
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return res.Data;
        }
        [Route("People/{showId}")]
        public async Task<People> GetPeople(int showId)
        {
            var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.GetPeople(showId);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return res.Data;
        }

        [Route("Popular")]
        public async Task<List<MiniShow>> GetPopular(int page = 1, int limit = 150)
        {
            var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
          var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.GetPopular(page, limit, token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return res.Data;
        }

        [HttpGet]
        [Route("Search/{key}")]
        public async Task<List<MiniShow>> Search(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.Search(key);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return res.Data;
        }

        [Route("Trending")]
        public async Task<List<MiniShow>> GetTrending(int page = 1, int limit = 150)
        {
            var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter; 
            var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.GetTrending(page, limit, token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return res.Data;
        }

        [Route("Progress")]
        public async Task<List<ShowProgress>> GetProgress()
        {
            var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
            var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.GetProgress(token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return res.Data;
        }

        [HttpGet]
        [Route("Update")]
        public void UpdateData()
        {
            var service = Ioc.Container.Resolve<IShowService>();
            service.UpdateData();
        }

        [Route("Seasons/{showId}")] 
        public async Task<List<Season>> GetSeasons(int showId)
        {
            var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter; 
            var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.GetSeasons(showId, token);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return res.Data;
        }

        [Route("Comments/{showId}")]
        public async Task<List<Comment>> GetComments(int showId, int page = 1, int limit = 150)
        {
            var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.GetComments(showId, page, limit);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return res.Data;
        }

        [Route("Comments/{showId}/{season}/{episode}")]
        public async Task<List<Comment>> GetComments(int showId, int season, int episode, int page = 1, int limit = 150)
        {
            var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.GetCommentsEpisode(showId, season, episode, page, limit);
            if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return res.Data;
        }

        //[HttpGet]
        //[Route("Calendar")]
        //public async Task<List<Calendar>> Calendar()
        //{
        //    var token = "";
        //    if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter; 
        //    if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
        //    var service = Ioc.Container.Resolve<IShowService>();
        //    var res = await service.GetCalendar(token);
        //    if (!res.IsOk) throw new HttpResponseException(HttpStatusCode.NotFound);
        //    if (res.Data == null) throw new HttpResponseException(HttpStatusCode.NotFound);
        //    return res.Data;
        //}   


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
                var res = service.Rate(rateRequest,token,RequestType.Shows);
                if (res) BackgroundJob.Enqueue(() => service.RateFireForget(rateRequest, token, RequestType.Shows));
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
                var res = service.Comment(commentRequest, token, RequestType.Shows);
                if (res) BackgroundJob.Enqueue(() => service.CommentFireForge(commentRequest, token, RequestType.Shows));
                if (!res) return new HttpResponseMessage(HttpStatusCode.PreconditionFailed);
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [Route("Force/{id}")]
        public async Task<Show> GetForce(int id)
        {
            var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
            var service = Ioc.Container.Resolve<IShowService>();
            var res = await service.ForceShowUpdate(id, token);
            if (res.IsOk && res.Data != null)
            {   

            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return res.Data;
        }
    
    }
}
