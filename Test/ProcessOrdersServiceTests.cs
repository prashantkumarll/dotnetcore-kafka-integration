using Xunit;
using FluentAssertions;
using Api.Services;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_Should_BeInstantiable()
        {
            // Arrange & Act
            var service = new ProcessOrdersService();

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_Should_BeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Should_BeConcreteClass()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void ProcessOrdersService_Should_HaveParameterlessConstructor()
        {
            // Arrange & Act
            var constructor = typeof(ProcessOrdersService).GetConstructor(System.Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_Multiple_Instances_Should_Be_Different()
        {
            // Arrange & Act
            var service1 = new ProcessOrdersService();
            var service2 = new ProcessOrdersService();

            // Assert
            service1.Should().NotBeSameAs(service2);
        }
    }
}