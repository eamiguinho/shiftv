using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ShiftWebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{param1}/{param2}/{param3}",
                defaults: new { id = RouteParameter.Optional, param1 = RouteParameter.Optional, param2 = RouteParameter.Optional, param3 =RouteParameter.Optional }
            );
        }
    }
}
