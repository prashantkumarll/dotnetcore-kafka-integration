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
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            FluentActions.Invoking(() => new ProducerWrapper(null, topicName))
                .Should().Throw<ArgumentNullException>().WithParameterName("config");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();

            // Act & Assert
            FluentActions.Invoking(() => new ProducerWrapper(config, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("topicName");
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);
            var message = "test message";

            // Act & Assert
            await FluentActions.Invoking(async () => await producerWrapper.writeMessage(message))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act & Assert
            await FluentActions.Invoking(async () => await producerWrapper.writeMessage(null))
                .Should().ThrowAsync<ArgumentNullException>().WithParameterName("message");
        }

        [Fact]
        public void Dispose_ShouldDisposeProducer()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            producerWrapper.Dispose();

            // Assert - no specific assertion, just ensuring no exceptions
            producerWrapper.Invoking(p => p.Dispose()).Should().NotThrow();
        }
    }
}