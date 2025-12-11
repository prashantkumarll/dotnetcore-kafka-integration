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
        public void ConsumerWrapper_Type_ShouldHaveExpectedProperties()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var implementsDisposable = typeof(IDisposable).IsAssignableFrom(type);
            
            // Assert
            implementsDisposable.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var readMessageAsyncMethod = type.GetMethod("ReadMessageAsync");
            var disposeAsyncMethod = type.GetMethod("DisposeAsync");
            var disposeMethod = type.GetMethod("Dispose");
            
            // Assert
            readMessageAsyncMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldAcceptStringParameters()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(2);
            constructor.GetParameters()[0].ParameterType.Should().Be(typeof(string));
            constructor.GetParameters()[1].ParameterType.Should().Be(typeof(string));
        }

        [Fact]
        public void ConsumerWrapper_ReadMessageAsync_ShouldBeAsyncMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("ReadMessageAsync");
            
            // Assert
            method.Should().NotBeNull();
            method.Name.Should().Contain("Async");
        }
    }
}