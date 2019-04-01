using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;

namespace ToDoList.Cache.CommandHandlers
{
    public class CompleteToDoListItemCommandHandler : IAsyncCommandHandler<ICompleteToDoListItemCommand>
    {
        private readonly ICacheAccessor _cacheAccessor;

        public CompleteToDoListItemCommandHandler(ICacheAccessor cacheAccessor)
        {
            _cacheAccessor = cacheAccessor;
        }

        public Task HandleAsync(ICompleteToDoListItemCommand command)
        {
            var lists = (IList<ToDoListModel>)_cacheAccessor.Get(CacheKeys.ToDoLists);
            var list = lists.Single(lst => lst.Items.Any(itm => itm.Id == command.Id));
            var item = ((ToDoListItemModel)list.Items.Single(itm => itm.Id == command.Id));
            item.IsCompleted = true;
            return Task.FromResult(0);
        }
    }
}
