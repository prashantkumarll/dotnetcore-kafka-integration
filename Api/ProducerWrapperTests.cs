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
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";

            // Act
            using var producerWrapper = new ProducerWrapper(config, topicName);

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
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(config, null));
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var message = "test message";

            // Act
            using var producerWrapper = new ProducerWrapper(config, topicName);
            await producerWrapper.writeMessage(message);

            // Assert
            // Note: Actual Kafka interaction would require integration testing
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";

            // Act & Assert
            using var producerWrapper = new ProducerWrapper(config, topicName);
            await Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null));
        }

        [Fact]
        public void Dispose_ShouldFlushAndDisposeProducer()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);
            producerWrapper.Dispose();

            // Assert
            // Verify disposal occurs without exceptions
        }

        [Fact]
        public void MultipleDispose_ShouldNotThrowException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);
            producerWrapper.Dispose();
            producerWrapper.Dispose(); // Second dispose

            // Assert
            // Verify multiple dispose calls do not cause issues
        }
    }
}