using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Api.Services;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void ProcessOrdersService_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProcessOrdersService");
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act & Assert
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_ShouldHaveConfigurationParameter()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);
            var constructors = type.GetConstructors();

            // Act
            var constructor = constructors.FirstOrDefault();

            // Assert
            constructor.Should().NotBeNull();
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(1);
            parameters.First().ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void ProcessOrdersService_WithMockedConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            
            mockSection.Setup(s => s.Value).Returns("test-connection-string");
            mockConfig.Setup(c => c.GetSection(It.IsAny<string>())).Returns(mockSection.Object);
            mockConfig.Setup(c => c[It.IsAny<string>()]).Returns("test-value");

            // Act
            var service = new ProcessOrdersService(mockConfig.Object);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeOfType<ProcessOrdersService>();
        }
    }
}