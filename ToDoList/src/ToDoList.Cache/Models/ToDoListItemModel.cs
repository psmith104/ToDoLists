using ToDoList.Domain.Models;

namespace ToDoList.Cache.Models
{
    public class ToDoListItemModel : IToDoListItem
    {
        public string Name { get; }

        public ToDoListItemModel(string name)
        {
            Name = name;
        }
    }
}
