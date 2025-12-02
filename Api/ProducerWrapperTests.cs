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
            var message = "test-message";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                await producerWrapper.writeMessage(message);

                // Assert - no exception means successful produce
            }
        }

        [Fact]
        public void WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act & Assert
                Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null));
            }
        }

        [Fact]
        public void Dispose_ShouldFlushAndDisposeProducer()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);
            producerWrapper.Dispose();

            // Assert - no exception means successful disposal
        }

        [Fact]
        public void MultipleDispose_ShouldNotThrowException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(config, topicName);
            producerWrapper.Dispose();
            producerWrapper.Dispose(); // Second dispose should not throw

            // Assert - no exception means successful multiple disposal
        }

        [Fact]
        public async Task WriteMessage_LongMessage_ShouldProduce()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var longMessage = new string('x', 1000);

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                await producerWrapper.writeMessage(longMessage);

                // Assert - no exception means successful produce
            }
        }
    }
}