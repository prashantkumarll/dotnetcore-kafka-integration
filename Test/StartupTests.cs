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

namespace Test
{
    public class StartupTests
    {
        [Fact]
        public void Startup_Type_ShouldHaveExpectedAttributes()
        {
            // Arrange & Act
            var type = typeof(Api.Startup);
            
            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("Startup");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void Startup_Constructor_ShouldAcceptIConfiguration()
        {
            // Arrange
            var type = typeof(Api.Startup);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(1);
            constructor.GetParameters().First().ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(Api.Startup);
            
            // Act
            var methods = type.GetMethods().Where(m => m.IsPublic && m.DeclaringType == type);
            var methodNames = methods.Select(m => m.Name).ToList();
            
            // Assert
            methodNames.Should().Contain("ConfigureServices");
            methodNames.Should().Contain("Configure");
        }

        [Fact]
        public void Startup_ConfigureServices_ShouldHaveCorrectSignature()
        {
            // Arrange
            var type = typeof(Api.Startup);
            
            // Act
            var method = type.GetMethod("ConfigureServices");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_Configure_ShouldHaveCorrectSignature()
        {
            // Arrange
            var type = typeof(Api.Startup);
            
            // Act
            var method = type.GetMethod("Configure");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }
    }
}