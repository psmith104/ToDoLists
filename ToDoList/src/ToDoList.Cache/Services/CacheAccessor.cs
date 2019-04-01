using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using ToDoList.Domain.Models;
using ToDoList.Cache.Helpers;

namespace ToDoList.Cache.Services
{
    public class CacheAccessor : ICacheAccessor
    {
        private readonly MemoryCache _memoryCache = MemoryCache.Default;

        public CacheAccessor()
        {
            // Add default values
            if (!_memoryCache.Contains(CacheKeys.ToDoLists))
            {
                var lists = new List<IToDoList> { new ToDoListModel { Id=1, Name = "List1" }, new ToDoListModel { Id=2, Name = "List2" } };
                _memoryCache.AddOrGetExisting(CacheKeys.ToDoLists, lists, new CacheItemPolicy());
            }
        }

        public object Get(string key) => _memoryCache.Get(key);

        private class ToDoListModel : IToDoList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
