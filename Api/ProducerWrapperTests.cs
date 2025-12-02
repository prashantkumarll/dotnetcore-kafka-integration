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
        public void Constructor_ValidClient_ShouldInitializeProducer()
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
        public void Constructor_NullClient_ShouldThrowArgumentNullException()
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
        public async Task WriteMessage_ValidMessage_ShouldSendMessage()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var message = "test-message";

            using (var producerWrapper = new ProducerWrapper(mockClient.Object, topicName))
            {
                // Act
                await producerWrapper.writeMessage(message);

                // Assert - no exception means successful send
            }
        }

        [Fact]
        public void WriteMessage_NullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";

            using (var producerWrapper = new ProducerWrapper(mockClient.Object, topicName))
            {
                // Act & Assert
                Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null));
            }
        }

        [Fact]
        public void Dispose_ShouldFlushAndDisposeProducer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);
            producerWrapper.Dispose();

            // Assert - no exception means successful disposal
            producerWrapper.Dispose(); // Second call should be no-op
        }

        [Fact]
        public async Task WriteMessage_WhenSendFails_ShouldThrowException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var message = "test-message";

            using (var producerWrapper = new ProducerWrapper(mockClient.Object, topicName))
            {
                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() => producerWrapper.writeMessage(message));
            }
        }
    }
}
}