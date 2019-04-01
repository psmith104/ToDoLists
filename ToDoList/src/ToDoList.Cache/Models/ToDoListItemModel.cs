using ToDoList.Domain.Models;

namespace ToDoList.Cache.Models
{
    public class ToDoListItemModel : IToDoListItem
    {
        public int Id { get; }
        public string Name { get; set; }

        public ToDoListItemModel()
        {

        }

        public ToDoListItemModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
