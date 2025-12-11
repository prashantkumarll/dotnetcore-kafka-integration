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
        public void Startup_Type_ShouldHaveCorrectProperties()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("Startup");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void Startup_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void Startup_Constructor_WithIConfiguration_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var constructors = type.GetConstructors();
            var configConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType == typeof(IConfiguration));

            // Assert
            configConstructor.Should().NotBeNull();
        }

        [Fact]
        public void Startup_Methods_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var methods = type.GetMethods().Select(m => m.Name).ToList();

            // Assert
            methods.Should().Contain("ConfigureServices");
            methods.Should().Contain("Configure");
        }

        [Fact]
        public void Startup_ConfigureServices_ShouldExist()
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
        public void Startup_Configure_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var configureMethod = type.GetMethod("Configure");

            // Assert
            configureMethod.Should().NotBeNull();
            configureMethod.IsPublic.Should().BeTrue();
        }
    }
}