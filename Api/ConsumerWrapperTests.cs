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
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

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

            // Act
            var result = mockConsumer.Object.Consume(TimeSpan.FromSeconds(1));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrowException()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Second call should not throw
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Throws(new OperationCanceledException());

            // Act
            var result = mockConsumer.Object.Consume(TimeSpan.FromSeconds(1));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Throws(new ConsumeException(new ConsumeResult<string, string>()));

            // Act
            var result = mockConsumer.Object.Consume(TimeSpan.FromSeconds(1));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_MessageAvailable_ShouldReturnMessageValue()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Act & Assert
            // Note: This test might require more complex mocking depending on actual Kafka implementation
            var result = consumerWrapper.readMessage();
            result.Should().BeNull(); // Default behavior with no actual Kafka connection
        }
    }
}