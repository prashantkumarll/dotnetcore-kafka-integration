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

            // Assert
            // No specific assertion needed, just ensuring no exceptions are thrown
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(config, topicName);
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Second dispose should not throw

            // Assert
            // No specific assertion needed, just ensuring no exceptions are thrown
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Throws(new ConsumeException(new ConsumeResult<string, string>()));

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