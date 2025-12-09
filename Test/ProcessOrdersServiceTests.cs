using Api.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly ProcessOrdersService _service;

        public ProcessOrdersServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _service = new ProcessOrdersService(_mockConfiguration.Object);
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInitializedWithConfiguration()
        {
            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldAcceptIConfiguration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_TypeShouldBeClass()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Should().NotBeNull();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_WithNullConfiguration_ShouldNotThrow()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(null);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_MultipleInstances_ShouldBeUnique()
        {
            // Arrange
            var mockConfig1 = new Mock<IConfiguration>();
            var mockConfig2 = new Mock<IConfiguration>();

            // Act
            var service1 = new ProcessOrdersService(mockConfig1.Object);
            var service2 = new ProcessOrdersService(mockConfig2.Object);

            // Assert
            service1.Should().NotBeSameAs(service2);
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
        }
    }
}