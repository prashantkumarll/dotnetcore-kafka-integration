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
            type.Namespace.Should().Be("Api");
            type.Name.Should().Be("Startup");
        }

        [Fact]
        public void Startup_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void Startup_Constructor_WithConfiguration_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });

            // Assert
            constructor.Should().NotBeNull();
            constructor.IsPublic.Should().BeTrue();
            constructor.GetParameters().Should().HaveCount(1);
            constructor.GetParameters()[0].ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_Should_HaveConfigureServicesMethod()
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
        public void Startup_Should_HaveConfigureMethod()
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
        public void Startup_Assembly_ShouldBeCorrect()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var assembly = type.Assembly;

            // Assert
            assembly.Should().NotBeNull();
            assembly.GetName().Name.Should().Be("Api");
        }

        [Fact]
        public void Startup_Methods_ShouldHaveCorrectReturnTypes()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var configureServicesMethod = type.GetMethod("ConfigureServices");
            var configureMethod = type.GetMethod("Configure");

            // Assert
            configureServicesMethod.Should().NotBeNull();
            configureMethod.Should().NotBeNull();
            configureServicesMethod.ReturnType.Should().NotBeNull();
            configureMethod.ReturnType.Should().NotBeNull();
        }
    }
}