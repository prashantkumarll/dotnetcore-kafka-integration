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
        public void ProducerWrapper_Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = "test-topic";
            
            // Act
            var producer = new ProducerWrapper(connectionString, topicName);
            
            // Assert
            producer.Should().NotBeNull();
            producer.Should().BeOfType<ProducerWrapper>();
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
        
        [Theory]
        [InlineData("connection1", "topic1")]
        [InlineData("connection2", "topic2")]
        [InlineData("test", "queue")]
        public void ProducerWrapper_Constructor_WithDifferentParameters_ShouldCreateInstances(string connectionString, string topicName)
        {
            // Act
            var producer = new ProducerWrapper(connectionString, topicName);
            
            // Assert
            producer.Should().NotBeNull();
            producer.Should().BeOfType<ProducerWrapper>();
        }
        
        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);
            
            // Act & Assert
            typeof(IDisposable).IsAssignableFrom(type).Should().BeTrue();
        }
    }
}