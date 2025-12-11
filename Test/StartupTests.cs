using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Api;

namespace Test
{
    public class StartupTests
    {
        [Fact]
        public void Startup_Type_ShouldBeInCorrectNamespace()
        {
            // Arrange & Act
            var type = typeof(Startup);

            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("Startup");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void Startup_ShouldHaveConstructorWithIConfigurationParameter()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var constructors = type.GetConstructors();
            var configurationConstructor = constructors.FirstOrDefault(c => 
                c.GetParameters().Length == 1 && 
                c.GetParameters().First().ParameterType == typeof(IConfiguration));

            // Assert
            configurationConstructor.Should().NotBeNull();
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
        public void Startup_TypeInfo_ShouldBePublic()
        {
            // Arrange
            var type = typeof(Startup);

            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void Startup_ConstructorParameterInfo_ShouldBeValid()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var constructor = type.GetConstructors().FirstOrDefault();
            var parameters = constructor.GetParameters();

            // Assert
            constructor.Should().NotBeNull();
            parameters.Should().HaveCount(1);
            parameters.First().ParameterType.Should().Be(typeof(IConfiguration));
        }
    }
}