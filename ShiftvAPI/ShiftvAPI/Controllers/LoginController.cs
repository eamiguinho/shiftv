using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Services.Login;
using ShiftvAPI.Contracts.Services.Movies;

namespace ShiftvAPI.Controllers
{
    [RoutePrefix("Login")]
    public class LoginController : ApiController
    {
        [Route("GetToken/{code}")]
        public async Task<TokenResult> GetToken(string code)
        {
            var service = Ioc.Container.Resolve<ILoginService>();
            var res = await service.GetToken(code);
            if (!res.IsOk || res.Data == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return res.Data;
        }
    }
}
