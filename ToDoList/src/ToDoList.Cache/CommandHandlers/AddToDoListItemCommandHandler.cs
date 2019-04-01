using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;

namespace ToDoList.Cache.CommandHandlers
{
    public class AddToDoListItemCommandHandler : IAsyncCommandHandler<IAddToDoListItemCommand>
    {
        private readonly ICacheAccessor _cacheAccessor;

        public AddToDoListItemCommandHandler(ICacheAccessor cacheAccessor)
        {
            _cacheAccessor = cacheAccessor;
        }

        public Task HandleAsync(IAddToDoListItemCommand command)
        {
            var lists = (IList<ToDoListModel>)_cacheAccessor.Get(CacheKeys.ToDoLists);
            var allItems = lists.SelectMany(ls => ls.Items);
            var maxId = allItems.Max(item => item.Id);
            var list = lists.Single(ls => ls.Id == command.ListId);
            var items = (List<ToDoListItemModel>)list.Items;
            items.Add(new ToDoListItemModel(maxId + 1, command.Name));
            return Task.FromResult(0);
        }
    }
}
