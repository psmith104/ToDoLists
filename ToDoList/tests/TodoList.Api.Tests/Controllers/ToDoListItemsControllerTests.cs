﻿using Moq;
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
                    Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>());

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
                    queryHandler, Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>());

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
                var passedListId = (int?)null;
                var passedName = (string)null;
                var commandHandler = Mock.Of<IAsyncCommandHandler<IAddToDoListItemCommand>>();
                Mock.Get(commandHandler)
                    .Setup(handler => handler.HandleAsync(It.IsAny<IAddToDoListItemCommand>()))
                    .Callback<IAddToDoListItemCommand>(command => { passedName = command.Name; passedListId = command.ListId; })
                    .Returns(Task.FromResult(0));

                var controller = new ToDoListItemsController(Mock.Of<IAsyncQueryHandler<IToDoListItemsQuery, IEnumerable<IToDoListItem>>>(),
                    Mock.Of<IAsyncQueryHandler<IToDoListItemByIdQuery, IToDoListItem>>(), commandHandler);

                // Act
                var result = await controller.PostAsync(1, new CreateToDoListItemRequest { Name = "newitem" }).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.InstanceOf(typeof(OkResult)));
                Assert.That(passedListId, Is.EqualTo(1));
                Assert.That(passedName, Is.EqualTo("newitem"));
            }
        }
    }
}
