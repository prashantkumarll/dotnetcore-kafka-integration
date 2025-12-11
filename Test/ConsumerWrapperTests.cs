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
            type.IsClass.Should().BeTrue();
            type.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldHaveExpectedMethods()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var readMessageMethod = type.GetMethod("readMessage");
            var disposeMethod = type.GetMethod("Dispose");
            
            // Assert
            readMessageMethod.Should().NotBeNull();
            disposeMethod.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var implementsIDisposable = typeof(IDisposable).IsAssignableFrom(type);
            
            // Assert
            implementsIDisposable.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_ShouldHaveExpectedParameters()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var constructors = type.GetConstructors();
            var constructor = constructors.FirstOrDefault();
            var parameterNames = constructor?.GetParameters().Select(p => p.Name).ToArray();
            var parameterCount = constructor?.GetParameters().Length;
            
            // Assert
            constructors.Should().NotBeEmpty();
            parameterCount.Should().Be(2);
            parameterNames.Should().Contain("config");
            parameterNames.Should().Contain("topicName");
        }

        [Fact]
        public void ConsumerWrapper_Methods_ShouldBePublic()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var readMessageMethod = type.GetMethod("readMessage");
            var disposeMethod = type.GetMethod("Dispose");
            
            // Assert
            readMessageMethod?.IsPublic.Should().BeTrue();
            disposeMethod?.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_And_ProducerWrapper_ShouldBeDifferentTypes()
        {
            // Arrange
            var consumerType = typeof(ConsumerWrapper);
            var producerType = typeof(ProducerWrapper);
            
            // Act & Assert
            consumerType.Should().NotBe(producerType);
            consumerType.Name.Should().Be("ConsumerWrapper");
            producerType.Name.Should().Be("ProducerWrapper");
        }
    }
}