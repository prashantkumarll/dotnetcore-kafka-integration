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
        public void Startup_Constructor_ShouldRequireIConfiguration()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var constructors = type.GetConstructors();
            var mainConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);

            // Assert
            mainConstructor.Should().NotBeNull();
            var parameter = mainConstructor.GetParameters().First();
            parameter.ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_ConfigureServicesMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var method = type.GetMethod("ConfigureServices");

            // Assert
            method.Should().NotBeNull();
            method.Name.Should().Be("ConfigureServices");
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_ConfigureMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var method = type.GetMethod("Configure");

            // Assert
            method.Should().NotBeNull();
            method.Name.Should().Be("Configure");
            method.IsPublic.Should().BeTrue();
        }
    }
}