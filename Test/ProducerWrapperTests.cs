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
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldHaveExpectedConstructor()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var constructors = type.GetConstructors();
            var parameterizedConstructor = constructors
                .FirstOrDefault(c => c.GetParameters().Length == 2 && 
                               c.GetParameters().All(p => p.ParameterType == typeof(string)));
            
            // Assert
            parameterizedConstructor.Should().NotBeNull();
            parameterizedConstructor.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            var expectedMethods = new[] { "writeMessage", "DisposeAsync", "Dispose" };
            
            // Act
            var methods = type.GetMethods().Where(m => m.DeclaringType == type);
            var methodNames = methods.Select(m => m.Name).ToList();
            
            // Assert
            foreach (var expectedMethod in expectedMethods)
            {
                methodNames.Should().Contain(expectedMethod);
            }
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldImplementDisposablePattern()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var interfaces = type.GetInterfaces();
            var disposableInterface = interfaces.FirstOrDefault(i => i == typeof(IDisposable));
            
            // Assert
            disposableInterface.Should().NotBeNull();
        }
    }
}