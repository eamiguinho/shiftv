using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Services.Lists;

namespace ShiftvAPI.Controllers
{
    [RoutePrefix("Lists")]
    public class ListsController : ApiController
    {
        [Route("{username}/{id}/{type}")]
        public async Task<object> Get(string username, string id,string type)
        {
            var service = Ioc.Container.Resolve<IListService>();
            if (type == "show")
            {
                var res = await service.GetListShow(username, id);
                if (!res.IsOk || res.Data == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return res.Data;
            }
            if (type == "movie")
            {
                var res = await service.GetListMovie(username, id);
                if (!res.IsOk || res.Data == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return res.Data;
            }
            return null;
        }
    }
}
