using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using ToDoList.Api.Controllers;
using ToDoList.Api.Requests;
using ToDoList.Domain.Commands;
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

                var controller = new ToDoListItemsController(queryHandler, Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>(),
                    Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>(), Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>(),
                    Mock.Of<IAsyncCommandHandler<IUpdateToDoListItemCommand>>(), Mock.Of<IAsyncCommandHandler<IDeleteToDoListItemCommand>>());

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
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>(), Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>(),
                    Mock.Of<IAsyncCommandHandler<IUpdateToDoListItemCommand>>(), Mock.Of<IAsyncCommandHandler<IDeleteToDoListItemCommand>>());

                // Act
                var result = ((JsonResult<IToDoListItem>)(await controller.GetAsync(1, 2).ConfigureAwait(false))).Content;

                // Assert
                Assert.That(passedId, Is.EqualTo(2));
                Assert.That(result, Is.EqualTo(items));
            }
        }

        [TestFixture]
        public class when_requesting_to_add_a_to_do_list_item
        {
            [Test]
            public async Task then_should_add_the_requested_todo_list_item_to_the_requested_list()
            {
                // Arrange
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListByIdQuery>()))
                    .ReturnsAsync(Mock.Of<IToDoList>());

                var passedListId = (int?)null;
                var passedName = (string)null;
                var commandHandler = Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>();
                Mock.Get(commandHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IAddToDoListItemCommand>()))
                    .Callback<IAddToDoListItemCommand>(command => { passedName = command.Name; passedListId = command.ListId; })
                    .Returns(Task.FromResult(0));

                var controller = new ToDoListItemsController(Mock.Of<IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>>(),
                    Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>(), commandHandler, queryHandler,
                    Mock.Of<IAsyncCommandHandler<IUpdateToDoListItemCommand>>(), Mock.Of<IAsyncCommandHandler<IDeleteToDoListItemCommand>>());

                // Act
                var result = await controller.PostAsync(1, new CreateToDoListItemRequest { Name = "newitem" }).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.InstanceOf(typeof(OkResult)));
                Assert.That(passedListId, Is.EqualTo(1));
                Assert.That(passedName, Is.EqualTo("newitem"));
            }

            [Test]
            public async Task then_should_return_a_bad_request_response_given_the_to_do_list_could_not_be_found()
            {
                // Arrange
                var passedId = (int?)null;
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListByIdQuery>()))
                    .Callback<IToDoListByIdQuery>(query => passedId = query.Id)
                    .ReturnsAsync((IToDoList)null);

                var commandHandler = Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>();

                var controller = new ToDoListItemsController(Mock.Of<IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>>(),
                    Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>(), commandHandler, queryHandler,
                    Mock.Of<IAsyncCommandHandler<IUpdateToDoListItemCommand>>(), Mock.Of<IAsyncCommandHandler<IDeleteToDoListItemCommand>>());

                // Act
                var result = ((BadRequestErrorMessageResult)await controller.PostAsync(1, new CreateToDoListItemRequest()).ConfigureAwait(false)).Message;

                // Assert
                Mock.Get(commandHandler).Verify(handler => handler.HandleAsync(It.IsAny<IAddToDoListItemCommand>()), Times.Never);
                Assert.That(result, Is.EqualTo("Could not retrieve requested To Do List"));
                Assert.That(passedId, Is.EqualTo(1));
            }
        }

        [TestFixture]
        public class when_requesting_to_update_a_to_do_list_item
        {
            [Test]
            public async Task then_should_update_the_requested_todo_list_item()
            {
                // Arrange
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListItemByIdQuery>()))
                    .ReturnsAsync(Mock.Of<IToDoListItem>());

                var passedId = (int?)null;
                var passedName = (string)null;
                var commandHandler = Mock.Of<IAsyncCommandHandler<IUpdateToDoListItemCommand>>();
                Mock.Get(commandHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IUpdateToDoListItemCommand>()))
                    .Callback<IUpdateToDoListItemCommand>(command => { passedName = command.Name; passedId = command.Id; })
                    .Returns(Task.FromResult(0));

                var controller = new ToDoListItemsController(Mock.Of<IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>>(),
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>(),
                    Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>(), commandHandler,
                    Mock.Of<IAsyncCommandHandler<IDeleteToDoListItemCommand>>());

                // Act
                var result = await controller.PutAsync(1, 2, new UpdateToDoListItemRequest { Name = "updateItem" }).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.InstanceOf(typeof(OkResult)));
                Assert.That(passedId, Is.EqualTo(2));
                Assert.That(passedName, Is.EqualTo("updateItem"));
            }

            [Test]
            public async Task then_should_return_a_bad_request_response_given_the_to_do_list_could_not_be_found()
            {
                // Arrange
                var passedId = (int?)null;
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListItemByIdQuery>()))
                    .Callback<IToDoListItemByIdQuery>(query => passedId = query.Id)
                    .ReturnsAsync((IToDoListItem)null);

                var commandHandler = Mock.Of<IAsyncCommandHandler<IUpdateToDoListItemCommand>>();

                var controller = new ToDoListItemsController(Mock.Of<IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>>(),
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>(),
                    Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>(), commandHandler,
                    Mock.Of<IAsyncCommandHandler<IDeleteToDoListItemCommand>>());

                // Act
                var result = ((BadRequestErrorMessageResult)await controller.PutAsync(1, 2, new UpdateToDoListItemRequest()).ConfigureAwait(false)).Message;

                // Assert
                Mock.Get(commandHandler).Verify(handler => handler.HandleAsync(It.IsAny<IUpdateToDoListItemCommand>()), Times.Never);
                Assert.That(result, Is.EqualTo("Could not retrieve requested To Do List"));
                Assert.That(passedId, Is.EqualTo(2));
            }
        }

        [TestFixture]
        public class when_requesting_to_delete_a_to_do_list_item
        {
            [Test]
            public async Task then_should_delete_the_requested_todo_list_item()
            {
                // Arrange
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListItemByIdQuery>()))
                    .ReturnsAsync(Mock.Of<IToDoListItem>());

                var passedId = (int?)null;
                var commandHandler = Mock.Of<IAsyncCommandHandler<IDeleteToDoListItemCommand>>();
                Mock.Get(commandHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IDeleteToDoListItemCommand>()))
                    .Callback<IDeleteToDoListItemCommand>(command => passedId = command.Id)
                    .Returns(Task.FromResult(0));

                var controller = new ToDoListItemsController(Mock.Of<IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>>(),
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>(),
                    Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>(), Mock.Of<IAsyncCommandHandler<IUpdateToDoListItemCommand>>(),
                    commandHandler);

                // Act
                var result = await controller.DeleteAsync(1, 2).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.InstanceOf(typeof(OkResult)));
                Assert.That(passedId, Is.EqualTo(2));
            }

            [Test]
            public async Task then_should_return_a_bad_request_response_given_the_to_do_list_could_not_be_found()
            {
                // Arrange
                var passedId = (int?)null;
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListItemByIdQuery>()))
                    .Callback<IToDoListItemByIdQuery>(query => passedId = query.Id)
                    .ReturnsAsync((IToDoListItem)null);

                var commandHandler = Mock.Of<IAsyncCommandHandler<IDeleteToDoListItemCommand>>();

                var controller = new ToDoListItemsController(Mock.Of<IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>>(),
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>(),
                    Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>(), Mock.Of<IAsyncCommandHandler<IUpdateToDoListItemCommand>>(),
                    commandHandler);

                // Act
                var result = ((BadRequestErrorMessageResult)await controller.PutAsync(1, 2, new UpdateToDoListItemRequest()).ConfigureAwait(false)).Message;

                // Assert
                Mock.Get(commandHandler).Verify(handler => handler.HandleAsync(It.IsAny<IDeleteToDoListItemCommand>()), Times.Never);
                Assert.That(result, Is.EqualTo("Could not retrieve requested To Do List"));
                Assert.That(passedId, Is.EqualTo(2));
            }
        }
    }
}
