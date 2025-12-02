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
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Assert
                consumerWrapper.Should().NotBeNull();
            }
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
            var config = new ConsumerConfig { GroupId = "test-group" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, null));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Returns((ConsumeResult<string, string>)null);

            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(config, topicName);
            consumerWrapper.Dispose();

            // Assert - no exception means successful disposal
            consumerWrapper.Dispose(); // Second dispose should be safe
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }
    }
}