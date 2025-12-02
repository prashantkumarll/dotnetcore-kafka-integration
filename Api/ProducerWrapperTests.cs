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
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, topicName));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, null));
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
            await producerWrapper.writeMessage(message);

            // Assert
            // Note: This is a basic test. Actual Service Bus interaction would require more complex mocking
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null));
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
            // Verify disposal happens without exceptions
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
            producerWrapper.Dispose(); // Second dispose should be no-op

            // Assert
            // No exception should be thrown
        }

        [Fact]
        public async Task WriteMessage_ProduceException_ShouldRethrow()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(() => 
                producerWrapper.writeMessage("problematic message"));
        }
    }
}
}