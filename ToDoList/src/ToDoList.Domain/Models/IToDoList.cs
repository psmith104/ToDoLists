using System.Collections.Generic;

namespace ToDoList.Domain.Models
{
    public interface IToDoList
    {
        string Name { get; }
        int Id { get; }

        IEnumerable<IToDoListItem> Items { get; }
    }
}
