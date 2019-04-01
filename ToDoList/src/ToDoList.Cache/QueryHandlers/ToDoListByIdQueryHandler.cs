using System.Threading.Tasks;
using ToDoList.Domain.Queries;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Services;
using ToDoList.Domain.Models;
using System.Linq;
using System.Collections.Generic;

namespace ToDoList.Cache.QueryHandlers
{
    public class ToDoListByIdQueryHandler : IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>
    {
        private readonly ICacheAccessor _cache;

        public ToDoListByIdQueryHandler(ICacheAccessor cache)
        {
            _cache = cache;
        }

        public Task<IToDoList> HandleAsync(IToDoListByIdQuery query)
        {
            var lists = (IEnumerable<IToDoList>) _cache.Get(CacheKeys.ToDoLists);
            var list = lists.SingleOrDefault(ls => ls.Id == query.Id);
            return Task.FromResult(list);
        }
    }
}
