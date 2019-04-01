using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ToDoList.Domain.Models;
using ToDoList.Domain.Queries;

namespace ToDoList.Api.Controllers
{
    public class ToDoListItemsController : ApiController
    {
        private readonly IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>> _listItemsQueryHandler;

        public ToDoListItemsController(IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>> listItemsQueryHandler)
        {
            _listItemsQueryHandler = listItemsQueryHandler;
        }

        [HttpGet]
        [Route("api/ToDoLists/{id}/Items")]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var items = await _listItemsQueryHandler.HandleAsync(new ToDoListItemsQuery(id)).ConfigureAwait(false);
            return Json(items);
        }

        private class ToDoListItemsQuery : IToDoListItemsQuery
        {
            public int ListId { get; }

            public ToDoListItemsQuery(int listId)
            {
                ListId = listId;
            }
        }
    }
}
