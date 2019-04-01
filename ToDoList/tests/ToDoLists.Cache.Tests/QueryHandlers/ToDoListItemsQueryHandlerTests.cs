using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Cache.Helpers;
using ToDoList.Cache.QueryHandlers;
using ToDoList.Cache.Services;
using ToDoList.Domain.Models;
using ToDoList.Domain.Queries;

namespace ToDoLists.Cache.Tests.QueryHandlers
{
    public static class ToDoListItemsQueryHandlerTests
    {
        [TestFixture]
        public class when_retrieving_the_items_on_a_to_do_list
        {
            [Test]
            public async Task then_should_get_the_items_from_the_cached_list_with_the_given_id()
            {
                // Arrange
                var query = Mock.Of<IToDoListItemsQuery>(qry => qry.ListId == 2);
                var expectedItems = new List<IToDoListItem> { Mock.Of<IToDoListItem>() };
                var list1 = Mock.Of<IToDoList>(list => list.Id == 1);
                var list2 = Mock.Of<IToDoList>(list => list.Id == 2 && list.Items == expectedItems);

                var passedKey = (string)null;
                var cacheAccessor = Mock.Of<ICacheAccessor>();
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Get(It.IsAny<string>()))
                    .Callback<string>(key => passedKey = key)
                    .Returns(new List<IToDoList> { list1, list2 });

                var queryHandler = new ToDoListItemsQueryHandler(cacheAccessor);

                // Act
                var result = await queryHandler.HandleAsync(query).ConfigureAwait(false);

                // Assert
                Assert.That(passedKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(result, Is.EqualTo(expectedItems));
            }
        }
    }
}
