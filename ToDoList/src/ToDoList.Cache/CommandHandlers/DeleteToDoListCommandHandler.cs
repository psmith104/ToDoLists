using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;

namespace ToDoList.Cache.CommandHandlers
{
    public class DeleteToDoListCommandHandler : IAsyncCommandHandler<IDeleteToDoListCommand>
    {
        private readonly ICacheAccessor _cacheAccessor;

        public DeleteToDoListCommandHandler(ICacheAccessor cacheAccessor)
        {
            _cacheAccessor = cacheAccessor;
        }

        public Task HandleAsync(IDeleteToDoListCommand command)
        {
            var lists = (IList<ToDoListModel>)_cacheAccessor.Get(CacheKeys.ToDoLists);
            var listToDelete = lists.Single(list => list.Id.Equals(command.Id));
            lists.Remove(listToDelete);
            return Task.FromResult(0);
        }
    }
}
