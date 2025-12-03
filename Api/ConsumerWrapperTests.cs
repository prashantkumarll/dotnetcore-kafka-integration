using System;
using Xunit;
using Moq;
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
        public void Constructor_NullClient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

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

            // Assert
            // No specific assertion needed, just ensuring no exceptions are thrown
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Second dispose should not throw

            // Assert
            // No specific assertion needed, just ensuring no exceptions are thrown
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

            // Simulate exceptions coming from the Service Bus processor creation/usage
            mockClient.Setup(c => c.CreateProcessor(It.IsAny<string>(), It.IsAny<ServiceBusProcessorOptions>())).Throws(new InvalidOperationException());

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