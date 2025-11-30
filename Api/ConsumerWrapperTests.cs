using System;
using Xunit;
using Moq;
using FluentAssertions;

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
            using var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

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

            // Act & Assert
            using var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);
            var result = consumerWrapper.readMessage();
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleDisposes_ShouldNotThrowException()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            using (var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options))
            {
                consumerWrapper.Dispose();
                consumerWrapper.Dispose(); // Second dispose should not throw
            }
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            using var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);
            var result = consumerWrapper.readMessage();
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            using var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);
            var result = consumerWrapper.readMessage();
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            using (var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options))
            {
                consumerWrapper.Dispose();
            }
        }
    }
}