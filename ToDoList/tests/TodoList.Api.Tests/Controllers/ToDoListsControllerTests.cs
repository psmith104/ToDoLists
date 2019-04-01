using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using ToDoList.Api.Controllers;
using ToDoList.Domain.Models;
using ToDoList.Domain.Queries;

namespace TodoList.Api.Tests.Controllers
{
    public static class ToDoListsControllerTests
    {
        [TestFixture]
        public class when_requesting_a_list_of_todo_lists
        {
            [Test]
            public async Task then_should_retreive_the_list_of_todo_lists()
            {
                // Arrange
                var lists = new List<IToDoList> { Mock.Of<IToDoList>(), Mock.Of<IToDoList>() };
                var queryHandler = Mock.Of<IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IAllToDoListsQuery>()))
                    .ReturnsAsync(lists);

                var controller = new ToDoListsController(queryHandler, Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>());

                // Act
                var result = ((JsonResult<IEnumerable<IToDoList>>) (await controller.GetAsync().ConfigureAwait(false))).Content;

                // Assert
                Assert.That(result, Is.EqualTo(lists));
            }
        }

        [TestFixture]
        public class when_requesting_a_single_todo_list
        {
            [Test]
            public async Task then_should_retreive_the_requested_todo_list()
            {
                // Arrange
                var passedId = (int?) null;
                var list = Mock.Of<IToDoList>();
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListByIdQuery>()))
                    .Callback<IToDoListByIdQuery>(query => passedId = query.Id)
                    .ReturnsAsync(list);

                var controller = new ToDoListsController(Mock.Of<IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>>(), queryHandler);

                // Act
                var result = ((JsonResult<IToDoList>)(await controller.GetAsync(2).ConfigureAwait(false))).Content;

                // Assert
                Assert.That(passedId, Is.EqualTo(2));
            }
        }
    }
}
