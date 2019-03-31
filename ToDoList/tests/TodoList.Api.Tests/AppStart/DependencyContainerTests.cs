using NUnit.Framework;
using ToDoList.Api;

namespace TodoList.Api.Tests.AppStart
{
    public static class DependencyContainerTests
    {
        [TestFixture]
        public class when_registering_dependencies
        {
            [Test]
            public void then_should_not_throw_an_error()
            {
                // Arrange
                var container = new DependencyContainer();

                // Act, Assert
                Assert.That(container.RegisterDependencies, Throws.Nothing);
            }
        }
    }
}
