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
        public void ConsumerWrapper_Constructor_ShouldCreateInstance()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };
            var topicName = "test-topic";
            
            // Act
            var wrapper = new ConsumerWrapper(config, topicName);
            
            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Should().BeOfType<ConsumerWrapper>();
        }

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
        public void ConsumerWrapper_ShouldHaveReadMessageMethod()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act
            var readMessageMethod = type.GetMethods().FirstOrDefault(m => m.Name == "readMessage");
            
            // Assert
            readMessageMethod.Should().NotBeNull();
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
    }
}