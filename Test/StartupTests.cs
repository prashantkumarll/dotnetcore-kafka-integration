// REQUIRED using statements - COPY THIS BLOCK TO EVERY TEST FILE:
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
        public void Startup_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void Startup_Constructor_ShouldAcceptIConfiguration()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var constructors = type.GetConstructors();
            var expectedConstructor = constructors.FirstOrDefault(c => 
            {
                var parameters = c.GetParameters();
                return parameters.Length == 1 && 
                       parameters[0].ParameterType == typeof(IConfiguration);
            });
            
            // Assert
            expectedConstructor.Should().NotBeNull("Startup should have constructor with IConfiguration parameter");
        }

        [Fact]
        public void Startup_Type_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var methods = type.GetMethods().Where(m => m.IsPublic && !m.IsSpecialName).Select(m => m.Name).ToList();
            
            // Assert
            methods.Should().Contain("ConfigureServices");
            methods.Should().Contain("Configure");
        }

        [Fact]
        public void Startup_WithMockedConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["TestKey"]).Returns("TestValue");
            
            // Act
            var startup = new Startup(mockConfig.Object);
            
            // Assert
            startup.Should().NotBeNull();
            startup.Should().BeOfType<Startup>();
        }

        [Fact]
        public void Startup_ConfigureServices_ShouldExistAsMethod()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var configureServicesMethod = type.GetMethod("ConfigureServices");
            
            // Assert
            configureServicesMethod.Should().NotBeNull();
            configureServicesMethod.IsPublic.Should().BeTrue();
        }
    }
}