namespace ToDoList.Domain.Queries
{
    public interface IToDoListByIdQuery : IQuery
    {
        int Id { get; }
    }
}