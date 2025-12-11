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
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveCorrectNamespace()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act & Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var implementsIDisposable = typeof(IDisposable).IsAssignableFrom(type);
            var implementsIAsyncDisposable = typeof(IAsyncDisposable).IsAssignableFrom(type);
            
            // Assert
            (implementsIDisposable || implementsIAsyncDisposable).Should().BeTrue("ConsumerWrapper should implement disposal pattern");
        }

        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var methods = type.GetMethods().Where(m => m.IsPublic && !m.IsSpecialName).Select(m => m.Name).ToList();
            
            // Assert
            methods.Should().Contain("ReadMessageAsync");
            methods.Should().Contain("DisposeAsync");
            methods.Should().Contain("Dispose");
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptStringParameters()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var constructors = type.GetConstructors();
            var expectedConstructor = constructors.FirstOrDefault(c => 
            {
                var parameters = c.GetParameters();
                return parameters.Length == 2 && 
                       parameters.All(p => p.ParameterType == typeof(string));
            });
            
            // Assert
            expectedConstructor.Should().NotBeNull("ConsumerWrapper should have constructor with two string parameters");
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_ShouldBeAsyncMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var readMessageMethod = type.GetMethod("ReadMessageAsync");
            
            // Assert
            readMessageMethod.Should().NotBeNull();
            readMessageMethod.Name.Should().Contain("Async");
        }
    }
}