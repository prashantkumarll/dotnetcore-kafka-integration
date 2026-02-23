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
        public void ConsumerWrapper_Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            
            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);
            
            // Assert
            consumer.Should().NotBeNull();
            consumer.Should().BeOfType<ConsumerWrapper>();
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
        
        [Theory]
        [InlineData("connection1", "topic1")]
        [InlineData("connection2", "topic2")]
        [InlineData("test", "queue")]
        public void ConsumerWrapper_Constructor_WithDifferentParameters_ShouldCreateInstances(string connectionString, string topicName)
        {
            // Act
            var consumer = new ConsumerWrapper(connectionString, topicName);
            
            // Assert
            consumer.Should().NotBeNull();
            consumer.Should().BeOfType<ConsumerWrapper>();
        }
        
        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);
            
            // Act & Assert
            typeof(IDisposable).IsAssignableFrom(type).Should().BeTrue();
        }
    }
}