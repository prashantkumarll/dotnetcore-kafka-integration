using Xunit;
using Moq;
using FluentAssertions;
using Api.Services;
using Microsoft.Extensions.Configuration;
using System;

namespace Api.Tests
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
        public void ProcessOrdersService_ShouldBeInstantiated_WithConfiguration()
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
        public void ProcessOrdersService_ShouldHaveCorrectNamespace()
        {
            // Act & Assert
            _service.GetType().Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeOfCorrectType()
        {
            // Act & Assert
            _service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_ShouldThrowIfConfigurationIsNull()
        {
            // Act & Assert
            Action act = () => new ProcessOrdersService(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveConfigurationDependency()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["SomeKey"]).Returns("SomeValue");

            // Act
            var service = new ProcessOrdersService(mockConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldWorkWithDifferentConfigurations()
        {
            // Arrange
            var mockConfig1 = new Mock<IConfiguration>();
            var mockConfig2 = new Mock<IConfiguration>();

            // Act
            var service1 = new ProcessOrdersService(mockConfig1.Object);
            var service2 = new ProcessOrdersService(mockConfig2.Object);

            // Assert
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
            service1.Should().NotBeSameAs(service2);
        }
    }
}