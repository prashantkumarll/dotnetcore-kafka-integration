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
            var config = new ProducerConfig { BootstrapServers = "test-server" };
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

            // Assert
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
            method.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProducerWrapper_ShouldImplementIDisposable()
        {
            // Arrange
            var type = typeof(ProducerWrapper);

            // Act
            var isDisposable = typeof(IDisposable).IsAssignableFrom(type);

            // Assert
            isDisposable.Should().BeTrue();
        }
    }
}