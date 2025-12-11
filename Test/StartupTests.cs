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
        public void Startup_ShouldHaveConfigurationConstructor()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(1);
            constructor.GetParameters().First().ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_ShouldHaveRequiredMethods()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var methods = type.GetMethods().Select(m => m.Name);
            
            // Assert
            methods.Should().Contain("ConfigureServices");
            methods.Should().Contain("Configure");
        }

        [Fact]
        public void Startup_Constructor_WithMockedConfiguration_ShouldWork()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["SomeKey"]).Returns("SomeValue");
            
            // Act
            var startup = new Startup(mockConfiguration.Object);
            
            // Assert
            startup.Should().NotBeNull();
            startup.Should().BeOfType<Startup>();
        }

        [Fact]
        public void Startup_TypeInfo_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
            type.IsSealed.Should().BeFalse();
        }
    }
}