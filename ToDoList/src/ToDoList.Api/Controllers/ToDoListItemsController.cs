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
        private readonly IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem> _listItemByIdQueryHandler;

        public ToDoListItemsController(IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>> listItemsQueryHandler,
            IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem> listItemByIdQueryHandler)
        {
            _listItemsQueryHandler = listItemsQueryHandler;
            _listItemByIdQueryHandler = listItemByIdQueryHandler;
        }

        [HttpGet]
        [Route("api/ToDoLists/{listId}/Items")]
        public async Task<IHttpActionResult> GetAsync(int listId)
        {
            var items = await _listItemsQueryHandler.HandleAsync(new ToDoListItemsQuery(listId)).ConfigureAwait(false);
            return Json(items);
        }

        [HttpGet]
        [Route("api/ToDoLists/{listId}/Items/{id}")]
        public async Task<IHttpActionResult> GetAsync(int listId, int id)
        {
            var item = await _listItemByIdQueryHandler.HandleAsync(new ToDoListItemByIdQuery(id)).ConfigureAwait(false);
            return Json(item);
        }

        private class ToDoListItemsQuery : IToDoListItemsQuery
        {
            public int ListId { get; }

            public ToDoListItemsQuery(int listId)
            {
                ListId = listId;
            }
        }

        private class ToDoListItemByIdQuery : IToDoListItemByIdQuery
        {
            public int Id { get; }

            public ToDoListItemByIdQuery(int id)
            {
                Id = id;
            }
        }
    }
}
