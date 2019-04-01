namespace ToDoList.Domain.Commands
{
    public interface ICompleteToDoListItemCommand : ICommand
    {
        int Id { get; }
    }
}
