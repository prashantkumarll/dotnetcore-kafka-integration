using Xunit;
using Moq;
using Api.Services;
using Microsoft.Extensions.Configuration;
using FluentAssertions;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_Should_Be_Instantiable_With_Configuration()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_Should_Accept_IConfiguration()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act & Assert
            var action = () => new ProcessOrdersService(mockConfiguration.Object);
            action.Should().NotThrow();
        }

        [Fact]
        public void ProcessOrdersService_Should_Be_In_Services_Namespace()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.GetType().Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Should_Be_Public_Class()
        {
            // Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_Should_Require_Configuration_Parameter()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var constructors = typeof(ProcessOrdersService).GetConstructors();
            var constructor = constructors[0];
            var parameters = constructor.GetParameters();

            // Assert
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be<IConfiguration>();
        }
    }
}