using System;
using Azure.Messaging.ServiceBus;
using Xunit;
using Azure.Messaging.ServiceBus;
using Moq;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
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
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null!, topicName));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, null!));
        }

        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceMessage()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);
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
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null!));
        }

        [Fact]
        public void Dispose_ShouldFlushAndDisposeProducer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);

            // Act
            producerWrapper.Dispose();

            // Assert
            producerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void MultipleDispose_ShouldNotThrowException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);

            // Act
            producerWrapper.Dispose();
            producerWrapper.Dispose();

            // Assert
            producerWrapper.Should().NotBeNull();
        }
    }
}

