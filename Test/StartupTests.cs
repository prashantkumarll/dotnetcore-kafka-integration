using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using FluentAssertions;
using Moq;
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
            constructors.Should().NotBeNull();
            configConstructor.Should().NotBeNull();
        }

        [Fact]
        public void Startup_RequiredMethods_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var methods = type.GetMethods();
            var configureServicesMethod = methods.FirstOrDefault(m => m.Name == "ConfigureServices");
            var configureMethod = methods.FirstOrDefault(m => m.Name == "Configure");

            // Assert
            configureServicesMethod.Should().NotBeNull();
            configureMethod.Should().NotBeNull();
        }

        [Fact]
        public void Startup_Type_ShouldBePublic()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var isPublic = type.IsPublic;

            // Assert
            isPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_Methods_ShouldBePublic()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var configureServices = type.GetMethod("ConfigureServices");
            var configure = type.GetMethod("Configure");

            // Assert
            configureServices?.IsPublic.Should().BeTrue();
            configure?.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_Constructor_Parameter_ShouldBeNamed()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var constructor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 1);
            var parameter = constructor?.GetParameters().FirstOrDefault();

            // Assert
            parameter.Should().NotBeNull();
            parameter?.ParameterType.Should().Be<IConfiguration>();
        }

        [Fact]
        public void Startup_Type_ShouldNotBeAbstract()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var isAbstract = type.IsAbstract;

            // Assert
            isAbstract.Should().BeFalse();
        }

        [Fact]
        public void Startup_Type_ShouldNotBeSealed()
        {
            // Arrange
            var type = typeof(Startup);

            // Act
            var isSealed = type.IsSealed;

            // Assert
            isSealed.Should().BeFalse();
        }
    }
}