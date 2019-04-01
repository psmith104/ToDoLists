namespace ToDoList.Domain.Commands
{
    public interface IDeleteToDoListCommand : ICommand
    {
        int Id { get; }
    }
}
