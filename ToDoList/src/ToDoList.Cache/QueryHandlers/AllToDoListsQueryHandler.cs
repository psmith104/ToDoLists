using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Domain.Queries;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Services;
using ToDoList.Domain.Models;

namespace ToDoList.Cache.QueryHandlers
{
    public class AllToDoListsQueryHandler : IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>
    {
        private readonly ICacheAccessor _cache;

        public AllToDoListsQueryHandler(ICacheAccessor cache)
        {
            _cache = cache;
        }

        public Task<IEnumerable<IToDoList>> HandleAsync(IAllToDoListsQuery query)
        {
            var list = _cache.Get(CacheKeys.ToDoLists);
            return Task.FromResult((IEnumerable<IToDoList>) list);
        }
    }
}
