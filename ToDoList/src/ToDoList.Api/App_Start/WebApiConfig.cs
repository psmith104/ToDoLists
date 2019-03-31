using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ToDoList.Api
{
    public static partial class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            // Setup DI
            var container = new DependencyContainer();
            container.RegisterDependencies();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
