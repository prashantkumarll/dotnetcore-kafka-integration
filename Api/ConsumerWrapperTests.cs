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
            var mockConfig = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            var topicName = "test-topic";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, topicName));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockConfig = new ServiceBusProcessorOptions { SessionId = "test-group" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(mockConfig, null));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var mockConsumer = new Mock<ServiceBusProcessor>();
            mockConsumer.Setup(c => c.ReceiveMessageAsync(It.IsAny<TimeSpan>())).Returns((ServiceBusReceivedMessagestring, string>)null);

            // Act
            var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic");
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldCloseAndDisposeConsumer()
        {
            // Arrange
            var mockConfig = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic");

            // Act
            consumerWrapper.Dispose();

            // Assert - No exception should be thrown
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var mockConsumer = new Mock<ServiceBusProcessor>();
            mockConsumer.Setup(c => c.ReceiveMessageAsync(It.IsAny<TimeSpan>())).Throws(new OperationCanceledException());

            // Act
            var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic");
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ServiceBusException_ShouldReturnNull()
        {
            // Arrange
            var mockConfig = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var mockConsumer = new Mock<ServiceBusProcessor>();
            mockConsumer.Setup(c => c.ReceiveMessageAsync(It.IsAny<TimeSpan>())).Throws(new Exception("Consume error"));

            // Act
            var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic");
            var result = consumerWrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var mockConfig = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var consumerWrapper = new ConsumerWrapper(mockConfig, "test-topic");

            // Act & Assert
            consumerWrapper.Dispose();
            consumerWrapper.Invoking(x => x.Dispose()).Should().NotThrow();
        }
    }
}