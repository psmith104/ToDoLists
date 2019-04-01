using System.Collections.Generic;
using ToDoList.Domain.Models;

namespace ToDoList.Cache.Models
{
    public class ToDoListModel : IToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<IToDoListItem> Items { get; }

        public ToDoListModel()
        {
            Items = new List<ToDoListItemModel>();
        }

        public ToDoListModel(int id, string name) : this()
        {
            Id = id;
            Name = name;
        }
    }
}
