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
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(mockConfig, topicName))
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
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(mockConfig, null));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Returns((ConsumeResult<string, string>)null);

            // Act & Assert
            using (var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic"))
            {
                var result = consumerWrapper.readMessage();
                result.Should().BeNull();
            }
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrowException()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };

            // Act & Assert
            using (var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic"))
            {
                consumerWrapper.Dispose();
                consumerWrapper.Dispose(); // Second dispose should not throw
            }
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Throws(new OperationCanceledException());

            // Act & Assert
            using (var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic"))
            {
                var result = consumerWrapper.readMessage();
                result.Should().BeNull();
            }
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Throws(new ConsumeException(new ConsumeResult<string, string>()));

            // Act & Assert
            using (var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic"))
            {
                var result = consumerWrapper.readMessage();
                result.Should().BeNull();
            }
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };

            // Act & Assert
            using (var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic"))
            {
                consumerWrapper.Dispose();
            }
        }
    }
}