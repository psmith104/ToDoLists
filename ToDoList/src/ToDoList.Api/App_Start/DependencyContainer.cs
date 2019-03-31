using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Web.Http;
using ToDoList.Domain.Queries;

namespace ToDoList.Api
{
    public static partial class WebApiConfig
    {
        public class DependencyContainer
        {
            private readonly Container _container = new Container();

            public void RegisterDependencies()
            {
                _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

                // Query Handlers
                _container.Register(typeof(IAsyncQueryHandler<,>), typeof(AllToDoListsQueryHandler).Assembly);

                _container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

                _container.Verify();

                GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(_container);
            }
        }
    }
}
