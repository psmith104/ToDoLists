using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;

namespace ToDoList.Cache.CommandHandlers
{
    public class UpdateToDoListItemCommandHandler : IAsyncCommandHandler<IUpdateToDoListItemCommand>
    {
        private readonly ICacheAccessor _cacheAccessor;

        public UpdateToDoListItemCommandHandler(ICacheAccessor cacheAccessor)
        {
            _cacheAccessor = cacheAccessor;
        }

        public Task HandleAsync(IUpdateToDoListItemCommand command)
        {
            var lists = (IList<ToDoListModel>)_cacheAccessor.Get(CacheKeys.ToDoLists);
            var allItems = lists.SelectMany(ls => ls.Items);
            var itemToUpdate = (ToDoListItemModel) allItems.Single(itm => itm.Id.Equals(command.Id));
            itemToUpdate.Name = command.Name;
            return Task.FromResult(0);
        }
    }
}
