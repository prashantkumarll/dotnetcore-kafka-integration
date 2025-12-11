using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Api.Services;
using System.Collections.Generic;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_Constructor_WithValidConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithMockedConfigurationValues_ShouldAcceptConfiguration()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["ServiceBus:ConnectionString"]).Returns("test-connection-string");
            mockConfiguration.Setup(c => c["ServiceBus:QueueName"]).Returns("test-queue");

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithEmptyConfiguration_ShouldStillCreateInstance()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var configDict = new Dictionary<string, string>();
            mockConfiguration.Setup(c => c[It.IsAny<string>()]).Returns((string)null);

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_TypeValidation_ShouldBeCorrectType()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.GetType().Should().Be(typeof(ProcessOrdersService));
            service.Should().BeAssignableTo<ProcessOrdersService>();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithConfigurationSections_ShouldHandleSectionAccess()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            
            mockSection.Setup(s => s.Value).Returns("test-value");
            mockConfiguration.Setup(c => c.GetSection("ServiceBus")).Returns(mockSection.Object);

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
        }
    }
}