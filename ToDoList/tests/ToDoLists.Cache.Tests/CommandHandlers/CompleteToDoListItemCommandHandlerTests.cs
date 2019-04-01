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
    public static class CompleteToDoListItemCommandHandlerTests
    {
        [TestFixture]
        public class when_completing_a_to_do_list_item
        {
            [Test]
            public async Task then_should_complete_the_requested_to_do_list_item()
            {
                // Arrange
                var passedGetKey = (string)null;
                var list = new ToDoListModel(1, "");
                var itemToComplete = new ToDoListItemModel(1, "");
                ((List<ToDoListItemModel>)list.Items).Add(itemToComplete);
                var lists = new List<ToDoListModel> { list };
                var cacheAccessor = Mock.Of<ICacheAccessor>();
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Get(It.IsAny<string>()))
                    .Callback<string>(key => passedGetKey = key)
                    .Returns(lists);

                var queryHandler = new CompleteToDoListItemCommandHandler(cacheAccessor);

                // Act
                await queryHandler.HandleAsync(Mock.Of<ICompleteToDoListItemCommand>(command => command.Id == 1)).ConfigureAwait(false);

                // Assert
                Assert.That(passedGetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(itemToComplete.IsCompleted, Is.True);
            }
        }
    }
}
