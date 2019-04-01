﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;
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
            var lists = (IList<ToDoListModel>)_cacheAccessor.Get(CacheKeys.ToDoLists);
            var maxId = lists.Max(list => list.Id);
            lists.Add(new ToDoListModel(maxId + 1, command.Name));
            _cacheAccessor.Set(CacheKeys.ToDoLists, lists);
            return Task.FromResult(0);
        }
    }
}
