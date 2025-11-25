using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void Constructor_InvalidTopicName_EmptyString_ShouldThrowArgumentException()
        {
            // Arrange
            var config = new ConsumerConfig();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ConsumerWrapper(config, string.Empty));
        }

        [Fact]
        public void Constructor_WhitespaceTopicName_ShouldThrowArgumentException()
        {
            // Arrange
            var config = new ConsumerConfig();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ConsumerWrapper(config, "   "));
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrowException()
        {
            // Arrange
            var config = new ConsumerConfig();
            var topicName = "test-topic";
            var wrapper = new ConsumerWrapper(config, topicName);

            // Act & Assert
            wrapper.Dispose();
            wrapper.Dispose(); // Second call should not throw
        }
    }
}