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
            var message = "test message";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act
                await producerWrapper.writeMessage(message);

                // Assert
                // Note: This is a basic test. Actual Kafka message verification would require more complex setup
            }
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null));
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

            // Assert
            // Verify that multiple dispose calls don't throw exceptions
            producerWrapper.Dispose();
        }

        [Fact]
        public void Dispose_MultipleCallsSafe_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";

            // Act
            using (var producerWrapper = new ProducerWrapper(config, topicName))
            {
                // Multiple dispose calls
                producerWrapper.Dispose();
                producerWrapper.Dispose();
            }

            // Assert - no exception should be thrown
        }
    }
}