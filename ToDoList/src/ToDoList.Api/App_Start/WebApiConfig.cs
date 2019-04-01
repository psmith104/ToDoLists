using System.Web.Http;

namespace ToDoList.Api
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            // Setup DI
            var container = new DependencyContainer();
            container.SetupConatiner();

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
