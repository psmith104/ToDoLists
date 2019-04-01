namespace ToDoList.Domain.Commands
{
    public interface IUpdateToDoListItemCommand : ICommand
    {
        int Id { get; }
        string Name { get; }
    }
}
