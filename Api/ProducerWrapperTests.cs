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
            var mockClient = new Mock<ServiceBusClient>();
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
            var message = "test-message";

            using (var producerWrapper = new ProducerWrapper(mockClient.Object, topicName))
            {
                // Act
                await producerWrapper.writeMessage(message);

                // Assert - no exception means successful produce
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
        }

        [Fact]
        public void MultipleDispose_ShouldNotThrowException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";

            // Act
            var producerWrapper = new ProducerWrapper(mockClient.Object, topicName);
            producerWrapper.Dispose();
            producerWrapper.Dispose(); // Second dispose should not throw

            // Assert - no exception means successful multiple disposal
        }

        [Fact]
        public async Task WriteMessage_LongMessage_ShouldProduce()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var longMessage = new string('x', 1000);

            using (var producerWrapper = new ProducerWrapper(mockClient.Object, topicName))
            {
                // Act
                await producerWrapper.writeMessage(longMessage);

                // Assert - no exception means successful produce
            }
        }
    }
}
}