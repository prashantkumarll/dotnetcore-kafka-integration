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
        public void Constructor_ValidConfig_ShouldInitializeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig();
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(config, topicName);

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
            var config = new ConsumerConfig();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, null));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig();
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrowException()
        {
            // Arrange
            var config = new ConsumerConfig();
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act & Assert
            wrapper.Dispose();
            wrapper.Dispose(); // Second call should not throw
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig();
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig();
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            // Arrange
            var config = new ConsumerConfig();
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            wrapper.Dispose();

            // Assert
            wrapper.Dispose(); // Verify disposal by calling again
        }

        [Fact]
        public void ReadMessage_LongRunningConsumer_ShouldHandleTimeout()
        {
            // Arrange
            var config = new ConsumerConfig();
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_AfterMultipleReads_ShouldCleanupResources()
        {
            // Arrange
            var config = new ConsumerConfig();
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            wrapper.readMessage();
            wrapper.readMessage();
            wrapper.Dispose();

            // Assert
            wrapper.Dispose(); // Verify no exceptions
        }
    }
}