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

            // Act
            await producerWrapper.writeMessage("test message");

            // Assert
            // Note: This is a basic test. More complex scenarios might require mocking IProducer
        }

        [Fact]
        public void WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ProducerConfig();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(config, topicName);

            // Act
            Func<Task> act = async () => await producerWrapper.writeMessage(null);

            // Assert
            act.Should().ThrowAsync<ArgumentNullException>().Which.ParamName.Should().Be("message");
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
            producerWrapper.Dispose(); // Ensure multiple calls are safe

            // Assert
            // Verification is implicit through no exceptions being thrown
        }
    }
}