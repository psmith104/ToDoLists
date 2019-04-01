using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;
using ToDoList.Domain.Models;

namespace ToDoList.Cache.CommandHandlers
{
    public class UpdateToDoListCommandHandler : IAsyncCommandHandler<IUpdateToDoListCommand>
    {
        private readonly ICacheAccessor _cacheAccessor;

        public UpdateToDoListCommandHandler(ICacheAccessor cacheAccessor)
        {
            _cacheAccessor = cacheAccessor;
        }

        public Task HandleAsync(IUpdateToDoListCommand command)
        {
            var lists = (IList<ToDoListModel>)_cacheAccessor.Get(CacheKeys.ToDoLists);
            var listToUpdate = lists.Single(list => list.Id.Equals(command.Id));
            listToUpdate.Name = command.Name;
            return Task.FromResult(0);
        }
    }
}
