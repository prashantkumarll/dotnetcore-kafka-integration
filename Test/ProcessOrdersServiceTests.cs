using Xunit;
using Moq;
using FluentAssertions;
using Api.Services;
using Microsoft.Extensions.Configuration;

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
        public void ProcessOrdersService_ShouldBeInstantiated_WithValidConfiguration()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_ShouldAcceptIConfigurationInConstructor()
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
            var serviceType = typeof(ProcessOrdersService);

            // Assert
            serviceType.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Constructor_ShouldNotThrowException()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var action = () => new ProcessOrdersService(mockConfig.Object);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHavePublicConstructor()
        {
            // Arrange & Act
            var constructors = typeof(ProcessOrdersService).GetConstructors();

            // Assert
            constructors.Should().NotBeEmpty();
            constructors.Should().Contain(c => c.GetParameters().Length == 1);
        }
    }
}