namespace ToDoList.Domain.Commands
{
    public interface IUpdateToDoListCommand : ICommand
    {
        int Id { get; }
        string Name { get; }
    }
}
