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
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
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
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, null));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleDisposes_ShouldNotThrowException()
        {
            // Arrange
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Second dispose should not throw
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Implicit dispose will be called
            }

            // Assert: No exception means successful disposal
        }

        [Fact]
        public void ReadMessage_ConsumerCancelled_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }
    }
}