using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ToDoList.Domain.Models;
using ToDoList.Domain.Queries;

namespace ToDoList.Api.Controllers
{
    public class ToDoListsController : ApiController
    {
        private readonly IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>> _allTodoListsQueryHandler;

        public ToDoListsController(IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>> allTodoListsQueryHandler)
        {
            _allTodoListsQueryHandler = allTodoListsQueryHandler;
        }

        [HttpGet]
        [Route("api/ToDoLists")]
        public async Task<IHttpActionResult> GetAsync()
        {
            var lists = await _allTodoListsQueryHandler.HandleAsync(new AllToDoListQuery()).ConfigureAwait(false);
            return Json(lists);
        }

        // GET: api/ToDoLists/5
        public string Get(int id)
        {
            return "value";
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

        private class AllToDoListQuery : IAllToDoListsQuery
        {

        }
    }
}
