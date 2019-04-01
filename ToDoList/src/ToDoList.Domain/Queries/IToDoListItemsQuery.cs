namespace ToDoList.Domain.Queries
{
    public interface IToDoListItemsQuery : IQuery
    {
        int ListId { get; }
    }
}