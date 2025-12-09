using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Api.Services;
using FluentAssertions;

namespace Api.Tests.Services
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_Constructor_ShouldAcceptConfiguration()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var serviceType = typeof(ProcessOrdersService);

            // Assert
            serviceType.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithNullConfiguration_ShouldThrowArgumentNullException()
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<System.ArgumentNullException>(() => new ProcessOrdersService(null));
            exception.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeConcreteClass()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.GetType().IsClass.Should().BeTrue();
            service.GetType().IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_ShouldBeUnique()
        {
            // Arrange
            var mockConfiguration1 = new Mock<IConfiguration>();
            var mockConfiguration2 = new Mock<IConfiguration>();

            // Act
            var service1 = new ProcessOrdersService(mockConfiguration1.Object);
            var service2 = new ProcessOrdersService(mockConfiguration2.Object);

            // Assert
            service1.Should().NotBeSameAs(service2);
        }
    }
}