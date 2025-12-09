using System;
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
        private readonly ProcessOrdersService _processOrdersService;

        public ProcessOrdersServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _processOrdersService = new ProcessOrdersService(_mockConfiguration.Object);
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeCreated_WithConfiguration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldAcceptIConfiguration()
        {
            // Arrange
            var config = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(config.Object);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInServicesNamespace()
        {
            // Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveConfigurationConstructor()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldNotThrowOnCreation()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act & Assert
            var exception = Record.Exception(() => new ProcessOrdersService(mockConfig.Object));
            exception.Should().BeNull();
        }
    }
}