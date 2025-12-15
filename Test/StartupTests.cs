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
        public void Startup_Constructor_WithValidConfiguration_ShouldCreateInstance()
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
        public void Startup_Type_ShouldHaveExpectedProperties()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("Startup");
            type.Namespace.Should().Be("Api");
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_Constructor_ShouldAcceptIConfigurationParameter()
        {
            // Arrange
            var constructors = typeof(Startup).GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);

            // Act & Assert
            targetConstructor.Should().NotBeNull();
            targetConstructor.GetParameters().First().ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_PublicMethods_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var methodNames = methods.Select(m => m.Name).ToList();

            // Assert
            methods.Should().NotBeEmpty();
            methodNames.Should().Contain("ConfigureServices");
            methodNames.Should().Contain("Configure");
        }
    }
}