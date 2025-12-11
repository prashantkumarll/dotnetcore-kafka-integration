// REQUIRED using statements - COPY THIS BLOCK TO EVERY TEST FILE:
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
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var implementsIDisposable = typeof(IDisposable).IsAssignableFrom(type);
            var implementsIAsyncDisposable = typeof(IAsyncDisposable).IsAssignableFrom(type);
            
            // Assert
            (implementsIDisposable || implementsIAsyncDisposable).Should().BeTrue("ProducerWrapper should implement disposal pattern");
        }

        [Fact]
        public void ProducerWrapper_Type_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var methods = type.GetMethods().Where(m => m.IsPublic && !m.IsSpecialName).Select(m => m.Name).ToList();
            
            // Assert
            methods.Should().Contain("writeMessage");
            methods.Should().Contain("DisposeAsync");
            methods.Should().Contain("Dispose");
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptStringParameters()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var constructors = type.GetConstructors();
            var expectedConstructor = constructors.FirstOrDefault(c => 
            {
                var parameters = c.GetParameters();
                return parameters.Length == 2 && 
                       parameters.All(p => p.ParameterType == typeof(string));
            });
            
            // Assert
            expectedConstructor.Should().NotBeNull("ProducerWrapper should have constructor with two string parameters");
        }
    }
}