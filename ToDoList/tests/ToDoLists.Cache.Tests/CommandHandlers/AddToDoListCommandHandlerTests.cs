using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Cache.CommandHandlers;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.Models;
using ToDoList.Cache.Services;
using ToDoList.Domain.Commands;

namespace ToDoLists.Cache.Tests.CommandHandlers
{
    public static class AddToDoListCommandHandlerTests
    {
        [TestFixture]
        public class when_add_a_new_command_list
        {
            [Test]
            public async Task then_should_add_a_new_to_do_list_with_the_provided_name()
            {
                // Arrange
                var passedGetKey = (string) null;
                var passedSetKey = (string) null;
                var passedObject = (object) null;
                var lists = new List<ToDoListModel> { Mock.Of<ToDoListModel>() };
                var cacheAccessor = Mock.Of<ICacheAccessor>();
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Get(It.IsAny<string>()))
                    .Callback<string>(key => passedGetKey = key)
                    .Returns(lists);
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Set(It.IsAny<string>(), It.IsAny<object>()))
                    .Callback<string, object>((key, obj) => { passedSetKey = key; passedObject = obj; });

                var queryHandler = new AddToDoListCommandHandler(cacheAccessor);

                // Act
                await queryHandler.HandleAsync(Mock.Of<IAddToDoListCommand>(command => command.Name == "test")).ConfigureAwait(false);

                // Assert
                Assert.That(passedGetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(passedSetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(passedObject, Is.EqualTo(lists));
                Assert.That(lists.Any(list => list.Name == "test"));
            }

            [Test]
            public async Task then_should_add_a_new_to_do_list_with_the_next_id()
            {
                // Arrange
                var passedGetKey = (string)null;
                var passedSetKey = (string)null;
                var passedObject = (object)null;
                var lists = new List<ToDoListModel> { Mock.Of<ToDoListModel>(list => list.Id == 1) };
                var cacheAccessor = Mock.Of<ICacheAccessor>();
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Get(It.IsAny<string>()))
                    .Callback<string>(key => passedGetKey = key)
                    .Returns(lists);
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Set(It.IsAny<string>(), It.IsAny<object>()))
                    .Callback<string, object>((key, obj) => { passedSetKey = key; passedObject = obj; });

                var queryHandler = new AddToDoListCommandHandler(cacheAccessor);

                // Act
                await queryHandler.HandleAsync(Mock.Of<IAddToDoListCommand>(command => command.Name == "test")).ConfigureAwait(false);

                // Assert
                Assert.That(passedGetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(passedSetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(passedObject, Is.EqualTo(lists));
                Assert.That(lists.Any(list => list.Id == 2 && list.Name == "test"));
            }
        }
    }
}
