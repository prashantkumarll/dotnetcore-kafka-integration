using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.Configuration;
using Api;

namespace Test
{
    public class StartupTests
    {
        [Fact]
        public void Startup_Type_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("Startup");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void Startup_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var methods = type.GetMethods().Where(m => m.DeclaringType == type).ToList();

            // Assert
            methods.Should().NotBeEmpty();
            methods.Should().Contain(m => m.Name == "ConfigureServices");
            methods.Should().Contain(m => m.Name == "Configure");
        }

        [Fact]
        public void Startup_ShouldHaveConstructorWithConfigurationParameter()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });

            // Assert
            constructor.Should().NotBeNull();
            constructor.IsPublic.Should().BeTrue();
            constructor.GetParameters().Should().HaveCount(1);
            constructor.GetParameters().First().ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_ConfigureServices_MethodShouldExist()
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
        public void Startup_Configure_MethodShouldExist()
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
        public void Startup_Constructor_ShouldAcceptConfiguration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["TestKey"]).Returns("TestValue");

            // Act
            Action createAction = () => 
            {
                var constructorInfo = typeof(Startup).GetConstructor(new[] { typeof(IConfiguration) });
                constructorInfo.Should().NotBeNull();
            };

            // Assert
            createAction.Should().NotThrow();
        }

        [Fact]
        public void Startup_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
            type.IsSealed.Should().BeFalse();
        }
    }
}