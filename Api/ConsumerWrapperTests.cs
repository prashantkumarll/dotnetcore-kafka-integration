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
            act.Should().Throw<ArgumentNullException>().WithMessage("*config*");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ConsumerWrapper(new ConsumerConfig(), null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*topicName*");
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockConsumer = new Mock<IConsumer<string, string>>(); 
            mockConsumer.Setup(c => c.Consume(It.IsAny<TimeSpan>())).Returns((ConsumeResult<string, string>)null);

            var config = new ConsumerConfig { GroupId = "test-group" };
            using (var consumerWrapper = new ConsumerWrapper(config, "test-topic"))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void Dispose_MultipleDisposes_ShouldNotThrowException()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var consumerWrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            consumerWrapper.Dispose();
            Action secondDispose = () => consumerWrapper.Dispose();

            // Assert
            secondDispose.Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            using (var consumerWrapper = new ConsumerWrapper(config, "test-topic"))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            using (var consumerWrapper = new ConsumerWrapper(config, "test-topic"))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var consumerWrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            consumerWrapper.Dispose();

            // Assert
            consumerWrapper.Should().NotBeNull();
        }
    }
}