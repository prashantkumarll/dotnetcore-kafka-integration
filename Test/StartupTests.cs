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
        public void Startup_Constructor_ShouldAcceptIConfiguration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["TestSetting"]).Returns("TestValue");

            // Act
            var startup = new Startup(mockConfig.Object);

            // Assert
            startup.Should().NotBeNull();
            startup.Should().BeOfType<Startup>();
        }

        [Fact]
        public void Startup_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var methods = type.GetMethods().Where(m => m.DeclaringType == type);
            var methodNames = methods.Select(m => m.Name).ToList();

            // Assert
            methodNames.Should().Contain("ConfigureServices");
            methodNames.Should().Contain("Configure");
        }

        [Fact]
        public void Startup_TypeInfo_ShouldBeCorrect()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }
    }
}