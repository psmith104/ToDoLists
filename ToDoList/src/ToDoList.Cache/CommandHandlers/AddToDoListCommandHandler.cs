using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;
using ToDoList.Domain.Models;

namespace ToDoList.Cache.CommandHandlers
{
    public class AddToDoListCommandHandler : IAsyncCommandHandler<IAddToDoListCommand>
    {
        private readonly ICacheAccessor _cacheAccessor;

        public AddToDoListCommandHandler(ICacheAccessor cacheAccessor)
        {
            _cacheAccessor = cacheAccessor;
        }

        public Task HandleAsync(IAddToDoListCommand command)
        {
            var lists = (IList<IToDoList>)_cacheAccessor.Get(CacheKeys.ToDoLists);
            lists.Add(new ToDoListModel(3, command.Name));
            _cacheAccessor.Set(CacheKeys.ToDoLists, lists);
            return Task.FromResult(0);
        }

        private class ToDoListModel : IToDoList
        {
            public int Id { get; }
            public string Name { get; }

            public ToDoListModel(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }
    }
}
