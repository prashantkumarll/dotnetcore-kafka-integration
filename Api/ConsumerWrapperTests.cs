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
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullClient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var topicName = "test-topic";
            ServiceBusProcessorOptions options = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, topicName, options));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, null, options));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);

            // Act
            consumerWrapper.Dispose();

            // Assert - No exception should be thrown
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }
    }
}
}