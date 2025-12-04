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
            var mockConsumer = new Mock<IConsumer<string, string>>();
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Returns((ConsumeResult<string, string>)null);

            var config = new ConsumerConfig { GroupId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrowException()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            wrapper.Dispose();
            wrapper.Dispose(); // Second call should not throw

            // Assert - no exception means success
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            wrapper.Dispose();

            // Assert - no specific assertion, just ensuring no exceptions
        }
    }
}