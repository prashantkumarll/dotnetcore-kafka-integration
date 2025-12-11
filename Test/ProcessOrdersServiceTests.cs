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
        public void ProcessOrdersService_Type_ShouldHaveCorrectProperties()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProcessOrdersService");
            type.Namespace.Should().Be("Api.Services");
        }

        [Fact]
        public void ProcessOrdersService_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_WithIConfiguration_ShouldExist()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act
            var constructors = type.GetConstructors();
            var configConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType == typeof(IConfiguration));

            // Assert
            configConstructor.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_Constructor_ShouldAcceptIConfiguration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ConnectionStrings:ServiceBus"]).Returns("test-connection-string");

            // Act
            var type = typeof(ProcessOrdersService);
            var constructor = type.GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length == 1 && 
                               c.GetParameters()[0].ParameterType == typeof(IConfiguration));

            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters()[0].ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void ProcessOrdersService_ConstructorParameters_ShouldHaveCorrectType()
        {
            // Arrange
            var type = typeof(ProcessOrdersService);

            // Act
            var constructors = type.GetConstructors();

            // Assert
            constructors.Should().HaveCount(1);
            var constructor = constructors.First();
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be(typeof(IConfiguration));
        }
    }
}