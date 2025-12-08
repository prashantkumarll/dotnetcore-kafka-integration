using Xunit;
using Api.Services;
using FluentAssertions;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        private readonly ProcessOrdersService _processOrdersService;

        public ProcessOrdersServiceTests()
        {
            _processOrdersService = new ProcessOrdersService();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInstantiable()
        {
            // Arrange & Act
            var service = new ProcessOrdersService();

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var service = new ProcessOrdersService();

            // Assert
            service.GetType().Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveParameterlessConstructor()
        {
            // Arrange & Act
            var service = new ProcessOrdersService();

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange
            var service1 = new ProcessOrdersService();
            var service2 = new ProcessOrdersService();

            // Act & Assert
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
            service1.Should().NotBeSameAs(service2);
        }
    }
}