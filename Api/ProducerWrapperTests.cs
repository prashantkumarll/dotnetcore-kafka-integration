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
            // Arrange & Act
            Action act = () => new ProducerWrapper(null, "topic");

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ProducerWrapper(new ProducerConfig(), null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);
            var message = "test message";

            // Act
            Func<Task> act = async () => await producerWrapper.writeMessage(message);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            Func<Task> act = async () => await producerWrapper.writeMessage(null);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public void Dispose_ShouldFlushAndDisposeProducer()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            producerWrapper.Dispose();

            // Assert
            producerWrapper.Dispose(); // Second call should not throw
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            Action act = () => {
                producerWrapper.Dispose();
                producerWrapper.Dispose();
            };

            // Assert
            act.Should().NotThrow();
        }
    }
}