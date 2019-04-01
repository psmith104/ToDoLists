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
    public static class ToDoListItemsControllerTests
    {
        [TestFixture]
        public class when_requesting_a_list_of_todo_list_items
        {
            [Test]
            public async Task then_should_retreive_the_list_of_todo_list_items_for_the_requested_to_do_list()
            {
                // Arrange
                var passedId = (int?)null;
                var items = new List<IToDoListItem> { Mock.Of<IToDoListItem>(), Mock.Of<IToDoListItem>() };
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListItemsQuery>()))
                    .Callback<IToDoListItemsQuery>(query => passedId = query.ListId)
                    .ReturnsAsync(items);

                var controller = new ToDoListItemsController(queryHandler, Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>());

                // Act
                var result = ((JsonResult<IEnumerable<IToDoListItem>>)(await controller.GetAsync(1).ConfigureAwait(false))).Content;

                // Assert
                Assert.That(passedId, Is.EqualTo(1));
                Assert.That(result, Is.EqualTo(items));
            }
        }

        [TestFixture]
        public class when_requesting_a_specific_todo_list_items
        {
            [Test]
            public async Task then_should_retreive_the_todo_list_item_for_the_requested_id()
            {
                // Arrange
                var passedId = (int?)null;
                var items = Mock.Of<IToDoListItem>();
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListItemByIdQuery>()))
                    .Callback<IToDoListItemByIdQuery>(query => passedId = query.Id)
                    .ReturnsAsync(items);

                var controller = new ToDoListItemsController(Mock.Of<IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>>(),
                    queryHandler);

                // Act
                var result = ((JsonResult<IToDoListItem>)(await controller.GetAsync(1, 2).ConfigureAwait(false))).Content;

                // Assert
                Assert.That(passedId, Is.EqualTo(2));
                Assert.That(result, Is.EqualTo(items));
            }
        }
    }
}
