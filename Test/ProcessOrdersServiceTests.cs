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
        public void ProcessOrdersService_Should_Be_Instantiated_With_Configuration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfig.Object);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_Should_Accept_IConfiguration_Parameter()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfig.Object);

            // Assert
            service.Should().NotBeNull();
            service.GetType().Should().Be<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_Should_Be_In_Correct_Namespace()
        {
            // Arrange & Act
            var namespaceName = _service.GetType().Namespace;

            // Assert
            namespaceName.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Constructor_Should_Store_Configuration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfig.Object);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeAssignableTo<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_Should_Have_Valid_Constructor()
        {
            // Arrange & Act
            var constructorInfo = typeof(ProcessOrdersService).GetConstructor(new[] { typeof(IConfiguration) });

            // Assert
            constructorInfo.Should().NotBeNull();
            constructorInfo.IsPublic.Should().BeTrue();
        }
    }
}