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
        public void Startup_Type_ShouldHaveExpectedAttributes()
        {
            // Arrange
            var type = typeof(Startup);

            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("Startup");
            type.Namespace.Should().Be("Api");
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void Startup_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var methods = type.GetMethods();
            var methodNames = methods.Select(m => m.Name).ToList();

            // Assert
            methodNames.Should().Contain("ConfigureServices");
            methodNames.Should().Contain("Configure");
        }

        [Fact]
        public void Startup_Constructor_ShouldAcceptIConfiguration()
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
        public void Startup_Multiple_Instances_ShouldBeIndependent()
        {
            // Arrange
            var mockConfiguration1 = new Mock<IConfiguration>();
            var mockConfiguration2 = new Mock<IConfiguration>();

            // Act
            var startup1 = new Startup(mockConfiguration1.Object);
            var startup2 = new Startup(mockConfiguration2.Object);

            // Assert
            startup1.Should().NotBeSameAs(startup2);
        }
    }
}