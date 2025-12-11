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
        }

        [Fact]
        public void Startup_Constructor_WithNullConfiguration_ShouldCreateInstance()
        {
            // Act & Assert
            Action createStartup = () => new Startup(null);
            createStartup.Should().NotThrow();
        }

        [Fact]
        public void Startup_Type_ShouldHaveExpectedMethods()
        {
            // Arrange
            var startupType = typeof(Startup);

            // Act
            var methods = startupType.GetMethods().Where(m => m.DeclaringType == startupType);

            // Assert
            methods.Should().Contain(m => m.Name == "ConfigureServices");
            methods.Should().Contain(m => m.Name == "Configure");
        }

        [Fact]
        public void Startup_Constructor_ShouldHaveCorrectParameters()
        {
            // Arrange
            var startupType = typeof(Startup);

            // Act
            var constructors = startupType.GetConstructors();
            var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);

            // Assert
            constructor.Should().NotBeNull();
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be<IConfiguration>();
            parameters[0].Name.Should().Be("configuration");
        }

        [Fact]
        public void Startup_Namespace_ShouldBeCorrect()
        {
            // Arrange
            var startupType = typeof(Startup);

            // Act
            var namespaceName = startupType.Namespace;

            // Assert
            namespaceName.Should().Be("Api");
        }

        [Fact]
        public void Startup_ConfigureServices_ShouldExist()
        {
            // Arrange
            var startupType = typeof(Startup);

            // Act
            var configureServicesMethod = startupType.GetMethod("ConfigureServices");

            // Assert
            configureServicesMethod.Should().NotBeNull();
            configureServicesMethod.Name.Should().Be("ConfigureServices");
        }

        [Fact]
        public void Startup_Configure_ShouldExist()
        {
            // Arrange
            var startupType = typeof(Startup);

            // Act
            var configureMethod = startupType.GetMethod("Configure");

            // Assert
            configureMethod.Should().NotBeNull();
            configureMethod.Name.Should().Be("Configure");
        }

        [Fact]
        public void Startup_WithMockedConfiguration_ShouldAcceptConfigurationValues()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["TestSetting"]).Returns("TestValue");

            // Act
            var startup = new Startup(mockConfiguration.Object);

            // Assert
            startup.Should().NotBeNull();
            mockConfiguration.Verify(c => c["TestSetting"], Times.Never);
        }
    }
}