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
        public void Startup_Constructor_ShouldCreateInstance()
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
        public void Startup_ShouldHaveConfigureServicesMethod()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var configureServicesMethod = type.GetMethods().FirstOrDefault(m => m.Name == "ConfigureServices");
            
            // Assert
            configureServicesMethod.Should().NotBeNull();
        }

        [Fact]
        public void Startup_ShouldHaveConfigureMethod()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var configureMethod = type.GetMethods().FirstOrDefault(m => m.Name == "Configure");
            
            // Assert
            configureMethod.Should().NotBeNull();
        }
    }
}