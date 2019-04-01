using System.Collections.Generic;
using System.Runtime.Caching;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;

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
                var lists = new List<ToDoListModel> { new ToDoListModel { Id=1, Name = "List1" }, new ToDoListModel { Id=2, Name = "List2" } };
                _memoryCache.AddOrGetExisting(CacheKeys.ToDoLists, lists, new CacheItemPolicy());
            }
        }

        public object Get(string key) => _memoryCache.Get(key);

        public void Set(string key, object obj) => _memoryCache.Set(key, obj, new CacheItemPolicy());
    }
}
