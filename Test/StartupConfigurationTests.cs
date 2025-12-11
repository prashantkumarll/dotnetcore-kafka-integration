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
    public class StartupConfigurationTests
    {
        [Fact]
        public void Startup_Constructor_ShouldAcceptConfiguration()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            // Act
            var startup = new Startup(mockConfiguration.Object);

            // Assert
            startup.Should().NotBeNull();
        }

        [Fact]
        public void Startup_Type_ShouldHaveExpectedStructure()
        {
            // Arrange
            var startupType = typeof(Startup);

            // Act
            var constructors = startupType.GetConstructors();
            var methods = startupType.GetMethods();

            // Assert
            startupType.Should().NotBeNull();
            startupType.Namespace.Should().Be("Api");
            constructors.Should().NotBeEmpty();
            
            var configConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters()[0].ParameterType == typeof(IConfiguration));
            
            configConstructor.Should().NotBeNull();
        }

        [Fact]
        public void Startup_ConfigureServicesMethod_ShouldExist()
        {
            // Arrange
            var startupType = typeof(Startup);

            // Act
            var configureServicesMethod = startupType.GetMethods()
                .FirstOrDefault(m => m.Name == "ConfigureServices");

            // Assert
            configureServicesMethod.Should().NotBeNull();
            configureServicesMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_ConfigureMethod_ShouldExist()
        {
            // Arrange
            var startupType = typeof(Startup);

            // Act
            var configureMethod = startupType.GetMethods()
                .FirstOrDefault(m => m.Name == "Configure");

            // Assert
            configureMethod.Should().NotBeNull();
            configureMethod.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_WithNullConfiguration_ShouldStillCreateInstance()
        {
            // Arrange & Act
            var startup = new Startup(null);

            // Assert
            startup.Should().NotBeNull();
        }

        [Fact]
        public void Startup_WithMockedConfiguration_ShouldCreateSuccessfully()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["ConnectionStrings:ServiceBus"]).Returns("test-connection");
            mockConfiguration.Setup(c => c["Logging:LogLevel:Default"]).Returns("Information");

            // Act
            var startup = new Startup(mockConfiguration.Object);

            // Assert
            startup.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Development")]
        [InlineData("Production")]
        [InlineData("Staging")]
        public void Startup_WithDifferentEnvironments_ShouldCreateSuccessfully(string environment)
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["ASPNETCORE_ENVIRONMENT"]).Returns(environment);

            // Act
            var startup = new Startup(mockConfiguration.Object);

            // Assert
            startup.Should().NotBeNull();
        }

        [Fact]
        public void Startup_Methods_ShouldHaveCorrectVisibility()
        {
            // Arrange
            var startupType = typeof(Startup);

            // Act
            var publicMethods = startupType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            // Assert
            publicMethods.Should().NotBeEmpty();
            publicMethods.Should().Contain(m => m.Name == "ConfigureServices");
            publicMethods.Should().Contain(m => m.Name == "Configure");
            
            foreach (var method in publicMethods)
            {
                method.IsPublic.Should().BeTrue();
            }
        }
    }
}