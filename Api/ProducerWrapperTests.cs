using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void Constructor_ValidConfig_ShouldInitializeProducer()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Assert
            producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, topicName));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(config, null));
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);
            var message = "test message";

            // Act & Assert
            await producerWrapper.writeMessage(message);
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null));
        }

        [Fact]
        public void Dispose_ShouldFlushAndDisposeProducer()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            producerWrapper.Dispose();

            // Assert - no exception means successful disposal
        }

        [Fact]
        public void MultipleDispose_ShouldNotThrowException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            producerWrapper.Dispose();
            producerWrapper.Dispose(); // Second dispose should be no-op

            // Assert - no exception means successful multiple disposal
        }

        [Fact]
        public async Task WriteMessage_LongMessage_ShouldSucceed()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);
            var longMessage = new string('x', 1000);

            // Act & Assert
            await producerWrapper.writeMessage(longMessage);
        }
    }
}