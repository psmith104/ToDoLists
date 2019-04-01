using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Cache.CommandHandlers;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;

namespace ToDoLists.Cache.Tests.CommandHandlers
{
    public static class DeleteToDoListItemCommandHandlerTests
    {
        [TestFixture]
        public class when_deleting_a_to_do_list_item
        {
            [Test]
            public async Task then_should_delete_the_requested_to_do_list_item()
            {
                // Arrange
                var passedGetKey = (string)null;
                var list = new ToDoListModel(1, "");
                var itemToDelete = new ToDoListItemModel(1, "");
                ((List<ToDoListItemModel>)list.Items).Add(itemToDelete);
                var lists = new List<ToDoListModel> { list };
                var cacheAccessor = Mock.Of<ICacheAccessor>();
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Get(It.IsAny<string>()))
                    .Callback<string>(key => passedGetKey = key)
                    .Returns(lists);

                var queryHandler = new DeleteToDoListItemCommandHandler(cacheAccessor);

                // Act
                await queryHandler.HandleAsync(Mock.Of<IDeleteToDoListItemCommand>(command => command.Id == 1)).ConfigureAwait(false);

                // Assert
                Assert.That(passedGetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(list.Items, Has.No.Member(itemToDelete));
            }
        }
    }
}
