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
        }

        [Fact]
        public void Startup_Type_ShouldHaveConfigurationConstructor()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var constructors = type.GetConstructors();
            var configConstructor = constructors
                .FirstOrDefault(c => c.GetParameters().Length == 1 && 
                               c.GetParameters().First().ParameterType == typeof(IConfiguration));
            
            // Assert
            configConstructor.Should().NotBeNull();
            configConstructor.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_Type_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(Startup);
            var expectedMethods = new[] { "ConfigureServices", "Configure" };
            
            // Act
            var methods = type.GetMethods().Where(m => m.DeclaringType == type);
            var methodNames = methods.Select(m => m.Name).ToList();
            
            // Assert
            foreach (var expectedMethod in expectedMethods)
            {
                methodNames.Should().Contain(expectedMethod);
            }
        }

        [Fact]
        public void Startup_Constructor_WithMockedConfiguration_ShouldWork()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            
            // Act
            var startup = new Startup(mockConfig.Object);
            
            // Assert
            startup.Should().NotBeNull();
            startup.Should().BeOfType<Startup>();
        }

        [Theory]
        [InlineData("ConfigureServices")]
        [InlineData("Configure")]
        public void Startup_ShouldHaveRequiredMethods(string methodName)
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var method = type.GetMethod(methodName);
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }
    }
}