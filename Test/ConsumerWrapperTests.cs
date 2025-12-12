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
    public class ConsumerWrapperTests
    {
        [Fact]
        public void ConsumerWrapper_Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            var topicName = "test-topic";
            
            // Act
            var consumer = new ConsumerWrapper(config, topicName);
            
            // Assert
            consumer.Should().NotBeNull();
            consumer.Should().BeOfType<ConsumerWrapper>();
        }
        
        [Fact]
        public void ConsumerWrapper_Type_ShouldHaveExpectedAttributes()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act & Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("ConsumerWrapper");
            type.Namespace.Should().Be("Api");
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
        public void ConsumerWrapper_ShouldHaveDisposeMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var method = type.GetMethod("Dispose");
            
            // Assert
            method.Should().NotBeNull();
            method.Name.Should().Be("Dispose");
        }
    }
}