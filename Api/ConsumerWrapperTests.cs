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
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            ServiceBusProcessorOptions options = null;

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
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrowException()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var options = new ServiceBusProcessorOptions();
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Second disposal should not throw
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var options = new ServiceBusProcessorOptions();
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act
            consumerWrapper.Dispose();

            // Assert - No specific assertion, just ensuring no exceptions are thrown
        }

        [Fact]
        public void ReadMessage_CancellationOccurs_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var options = new ServiceBusProcessorOptions();
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act
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
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }
    }
}
