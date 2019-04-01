using System.Threading.Tasks;
using ToDoList.Domain.Queries;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Services;
using ToDoList.Domain.Models;
using System.Linq;
using System.Collections.Generic;

namespace ToDoList.Cache.QueryHandlers
{
    public class ToDoListItemsQueryHandler : IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>
    {
        private readonly ICacheAccessor _cache;

        public ToDoListItemsQueryHandler(ICacheAccessor cache)
        {
            _cache = cache;
        }

        public Task<IEnumerable<IToDoListItem>> HandleAsync(IToDoListItemsQuery query)
        {
            var lists = (IEnumerable<IToDoList>)_cache.Get(CacheKeys.ToDoLists);
            var list = lists.SingleOrDefault(ls => ls.Id == query.ListId);
            return Task.FromResult(list.Items);
        }
    }
}
