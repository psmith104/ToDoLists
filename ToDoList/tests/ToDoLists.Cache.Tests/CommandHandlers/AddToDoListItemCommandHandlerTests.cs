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
    public static class AddToDoListItemCommandHandlerTests
    {
        [TestFixture]
        public class when_add_a_new_list_item
        {
            [Test]
            public async Task then_should_add_a_new_item_with_the_provided_name()
            {
                // Arrange
                var passedGetKey = (string)null;
                var itemsToAddTo = new List<ToDoListItemModel> { new ToDoListItemModel(1, "") };
                var listToAddTo = new ToDoListModel(1, "");
                ((List<ToDoListItemModel>)listToAddTo.Items).AddRange(itemsToAddTo);
                var itemsToIgnore = new List<ToDoListItemModel> { new ToDoListItemModel(2, "") };
                var listToIgnore = new ToDoListModel(2, "");
                ((List<ToDoListItemModel>)listToIgnore.Items).AddRange(itemsToIgnore);
                var lists = new List<ToDoListModel> { listToAddTo, listToIgnore };
                var cacheAccessor = Mock.Of<ICacheAccessor>();
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Get(It.IsAny<string>()))
                    .Callback<string>(key => passedGetKey = key)
                    .Returns(lists);

                var queryHandler = new AddToDoListItemCommandHandler(cacheAccessor);

                // Act
                await queryHandler.HandleAsync(Mock.Of<IAddToDoListItemCommand>(command => command.ListId == 1 && command.Name == "test")).ConfigureAwait(false);

                // Assert
                Assert.That(passedGetKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(listToAddTo.Items.Any(itm => itm.Id == 3 && itm.Name == "test"));
            }
        }
    }
}
