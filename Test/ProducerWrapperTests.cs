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
        public void ProducerWrapper_Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            var topicName = "test-topic";
            
            // Act
            var producer = new ProducerWrapper(config, topicName);
            
            // Assert
            producer.Should().NotBeNull();
            producer.Should().BeOfType<ProducerWrapper>();
        }
        
        [Fact]
        public void ProducerWrapper_Type_ShouldHaveExpectedAttributes()
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
            var method = type.GetMethod("writeMessage");
            
            // Assert
            method.Should().NotBeNull();
            method.Name.Should().Be("writeMessage");
        }
        
        [Fact]
        public void ProducerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act
            var method = type.GetMethod("Dispose");
            
            // Assert
            method.Should().NotBeNull();
            method.Name.Should().Be("Dispose");
        }
    }
}