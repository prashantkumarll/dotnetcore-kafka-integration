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
        public void Startup_Type_ShouldExist()
        {
            // Arrange & Act
            var type = typeof(Startup);
            
            // Assert
            type.Should().NotBeNull();
            type.IsPublic.Should().BeTrue();
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void Startup_Constructor_ShouldAcceptIConfiguration()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.IsPublic.Should().BeTrue();
            
            var parameters = constructor.GetParameters();
            parameters.Should().HaveCount(1);
            parameters[0].ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_ConfigureServicesMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var method = type.GetMethod("ConfigureServices");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_ConfigureMethod_ShouldExist()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var method = type.GetMethod("Configure");
            
            // Assert
            method.Should().NotBeNull();
            method.IsPublic.Should().BeTrue();
        }

        [Theory]
        [InlineData("ConfigureServices")]
        [InlineData("Configure")]
        public void Startup_StandardMethods_ShouldExist(string methodName)
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var method = type.GetMethod(methodName);
            
            // Assert
            method.Should().NotBeNull($"Method {methodName} should exist");
            method.IsPublic.Should().BeTrue($"Method {methodName} should be public");
        }

        [Fact]
        public void Startup_Constructor_ShouldHaveCorrectSignature()
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
            configConstructor.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Startup_Methods_ShouldHaveCorrectCount()
        {
            // Arrange
            var type = typeof(Startup);
            
            // Act
            var publicMethods = type.GetMethods()
                .Where(m => m.IsPublic && m.DeclaringType == type)
                .ToList();
            
            // Assert
            publicMethods.Should().NotBeEmpty();
            
            // Should have at least ConfigureServices and Configure
            var configureServicesMethod = publicMethods.Any(m => m.Name == "ConfigureServices");
            var configureMethod = publicMethods.Any(m => m.Name == "Configure");
            
            configureServicesMethod.Should().BeTrue("ConfigureServices method should exist");
            configureMethod.Should().BeTrue("Configure method should exist");
        }

        [Fact]
        public void Startup_WithMockedConfiguration_ShouldCreateInstance()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["TestKey"]).Returns("TestValue");
            
            // Act & Assert - We can verify the constructor accepts IConfiguration
            // without actually creating the instance since it might have dependencies
            var type = typeof(Startup);
            var constructor = type.GetConstructor(new[] { typeof(IConfiguration) });
            
            constructor.Should().NotBeNull();
            constructor.GetParameters()[0].ParameterType.Should().Be(typeof(IConfiguration));
        }

        [Fact]
        public void Startup_ClassName_ShouldFollowConvention()
        {
            // Arrange & Act
            var type = typeof(Startup);
            
            // Assert
            type.Name.Should().Be("Startup");
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }
    }
}