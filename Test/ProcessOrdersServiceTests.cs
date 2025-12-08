using Xunit;
using Api.Services;
using FluentAssertions;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_ShouldBeInstantiable()
        {
            // Act
            var service = new ProcessOrdersService();

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Namespace.Should().Be("Api.Services");
            type.FullName.Should().Be("Api.Services.ProcessOrdersService");
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveDefaultConstructor()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act
            var constructor = type.GetConstructor(System.Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeReferenceType()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsClass.Should().BeTrue();
            type.IsValueType.Should().BeFalse();
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_ShouldBeUnique()
        {
            // Act
            var service1 = new ProcessOrdersService();
            var service2 = new ProcessOrdersService();

            // Assert
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
            service1.Should().NotBeSameAs(service2);
        }

        [Fact]
        public void ProcessOrdersService_ShouldHavePublicVisibility()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsPublic.Should().BeTrue();
            type.IsNotPublic.Should().BeFalse();
        }
    }
}