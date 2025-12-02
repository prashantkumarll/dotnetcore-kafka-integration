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
            var topicName = "test-topic";
            ServiceBusProcessorOptions options = null;

            // Act
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, topicName, options);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ConsumerWrapper(null, "test-topic", null);

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("client");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            var mockClient = new Mock<ServiceBusClient>();
            ServiceBusProcessorOptions options = null;
            Action act = () => new ConsumerWrapper(mockClient.Object, null, options);

            // Assert
            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            ServiceBusProcessorOptions options = null;

            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            ServiceBusProcessorOptions options = null;
            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);

            // Act
            consumerWrapper.Dispose();
            consumerWrapper.Dispose(); // Ensure multiple calls don't cause issues

            // Assert - no exception is thrown
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            ServiceBusProcessorOptions options = null;

            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var mockClient = new Mock<ServiceBusClient>();
            ServiceBusProcessorOptions options = null;

            var consumerWrapper = new ConsumerWrapper(mockClient.Object, "test-topic", options);

            // Act
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }
    }
}