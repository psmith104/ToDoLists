namespace ToDoList.Domain.Commands
{
    public interface IDeleteToDoListItemCommand : ICommand
    {
        int Id { get; }
    }
}
