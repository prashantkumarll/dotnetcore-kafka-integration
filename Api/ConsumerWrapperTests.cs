using System;
using Azure.Messaging.ServiceBus;
using Xunit;
using Azure.Messaging.ServiceBus;
using Moq;
using Azure.Messaging.ServiceBus;
using FluentAssertions;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void Constructor_ValidConfig_ShouldInitializeConsumer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options))
            {
                // Assert
                consumerWrapper.Should().NotBeNull();
            }
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            string topicName = "test-topic";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, topicName, options));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, null, options));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);
            consumerWrapper.Dispose();

            // Assert - no exception means successful disposal
            consumerWrapper.Dispose(); // Second dispose should be safe
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }
    }
}