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
    public static class DeleteToDoListCommandHandlerTests
    {
        [TestFixture]
        public class when_deleting_a_command_list
        {
            [Test]
            public async Task then_should_delete_the_requested_to_do_list()
            {
                // Arrange
                var passedGetKey = (string)null;
                var listToDelete = Mock.Of<ToDoListModel>(list => list.Id == 1);
                var lists = new List<ToDoListModel> { listToDelete };
                var cacheAccessor = Mock.Of<ICacheAccessor>();
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Get(It.IsAny<string>()))
                    .Callback<string>(key => passedGetKey = key)
                    .Returns(lists);

                var queryHandler = new DeleteToDoListCommandHandler(cacheAccessor);

                // Act
                await queryHandler.HandleAsync(Mock.Of<IDeleteToDoListCommand>(command => command.Id == 1)).ConfigureAwait(false);

                // Assert
                Assert.That(passedGetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(lists, Has.No.Member(listToDelete));
            }
        }
    }
}
