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
using Api;

namespace Test
{
    public class StartupTests
    {
        [Fact]
        public void Startup_Type_ShouldHaveExpectedProperties()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("Startup");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void Startup_Constructor_ShouldAcceptIConfiguration()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(1);
            constructor.GetParameters()[0].ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_ShouldHaveConfigureServicesMethod()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var configureServicesMethod = type.GetMethod("ConfigureServices");
            
            // Assert
            configureServicesMethod.Should().NotBeNull();
            configureServicesMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_ShouldHaveConfigureMethod()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var configureMethod = type.GetMethod("Configure");
            
            // Assert
            configureMethod.Should().NotBeNull();
            configureMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_WithMockedConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ConnectionStrings:ServiceBus"]).Returns("test-connection-string");
            
            // Act
            var startup = new Startup(mockConfig.Object);
            
            // Assert
            startup.Should().NotBeNull();
            startup.Should().BeOfType<Startup>();
        }
    }
}