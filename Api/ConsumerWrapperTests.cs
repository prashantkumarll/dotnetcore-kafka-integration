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
        public void Constructor_ValidParameters_ShouldInitializeConsumer()
        {
            // Arrange
            var mockOptions = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(mockOptions, mockClient.Object, topicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            var topicName = "test-topic";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper((ServiceBusProcessorOptions)null, new Mock<ServiceBusClient>().Object, topicName));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockOptions = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(mockOptions, null, null));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockOptions = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockOptions, mockClient.Object, "test-topic");
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var mockOptions = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();
            var consumerWrapper = new ConsumerWrapper(mockOptions, mockClient.Object, "test-topic");

            // Act
            consumerWrapper.Dispose();

            // Assert - No exception should be thrown
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var mockOptions = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockOptions, mockClient.Object, "test-topic");
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var mockOptions = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();

            // Act
            var consumerWrapper = new ConsumerWrapper(mockOptions, mockClient.Object, "test-topic");
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var mockOptions = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();
            var consumerWrapper = new ConsumerWrapper(mockOptions, mockClient.Object, "test-topic");

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }
    }
}


