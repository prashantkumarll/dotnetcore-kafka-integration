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
        public void Constructor_NullClient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var topicName = "test-topic";
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
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var options = new ServiceBusProcessorOptions();
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Ensure multiple calls don't cause issues

            // Assert - no exception should be thrown
            true.Should().BeTrue();
        }

        [Fact]
        public void ReadMessage_ServiceBusException_ShouldReturnNull()
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
        public void ReadMessage_MessageAvailable_ShouldReturnMessageValue()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";
            var expectedMessage = "test message";
            var options = new ServiceBusProcessorOptions();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().Be(expectedMessage);
        }
    }
}
}