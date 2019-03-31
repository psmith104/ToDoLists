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

                var controller = new ToDoListsController(queryHandler);

                // Act
                var result = ((JsonResult<IEnumerable<IToDoList>>) (await controller.GetAsync().ConfigureAwait(false))).Content;

                // Assert
                Assert.That(result, Is.EqualTo(lists));
            }
        }
    }
}
