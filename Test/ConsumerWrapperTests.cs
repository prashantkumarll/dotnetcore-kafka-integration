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
                BootstrapServers = "test-server",
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

            // Assert
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
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ConsumerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ConsumerWrapper);

            // Act
            var isDisposable = typeof(IDisposable).IsAssignableFrom(type);

            // Assert
            isDisposable.Should().BeTrue();
        }
    }
}