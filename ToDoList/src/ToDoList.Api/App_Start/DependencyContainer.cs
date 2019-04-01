using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Web.Http;
using ToDoList.Cache.CommandHandlers;
using ToDoList.Cache.QueryHandlers;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;
using ToDoList.Domain.Queries;

namespace ToDoList.Api
{
    public class DependencyContainer
    {
        private readonly Container _container = new Container();

        public void RegisterDependencies()
        {
            //Services
            _container.Register<ICacheAccessor, CacheAccessor>(Lifestyle.Singleton);

            // Command Handlers
            _container.Register(typeof(IAsyncCommandHandler<>), typeof(AddToDoListCommandHandler).Assembly);

            // Query Handlers
            _container.Register(typeof(IAsyncQueryHandler<,>), typeof(AllToDoListsQueryHandler).Assembly);

            _container.Verify();

        }

        public void SetupConatiner()
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            _container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            RegisterDependencies();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(_container);
        }
    }
}
