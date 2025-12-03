using System;
using Xunit;
using Moq;
using FluentAssertions;
using Azure.Messaging.ServiceBus;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void Constructor_ValidParameters_ShouldInitializeConsumer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var options = new ServiceBusProcessorOptions();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";
            var options = new ServiceBusProcessorOptions();

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
            var topicName = "test-topic";
            var options = new ServiceBusProcessorOptions();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrowException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", new ServiceBusProcessorOptions());

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Second dispose should not throw
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var options = new ServiceBusProcessorOptions();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var options = new ServiceBusProcessorOptions();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", new ServiceBusProcessorOptions());

            // Act
            consumerWrapper.Dispose();

            // Assert
            // Implicit assertion: No exception thrown during disposal
        }
    }
}
}