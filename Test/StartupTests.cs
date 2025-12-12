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
        public void Startup_Type_ShouldHaveExpectedAttributes()
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
            var methods = type.GetMethods().Select(m => m.Name).ToArray();
            
            // Assert
            methods.Should().Contain("ConfigureServices");
            methods.Should().Contain("Configure");
        }
        
        [Fact]
        public void Startup_Constructor_ShouldAcceptIConfiguration()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var constructors = type.GetConstructors();
            var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 1);
            
            // Assert
            constructor.Should().NotBeNull();
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be(typeof(IConfiguration));
        }
        
        [Fact]
        public void Startup_IsPublicClass_ShouldBeTrue()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var isPublic = type.IsPublic;
            
            // Assert
            isPublic.Should().BeTrue();
        }
    }
}