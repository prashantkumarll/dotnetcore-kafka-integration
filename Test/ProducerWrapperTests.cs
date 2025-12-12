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
    public class ProducerWrapperTests
    {
        [Fact]
        public void ProducerWrapper_Constructor_ShouldCreateInstance()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            var topicName = "test-topic";
            
            // Act
            var wrapper = new ProducerWrapper(config, topicName);
            
            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Should().BeOfType<ProducerWrapper>();
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
        public void ProducerWrapper_ShouldHaveWriteMessageMethod()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var writeMessageMethod = type.GetMethods().FirstOrDefault(m => m.Name == "writeMessage");
            
            // Assert
            writeMessageMethod.Should().NotBeNull();
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
    }
}