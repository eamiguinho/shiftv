using System;
using System.Collections.Generic;
using System.Linq;
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
using ShiftvAPI.Contracts.Services.Sync;

namespace ShiftvAPI.Controllers
{
    [RoutePrefix("Episodes")]
    public class EpisodesController : ApiController
    {
        [HttpPost]
        [Route("Rate")]
        public async Task<List<RateResultJsonDto>> Rate(HttpRequestMessage request)
        {
            var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var jsonString = await request.Content.ReadAsStringAsync();
            var rateRequest = JsonConvert.DeserializeObject<List<RateRequestJsonDto>>(jsonString);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = service.Rate(rateRequest, token, RequestType.Episodes);
            if (res != null) BackgroundJob.Enqueue(() => service.RateFireForget(rateRequest, token, RequestType.Episodes));
            return res;
        }


        [HttpPost]
        [Route("Seen")]
        public async Task<List<SetAsSeenResultJson>> Seen(HttpRequestMessage request)
        {
            var token = "";
            if (this.Request.Headers.Authorization != null) token = this.Request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token)) throw new HttpResponseException(HttpStatusCode.Forbidden);
            var jsonString = await request.Content.ReadAsStringAsync();
            var rateRequest = JsonConvert.DeserializeObject<List<SetAsSeenJson>>(jsonString);
            var service = Ioc.Container.Resolve<ISyncService>();
            var res = service.SetAsSeen(rateRequest, token, RequestType.Episodes);
            if(res != null) BackgroundJob.Enqueue(() => service.SetAsSeenFireForget(rateRequest, token, RequestType.Episodes));
            return res;
        }

    }
}
