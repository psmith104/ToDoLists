using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ToDoList.Api.Requests;
using ToDoList.Domain.Commands;
using ToDoList.Domain.Models;
using ToDoList.Domain.Queries;

namespace ToDoList.Api.Controllers
{
    public class ToDoListsController : ApiController
    {
        private readonly IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>> _allToDoListsQueryHandler;
        private readonly IAsyncQueryHandler<IToDoListByIdQuery, IToDoList> _toDoListByIdQueryHandler;
        private readonly IAsyncCommandHandler<IAddToDoListCommand> _addListCommandHandler;
        private readonly IAsyncCommandHandler<IUpdateToDoListCommand> _updateListCommandHandler;
        private readonly IAsyncCommandHandler<IDeleteToDoListCommand> _deleteListCommandHandler;

        public ToDoListsController(IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>> allToDoListsQueryHandler,
            IAsyncQueryHandler<IToDoListByIdQuery, IToDoList> toDoListByIdQueryHandler,
            IAsyncCommandHandler<IAddToDoListCommand> addListCommandHandler,
            IAsyncCommandHandler<IUpdateToDoListCommand> updateListCommandHandler,
            IAsyncCommandHandler<IDeleteToDoListCommand> deleteListCommandHandler)
        {
            _allToDoListsQueryHandler = allToDoListsQueryHandler;
            _toDoListByIdQueryHandler = toDoListByIdQueryHandler;
            _addListCommandHandler = addListCommandHandler;
            _updateListCommandHandler = updateListCommandHandler;
            _deleteListCommandHandler = deleteListCommandHandler;

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

        [HttpPost]
        [Route("api/ToDoLists")]
        public async Task<IHttpActionResult> PostAsync([FromBody]CreateToDoListRequest request)
        {
            await _addListCommandHandler.HandleAsync(request).ConfigureAwait(false);
            return Ok();
        }

        [HttpPut]
        [Route("api/ToDoLists/{id}")]
        public async Task<IHttpActionResult> PutAsync(int id, [FromBody]UpdateToDoListRequest request)
        {
            var list = await _toDoListByIdQueryHandler.HandleAsync(new ToDoListByIdQuery(id)).ConfigureAwait(false);
            if (list == null) return BadRequest("Could not retrieve requested To Do List");

            await _updateListCommandHandler.HandleAsync(new UpdateToDoListCommand(id, request)).ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete]
        [Route("api/ToDoLists/{id}")]
        public async Task<IHttpActionResult> DeleteAsync(int id)
        {
            var list = await _toDoListByIdQueryHandler.HandleAsync(new ToDoListByIdQuery(id)).ConfigureAwait(false);
            if (list == null) return BadRequest("Could not retrieve requested To Do List");

            await _deleteListCommandHandler.HandleAsync(new DeleteToDoListCommand(id)).ConfigureAwait(false);
            return Ok();
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

        private class UpdateToDoListCommand : IUpdateToDoListCommand
        {
            public int Id { get; }
            public string Name { get; }

            public UpdateToDoListCommand(int id, UpdateToDoListRequest request)
            {
                Id = id;
                Name = request.Name;
            }
        }

        private class DeleteToDoListCommand : IDeleteToDoListCommand
        {
            public int Id { get; }

            public DeleteToDoListCommand(int id)
            {
                Id = id;
            }
        }
    }
}
