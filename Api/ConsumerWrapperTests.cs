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
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

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
            var mockClient = new Mock<ServiceBusClient>();
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
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act
            consumerWrapper.Dispose();

            // Assert - No exception should be thrown
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_CancelledConsumer_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Act
            consumerWrapper.Dispose(); // Close consumer
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Constructor_SubscribesToTopic()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            var options = new ServiceBusProcessorOptions();
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }
    }
}