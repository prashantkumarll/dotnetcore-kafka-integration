using System;
using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Configuration;
using Api.Services;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_Constructor_ShouldAcceptConfiguration()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.GetType().Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_ShouldHaveCorrectClassName()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var service = new ProcessOrdersService(mockConfiguration.Object);

            // Assert
            service.GetType().Name.Should().Be("ProcessOrdersService");
        }

        [Fact]
        public void ProcessOrdersService_ShouldBeReferenceType()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithNullConfiguration_ShouldNotThrow()
        {
            // Arrange
            IConfiguration configuration = null;

            // Act
            Action act = () => new ProcessOrdersService(configuration);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ProcessOrdersService_ShouldNotBeAbstract()
        {
            // Arrange & Act
            var type = typeof(ProcessOrdersService);

            // Assert
            type.IsAbstract.Should().BeFalse();
        }
    }
}