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
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
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
            method.Name.Should().Be("ConfigureServices");
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
            method.Name.Should().Be("Configure");
            method.IsPublic.Should().BeTrue();
        }
    }
}