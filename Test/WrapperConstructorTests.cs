using Xunit;
using FluentAssertions;
using Api;
using System;

namespace Test
{
    public class WrapperConstructorTests
    {
        [Fact]
        public void ProducerWrapper_Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var connectionString = "Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=test;SharedAccessKey=testkey";
            var topicName = "test-topic";

            // Act
            var wrapper = new ProducerWrapper(connectionString, topicName);

            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Should().BeOfType<ProducerWrapper>();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange
            var connectionString = "Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=test;SharedAccessKey=testkey";
            var topicName = "test-topic";

            // Act
            var wrapper = new ConsumerWrapper(connectionString, topicName);

            // Assert
            wrapper.Should().NotBeNull();
            wrapper.Should().BeOfType<ConsumerWrapper>();
        }

        [Fact]
        public void ProducerWrapper_Constructor_WithEmptyConnectionString_ShouldStillCreateInstance()
        {
            // Arrange
            var connectionString = string.Empty;
            var topicName = "test-topic";

            // Act
            var wrapper = new ProducerWrapper(connectionString, topicName);

            // Assert
            wrapper.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_WithEmptyTopicName_ShouldStillCreateInstance()
        {
            // Arrange
            var connectionString = "test-connection";
            var topicName = string.Empty;

            // Act
            var wrapper = new ConsumerWrapper(connectionString, topicName);

            // Assert
            wrapper.Should().NotBeNull();
        }

        [Fact]
        public void ProducerWrapper_Constructor_WithNullParameters_ShouldCreateInstance()
        {
            // Arrange & Act
            var wrapper = new ProducerWrapper(null, null);

            // Assert
            wrapper.Should().NotBeNull();
        }

        [Fact]
        public void ConsumerWrapper_Constructor_WithNullParameters_ShouldCreateInstance()
        {
            // Arrange & Act
            var wrapper = new ConsumerWrapper(null, null);

            // Assert
            wrapper.Should().NotBeNull();
        }
    }
}