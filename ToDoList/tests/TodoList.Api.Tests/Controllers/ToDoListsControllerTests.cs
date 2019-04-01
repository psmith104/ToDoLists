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

                var controller = new ToDoListsController(queryHandler, Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>(),
                    Mock.Of<IAsyncCommandHandler<IAddToDoListCommand>>(), Mock.Of<IAsyncCommandHandler<IUpdateToDoListCommand>>(),
                    Mock.Of<IAsyncCommandHandler<IDeleteToDoListCommand>>());

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

                var controller = new ToDoListsController(Mock.Of<IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>>(), queryHandler,
                    Mock.Of<IAsyncCommandHandler<IAddToDoListCommand>>(), Mock.Of<IAsyncCommandHandler<IUpdateToDoListCommand>>(),
                    Mock.Of<IAsyncCommandHandler<IDeleteToDoListCommand>>());

                // Act
                var result = ((JsonResult<IToDoList>)(await controller.GetAsync(2).ConfigureAwait(false))).Content;

                // Assert
                Assert.That(passedId, Is.EqualTo(2));
            }
        }

        [TestFixture]
        public class when_requesting_to_add_a_to_do_list
        {
            [Test]
            public async Task then_should_add_the_requested_todo_list()
            {
                // Arrange
                var passedName = (string) null;
                var list = Mock.Of<IToDoList>();
                var commandHandler = Mock.Of<IAsyncCommandHandler<IAddToDoListCommand>>();
                Mock.Get(commandHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IAddToDoListCommand>()))
                    .Callback<IAddToDoListCommand>(command => passedName = command.Name)
                    .Returns(Task.FromResult(0));

                var controller = new ToDoListsController(Mock.Of<IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>>(),
                    Mock.Of< IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>(), commandHandler, Mock.Of<IAsyncCommandHandler<IUpdateToDoListCommand>>(),
                    Mock.Of<IAsyncCommandHandler<IDeleteToDoListCommand>>());

                // Act
                var result = await controller.PostAsync(new CreateToDoListRequest { Name="mylist" }).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.InstanceOf(typeof(OkResult)));
                Assert.That(passedName, Is.EqualTo("mylist"));
            }
        }

        [TestFixture]
        public class when_requesting_to_update_a_to_do_list
        {
            [Test]
            public async Task then_should_update_the_requested_todo_list()
            {
                // Arrange
                var list = Mock.Of<IToDoList>();
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListByIdQuery>()))
                    .ReturnsAsync(list);

                var passedId = (int?) null;
                var passedName = (string) null;
                var commandHandler = Mock.Of<IAsyncCommandHandler<IUpdateToDoListCommand>>();
                Mock.Get(commandHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IUpdateToDoListCommand>()))
                    .Callback<IUpdateToDoListCommand>(command => { passedName = command.Name; passedId = command.Id; })
                    .Returns(Task.FromResult(0));

                var controller = new ToDoListsController(Mock.Of<IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>>(),
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListCommand>>(), commandHandler,
                    Mock.Of<IAsyncCommandHandler<IDeleteToDoListCommand>>());

                // Act
                var result = await controller.PutAsync(1, new UpdateToDoListRequest { Name = "mylist" }).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.InstanceOf(typeof(OkResult)));
                Assert.That(passedId, Is.EqualTo(1));
                Assert.That(passedName, Is.EqualTo("mylist"));
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
                    .ReturnsAsync((IToDoList) null);

                var commandHandler = Mock.Of<IAsyncCommandHandler<IUpdateToDoListCommand>>();

                var controller = new ToDoListsController(Mock.Of<IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>>(),
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListCommand>>(), commandHandler,
                    Mock.Of<IAsyncCommandHandler<IDeleteToDoListCommand>>());

                // Act
                var result = ((BadRequestErrorMessageResult) await controller.PutAsync(1, new UpdateToDoListRequest()).ConfigureAwait(false)).Message;

                // Assert
                Mock.Get(commandHandler).Verify(handler => handler.HandleAsync(It.IsAny<IUpdateToDoListCommand>()), Times.Never);
                Assert.That(result, Is.EqualTo("Could not retrieve requested To Do List"));
                Assert.That(passedId, Is.EqualTo(1));
            }
        }

        [TestFixture]
        public class when_requesting_to_delete_a_to_do_list
        {
            [Test]
            public async Task then_should_delete_the_requested_todo_list()
            {
                // Arrange
                var list = Mock.Of<IToDoList>();
                var queryHandler = Mock.Of<IAsyncQueryHandler<IToDoListByIdQuery, IToDoList>>();
                Mock.Get(queryHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IToDoListByIdQuery>()))
                    .ReturnsAsync(list);

                var passedId = (int?)null;
                var passedName = (string)null;
                var commandHandler = Mock.Of<IAsyncCommandHandler<IDeleteToDoListCommand>>();
                Mock.Get(commandHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IDeleteToDoListCommand>()))
                    .Callback<IDeleteToDoListCommand>(command => { passedId = command.Id; })
                    .Returns(Task.FromResult(0));

                var controller = new ToDoListsController(Mock.Of<IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>>(),
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListCommand>>(), Mock.Of<IAsyncCommandHandler<IUpdateToDoListCommand>>(),
                    commandHandler);

                // Act
                var result = await controller.DeleteAsync(1).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.InstanceOf(typeof(OkResult)));
                Assert.That(passedId, Is.EqualTo(1));
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

                var commandHandler = Mock.Of<IAsyncCommandHandler<IDeleteToDoListCommand>>();

                var controller = new ToDoListsController(Mock.Of<IAsyncQueryHandler<IAllToDoListsQuery, IEnumerable<IToDoList>>>(),
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListCommand>>(), Mock.Of<IAsyncCommandHandler<IUpdateToDoListCommand>>(),
                    commandHandler);

                // Act
                var result = ((BadRequestErrorMessageResult)await controller.DeleteAsync(1).ConfigureAwait(false)).Message;

                // Assert
                Mock.Get(commandHandler).Verify(handler => handler.HandleAsync(It.IsAny<IDeleteToDoListCommand>()), Times.Never);
                Assert.That(result, Is.EqualTo("Could not retrieve requested To Do List"));
                Assert.That(passedId, Is.EqualTo(1));
            }
        }
    }
}
