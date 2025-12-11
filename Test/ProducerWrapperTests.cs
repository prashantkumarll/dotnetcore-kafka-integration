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
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var writeMessageMethod = type.GetMethod("writeMessage");
            var disposeMethod = type.GetMethod("Dispose");
            
            // Assert
            writeMessageMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var implementsIDisposable = typeof(IDisposable).IsAssignableFrom(type);
            
            // Assert
            implementsIDisposable.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_Constructor_ShouldHaveExpectedParameters()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var constructors = type.GetConstructors();
            var parameterNames = constructors.FirstOrDefault()?.GetParameters().Select(p => p.Name).ToArray();
            
            // Assert
            constructors.Should().NotBeEmpty();
            parameterNames.Should().Contain("config");
            parameterNames.Should().Contain("topicName");
        }

        [Fact]
        public void ProducerWrapper_Methods_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var writeMessageMethod = type.GetMethod("writeMessage");
            var disposeMethod = type.GetMethod("Dispose");
            
            // Assert
            writeMessageMethod?.IsPublic.Should().BeTrue();
            disposeMethod?.IsPublic.Should().BeTrue();
        }
    }
}