using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ToDoList.Domain.Models;
using ToDoList.Domain.Queries;

namespace ToDoList.Api.Controllers
{
    public class ToDoListsController : ApiController
    {
        private readonly IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>> _allToDoListsQueryHandler;
        private readonly IAsyncQueryHandler<IToDoListByIdQuery, IToDoList> _toDoListByIdQueryHandler;

        public ToDoListsController(IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>> allToDoListsQueryHandler,
            IAsyncQueryHandler<IToDoListByIdQuery, IToDoList> toDoListByIdQueryHandler)
        {
            _allToDoListsQueryHandler = allToDoListsQueryHandler;
            _toDoListByIdQueryHandler = toDoListByIdQueryHandler;
        }

        [HttpGet]
        [Route("api/ToDoLists")]
        public async Task<IHttpActionResult> GetAsync()
        {
            var lists = await _allToDoListsQueryHandler.HandleAsync(new AllToDoListQuery()).ConfigureAwait(false);
            return Json(lists);
        }

        [HttpGet]
        [Route("api/ToDoLists/{id}")]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var list = await _toDoListByIdQueryHandler.HandleAsync(new ToDoListByIdQuery(id)).ConfigureAwait(false);
            return Json(list);
        }

        // POST: api/ToDoLists
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ToDoLists/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ToDoLists/5
        public void Delete(int id)
        {
        }

        private class AllToDoListQuery : IAllToDoListsQuery { }

        private class ToDoListByIdQuery : IToDoListByIdQuery
        {
            public int Id { get; }

            public ToDoListByIdQuery(int id)
            {
                Id = id;
            }
        }
    }
}
