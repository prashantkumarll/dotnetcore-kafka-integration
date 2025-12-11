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
        public void ProducerWrapper_Type_ShouldHaveExpectedProperties()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ProducerWrapper");
            type.Namespace.Should().Be("Api");
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var implementsDisposable = typeof(IDisposable).IsAssignableFrom(type);
            
            // Assert
            implementsDisposable.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var writeMessageMethod = type.GetMethod("writeMessage");
            var disposeAsyncMethod = type.GetMethod("DisposeAsync");
            var disposeMethod = type.GetMethod("Dispose");
            
            // Assert
            writeMessageMethod.Should().NotBeNull();
            disposeAsyncMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldAcceptStringParameters()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var constructor = type.GetConstructor(new[] { typeof(string), typeof(string) });
            
            // Assert
            constructor.Should().NotBeNull();
            constructor.GetParameters().Should().HaveCount(2);
            constructor.GetParameters()[0].ParameterType.Should().Be(typeof(string));
            constructor.GetParameters()[1].ParameterType.Should().Be(typeof(string));
        }
    }
}