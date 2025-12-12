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
        public void Startup_Type_ShouldHaveExpectedAttributes()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("Startup");
            type.Namespace.Should().Be("Api");
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void Startup_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_Type_ShouldHaveExpectedConstructor()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var constructors = type.GetConstructors();
            var targetConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType == typeof(IConfiguration));

            // Assert
            targetConstructor.Should().NotBeNull();
        }

        [Fact]
        public void Startup_Type_ShouldHaveConfigureServicesMethod()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var method = type.GetMethod("ConfigureServices");

            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }
    }
}