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
        public void Constructor_ValidConfig_ShouldInitializeConsumer()
        {
            // Arrange
            var config = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Assert
            consumerWrapper.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, topicName));
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ServiceBusProcessorOptions { SessionId = "test-group" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, null));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var mockConsumer = new Mock<ServiceBusProcessor>();
            mockConsumer.Setup(c => c.ReceiveMessageAsync(It.IsAny<TimeSpan>())).Returns((ServiceBusReceivedMessagestring, string>)null);

            var config = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Dispose_MultipleInvocations_ShouldNotThrow()
        {
            // Arrange
            var config = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act & Assert
            wrapper.Dispose();
            wrapper.Dispose(); // Second call should not throw
        }

        [Fact]
        public void ReadMessage_OperationCancelled_ShouldReturnNull()
        {
            // Arrange
            var mockConsumer = new Mock<ServiceBusProcessor>();
            mockConsumer.Setup(c => c.ReceiveMessageAsync(It.IsAny<TimeSpan>())).Throws(new OperationCanceledException());

            var config = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_ServiceBusException_ShouldReturnNull()
        {
            // Arrange
            var mockConsumer = new Mock<ServiceBusProcessor>();
            mockConsumer.Setup(c => c.ReceiveMessageAsync(It.IsAny<TimeSpan>())).Throws(new ServiceBusException(new ServiceBusReceivedMessagestring, string>()));

            var config = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ReadMessage_MessageAvailable_ShouldReturnMessageValue()
        {
            // Arrange
            var expectedMessage = "test-message";
            var mockConsumer = new Mock<ServiceBusProcessor>();
            var consumeResult = new ServiceBusReceivedMessagestring, string> { Message = new ServiceBusMessage { Value = expectedMessage } };
            mockConsumer.Setup(c => c.ReceiveMessageAsync(It.IsAny<TimeSpan>())).Returns(consumeResult);

            var config = new ServiceBusProcessorOptions { SessionId = "test-group" };
            var wrapper = new ConsumerWrapper(config, "test-topic");

            // Act
            var result = wrapper.readMessage();

            // Assert
            result.Should().Be(expectedMessage);
        }
    }
}