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
        public void Startup_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("Startup");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void Startup_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void Startup_ShouldHaveConfigurationConstructor()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var constructors = type.GetConstructors();
            var configurationConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType == typeof(IConfiguration));
            
            // Assert
            configurationConstructor.Should().NotBeNull();
            var parameter = configurationConstructor.GetParameters()[0];
            parameter.Name.Should().Be("configuration");
        }

        [Fact]
        public void Startup_Constructor_WithMockedConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            
            // Act
            var startup = new Startup(mockConfiguration.Object);
            
            // Assert
            startup.Should().NotBeNull();
            startup.Should().BeOfType<Startup>();
        }

        [Fact]
        public void Startup_ShouldHaveConfigureServicesMethod()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var method = type.GetMethod("ConfigureServices");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_ShouldHaveConfigureMethod()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var method = type.GetMethod("Configure");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Theory]
        [InlineData("ConfigureServices")]
        [InlineData("Configure")]
        public void Startup_PublicMethods_ShouldExist(string methodName)
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var method = type.GetMethod(methodName);
            
            // Assert
            method.Should().NotBeNull($"Method {methodName} should exist");
            method.IsPublic.Should().BeTrue($"Method {methodName} should be public");
        }
    }
}