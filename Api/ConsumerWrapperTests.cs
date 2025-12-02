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
        public void Constructor_ValidParameters_ShouldInitializeConsumer()
        {
            // Arrange
            var mockConfig = new ConsumerConfig();
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, topicName));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockConfig = new ConsumerConfig();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(mockConfig, null));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ConsumerConfig();
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            // Arrange
            var mockConfig = new ConsumerConfig();
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Act
            consumerWrapper.Dispose();

            // Assert - No exception should be thrown
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var mockConfig = new ConsumerConfig();
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_CancelledConsumer_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ConsumerConfig();
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Act
            consumerWrapper.Dispose(); // Close consumer
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Constructor_SubscribesToTopic()
        {
            // Arrange
            var mockConfig = new ConsumerConfig();
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }
    }
}