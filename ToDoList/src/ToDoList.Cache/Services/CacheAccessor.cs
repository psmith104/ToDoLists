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
                var list1 = new ToDoListModel { Id = 1, Name = "Nursery" };
                var items1 = (List<ToDoListItemModel>) list1.Items;
                items1.Add(new ToDoListItemModel("Paint"));
                items1.Add(new ToDoListItemModel("Build Furniture"));
                var list2 = new ToDoListModel { Id = 2, Name = "Yard" };
                var items2 = (List<ToDoListItemModel>)list2.Items;
                items2.Add(new ToDoListItemModel("Remove Gate"));
                var lists = new List<ToDoListModel> { list1, list2 };
                _memoryCache.AddOrGetExisting(CacheKeys.ToDoLists, lists, new CacheItemPolicy());
            }
        }

        public object Get(string key) => _memoryCache.Get(key);

        public void Set(string key, object obj) => _memoryCache.Set(key, obj, new CacheItemPolicy());
    }
}
