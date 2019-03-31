using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.Queries
{
    public interface IAllToDoListsQuery : IQuery
    {
    }

    public class AllToDoListsQueryHandler : IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>
    {
        public async Task<IEnumerable<IToDoList>> HandleAsync(IAllToDoListsQuery getAllToDoListsQuery)
        {
            await Task.Delay(200).ConfigureAwait(false);
            return new List<IToDoList> { new ToDoListModel { Name = "List1" }, new ToDoListModel { Name="List2"} };
        }

        private class ToDoListModel : IToDoList
        {
            public string Name { get; set; }
        }
    }
}
