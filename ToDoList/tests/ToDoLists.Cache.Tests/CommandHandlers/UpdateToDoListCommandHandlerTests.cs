using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Cache.CommandHandlers;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;
using ToDoList.Cache.QueryHandlers;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;
using ToDoList.Domain.Models;
using ToDoList.Domain.Queries;

namespace ToDoLists.Cache.Tests.CommandHandlers
{
    public static class UpdateToDoListCommandHandlerTests
    {
        [TestFixture]
        public class when_updating_a_command_list
        {
            [Test]
            public async Task then_should_update_the_requested_to_do_list_with_the_provided_name()
            {
                // Arrange
                var passedGetKey = (string) null;
                var passedSetKey = (string) null;
                var passedObject = (object) null;
                var listToUpdate = Mock.Of<ToDoListModel>(list => list.Id == 1);
                var lists = new List<ToDoListModel> { listToUpdate };
                var cacheAccessor = Mock.Of<ICacheAccessor>();
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Get(It.IsAny<string>()))
                    .Callback<string>(key => passedGetKey = key)
                    .Returns(lists);
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Set(It.IsAny<string>(), It.IsAny<object>()))
                    .Callback<string, object>((key, obj) => { passedSetKey = key; passedObject = obj; });

                var queryHandler = new UpdateToDoListCommandHandler(cacheAccessor);

                // Act
                await queryHandler.HandleAsync(Mock.Of<IUpdateToDoListCommand>(command => command.Id == 1 && command.Name == "test")).ConfigureAwait(false);

                // Assert
                Assert.That(passedGetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(passedSetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(passedObject, Is.EqualTo(lists));
                Assert.That(listToUpdate.Name, Is.EqualTo("test"));
            }
        }
    }
}
