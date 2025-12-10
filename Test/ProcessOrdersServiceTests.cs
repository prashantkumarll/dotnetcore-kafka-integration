using Xunit;
using Moq;
using Api.Services;
using Microsoft.Extensions.Configuration;
using FluentAssertions;

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
        public void ProcessOrdersService_Should_Initialize_Successfully()
        {
            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_Should_Accept_Configuration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_With_Null_Configuration_Should_Throw()
        {
            // Act & Assert
            var act = () => new ProcessOrdersService(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ProcessOrdersService_Type_Should_Be_In_Correct_Namespace()
        {
            // Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Namespace.Should().Be("Api.Services");
            type.Name.Should().Be("ProcessOrdersService");
        }

        [Fact]
        public void ProcessOrdersService_Should_Be_Concrete_Class()
        {
            // Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
            type.IsInterface.Should().BeFalse();
        }
    }
}