namespace ToDoList.Domain.Queries
{
    public interface IToDoListItemByIdQuery : IQuery
    {
        int Id { get; }
    }
}