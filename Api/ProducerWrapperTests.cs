using System;
using Xunit;
using Moq;
using FluentAssertions;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProducerWrapperTests
    {
        [Fact]
        public void Constructor_ValidConfig_ShouldInitializeProducer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);

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
                .Should().Throw<ArgumentNullException>().WithParameterName("client");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            FluentActions.Invoking(() => new ProducerWrapper(mockClient.Object, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("topicName");
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);
            var message = "test message";

            // Act & Assert
            await FluentActions.Invoking(async () => await producerWrapper.writeMessage(message))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);

            // Act & Assert
            await FluentActions.Invoking(async () => await producerWrapper.writeMessage(null))
                .Should().ThrowAsync<ArgumentNullException>().WithParameterName("message");
        }

        [Fact]
        public void Dispose_ShouldDisposeProducer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);

            // Act
            producerWrapper.Dispose();

            // Assert - No specific assertion, just ensuring no exceptions are thrown
            producerWrapper.Invoking(p => p.Dispose()).Should().NotThrow();
        }
    }
}
}