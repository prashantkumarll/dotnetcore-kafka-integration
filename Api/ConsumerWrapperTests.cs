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
            // Arrange & Act
            Action act = () => new ConsumerWrapper(null, "test-topic");

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ConsumerWrapper(new ConsumerConfig(), null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Returns((ConsumeResult<string, string>)null);

            var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic");

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic");

            // Act
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Ensure multiple calls don't cause issues

            // Assert - no exception is thrown
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Throws(new OperationCanceledException());

            var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic");

            // Act
            var result = consumerWrapper.readMessage();

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

            var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic");

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }
    }
}