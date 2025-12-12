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
        public void Startup_Constructor_WithConfiguration_ShouldCreateInstance()
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
        public void Startup_ShouldHaveRequiredMethods()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var configureServicesMethod = type.GetMethod("ConfigureServices");
            var configureMethod = type.GetMethod("Configure");

            // Assert
            configureServicesMethod.Should().NotBeNull();
            configureMethod.Should().NotBeNull();
        }

        [Fact]
        public void Startup_Type_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(Startup);

            // Assert
            type.Namespace.Should().Be("Api");
            type.Name.Should().Be("Startup");
            type.IsPublic.Should().BeTrue();
        }
    }
}