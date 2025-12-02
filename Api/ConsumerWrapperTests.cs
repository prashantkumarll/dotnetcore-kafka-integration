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
            // No messages are set up on the mock client

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrowException()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Second dispose should not throw
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var mockClient = new Mock<ServiceBusClient>();
            // Simulate operation cancelled when creating processor
            mockClient.Setup(c => c.CreateProcessor(It.IsAny<string>(), It.IsAny<ServiceBusProcessorOptions>())).Throws(new OperationCanceledException());

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // This Kafka-specific test is no longer applicable for Service Bus and has been removed.
        }

        [Fact]
        public void ReadMessage_MessageAvailable_ShouldReturnMessageValue()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var expectedMessage = "test-message";
            var mockClient = new Mock<ServiceBusClient>();
            // Setup mock to return a processor that will provide the message when read
            var mockProcessor = new Mock<ServiceBusProcessor>();
            // Here we assume ConsumerWrapper uses processor to receive messages synchronously via an internal mechanism; setup as needed.
            // No explicit setup to keep test focused on public API

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);
            var result = await consumerWrapper.readMessage();

            // Assert
            result.Should().Be(expectedMessage);
        }
    }
}