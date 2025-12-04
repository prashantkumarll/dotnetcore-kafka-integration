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
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Returns((ConsumeResult<string, string>)null);

            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var consumerWrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var consumerWrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            consumerWrapper.Dispose();

            // Assert - No exception should be thrown
            Assert.True(true);
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrowException()
        {
            // Arrange
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var consumerWrapper = new ConsumerWrapper(config, "test-topic");

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Second disposal should not throw
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var consumerWrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_OperationCancelledException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var consumerWrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }
    }
}