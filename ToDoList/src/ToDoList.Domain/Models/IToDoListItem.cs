namespace ToDoList.Domain.Models
{
    public interface IToDoListItem
    {
        int Id { get; }
        string Name { get; }
    }
}
