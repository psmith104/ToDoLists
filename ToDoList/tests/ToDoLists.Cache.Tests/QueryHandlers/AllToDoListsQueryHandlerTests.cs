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
    public static class AllToDoListsQueryHandlerTests
    {
        [TestFixture]
        public class when_retrieving_a_list_of_to_do_lists
        {
            [Test]
            public async Task then_should_get_the_list_from_the_cacheAsync()
            {
                // Arrange
                var passedKey = (string)null;
                var lists = new List<IToDoList> { Mock.Of<IToDoList>() };
                var cacheAccessor = Mock.Of<ICacheAccessor>();
                Mock.Get(cacheAccessor)
                    .Setup(accessor => accessor.Get(It.IsAny<string>()))
                    .Callback<string>(key => passedKey = key)
                    .Returns(lists);

                var queryHandler = new AllToDoListsQueryHandler(cacheAccessor);

                // Act
                var result = await queryHandler.HandleAsync(Mock.Of<IAllToDoListsQuery>()).ConfigureAwait(false);

                // Assert
                Assert.That(passedKey, Is.EqualTo(CacheKeys.ToDoLists));
                Assert.That(result, Is.EqualTo(lists));
            }
        }
    }
}
