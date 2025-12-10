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
        public void ProcessOrdersService_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithNullConfiguration_ShouldNotThrow()
        {
            // Arrange & Act
            var action = () => new ProcessOrdersService(null);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithMockConfiguration_ShouldAcceptParameter()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act & Assert
            var action = () => new ProcessOrdersService(mockConfig.Object);
            action.Should().NotThrow();
        }

        [Fact]
        public void ProcessOrdersService_Instance_ShouldNotBeNull()
        {
            // Assert
            _service.Should().NotBeNull();
        }
    }
}