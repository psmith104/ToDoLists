using System.Threading.Tasks;

namespace ToDoList.Domain.Queries
{
    public interface IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> HandleAsync(TQuery getAllToDoListsQuery);
    }
}
