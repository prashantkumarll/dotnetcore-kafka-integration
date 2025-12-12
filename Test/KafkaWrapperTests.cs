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
using Confluent.Kafka;

namespace Test
{
    public class KafkaWrapperTests
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
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var implementsDisposable = type.GetInterfaces().Contains(typeof(IDisposable));
            
            // Assert
            implementsDisposable.Should().BeTrue();
        }
        
        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var implementsDisposable = type.GetInterfaces().Contains(typeof(IDisposable));
            
            // Assert
            implementsDisposable.Should().BeTrue();
        }
        
        [Fact]
        public void ConsumerWrapper_ShouldHaveReadMessageMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("readMessage");
            
            // Assert
            method.Should().NotBeNull();
            method.Name.Should().Be("readMessage");
        }
        
        [Fact]
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var method = type.GetMethod("writeMessage");
            
            // Assert
            method.Should().NotBeNull();
            method.Name.Should().Be("writeMessage");
        }
    }
}