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
        public void ProcessOrdersService_ShouldInstantiateSuccessfully()
        {
            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveCorrectConstructor()
        {
            // Act
            var constructors = typeof(ProcessOrdersService).GetConstructors();

            // Assert
            constructors.Should().HaveCount(1);
            var constructor = constructors[0];
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void ProcessOrdersService_ShouldAcceptConfigurationInConstructor()
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
            // Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_WithNullConfiguration_ShouldThrowException()
        {
            // Act
            Action act = () => new ProcessOrdersService(null);

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}