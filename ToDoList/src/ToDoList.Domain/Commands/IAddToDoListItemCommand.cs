namespace ToDoList.Domain.Commands
{
    public interface IAddToDoListItemCommand : ICommand
    {
        int ListId { get; }
        string Name { get; }
    }
}
