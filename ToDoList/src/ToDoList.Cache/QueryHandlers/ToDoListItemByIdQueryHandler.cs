using System.Threading.Tasks;
using ToDoList.Domain.Queries;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Services;
using ToDoList.Domain.Models;
using System.Linq;
using System.Collections.Generic;

namespace ToDoList.Cache.QueryHandlers
{
    public class ToDoListItemByIdQueryHandler : IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>
    {
        private readonly ICacheAccessor _cache;

        public ToDoListItemByIdQueryHandler(ICacheAccessor cache)
        {
            _cache = cache;
        }

        public Task<IToDoListItem> HandleAsync(IToDoListItemByIdQuery query)
        {
            var lists = (IEnumerable<IToDoList>)_cache.Get(CacheKeys.ToDoLists);
            var list = lists.SingleOrDefault(ls => ls.Items.Any(itm => itm.Id == query.Id));
            if (list == null) return Task.FromResult((IToDoListItem) null);
            var item = list.Items.Single(itm => itm.Id == query.Id);
            return Task.FromResult(item);
        }
    }
}
