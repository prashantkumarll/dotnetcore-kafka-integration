using Xunit;
using Moq;
using FluentAssertions;
using Api;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;
using System;

namespace Test
{
    public class ProducerWrapperTests
    {
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusSender> _mockServiceBusSender;
        private readonly ProducerWrapper _producerWrapper;

        public ProducerWrapperTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusSender = new Mock<ServiceBusSender>();
            
            _mockServiceBusClient
                .Setup(x => x.CreateSender(It.IsAny<string>()))
                .Returns(_mockServiceBusSender.Object);

            _producerWrapper = new ProducerWrapper(_mockServiceBusClient.Object);
        }

        [Fact]
        public async Task SendMessageAsync_WithValidTopicAndMessage_SendsMessage()
        {
            // Arrange
            var topic = "test-topic";
            var message = "test message";

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .Returns(Task.CompletedTask);

            // Act
            await _producerWrapper.SendMessageAsync(topic, message);

            // Assert
            _mockServiceBusClient.Verify(
                x => x.CreateSender(topic),
                Times.Once);
            
            _mockServiceBusSender.Verify(
                x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default),
                Times.Once);
        }

        [Theory]
        [InlineData(null, "message")]
        [InlineData("", "message")]
        [InlineData("topic", null)]
        [InlineData("topic", "")]
        public async Task SendMessageAsync_WithInvalidParameters_ThrowsArgumentException(
            string topic, string message)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _producerWrapper.SendMessageAsync(topic, message));
        }

        [Fact]
        public async Task SendMessageAsync_WhenSenderThrowsException_PropagatesException()
        {
            // Arrange
            var topic = "error-topic";
            var message = "error message";

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .ThrowsAsync(new ServiceBusException("Send failed"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(
                () => _producerWrapper.SendMessageAsync(topic, message));
        }

        [Fact]
        public async Task SendMessageAsync_WithLargeMessage_SendsSuccessfully()
        {
            // Arrange
            var topic = "large-message-topic";
            var largeMessage = new string('A', 10000);

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .Returns(Task.CompletedTask);

            // Act
            await _producerWrapper.SendMessageAsync(topic, largeMessage);

            // Assert
            _mockServiceBusSender.Verify(
                x => x.SendMessageAsync(
                    It.Is<ServiceBusMessage>(msg => msg.Body.ToString() == largeMessage),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_WithSpecialCharacters_SendsSuccessfully()
        {
            // Arrange
            var topic = "special-chars-topic";
            var messageWithSpecialChars = "Message with émojis 🚀 and spëcial chârs!";

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .Returns(Task.CompletedTask);

            // Act
            await _producerWrapper.SendMessageAsync(topic, messageWithSpecialChars);

            // Assert
            _mockServiceBusSender.Verify(
                x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default),
                Times.Once);
        }

        [Fact]
        public void Dispose_CallsDisposeOnServiceBusClient()
        {
            // Act
            _producerWrapper.Dispose();

            // Assert
            _mockServiceBusClient.Verify(x => x.DisposeAsync(), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_MultipleMessages_CreatesCorrectSenders()
        {
            // Arrange
            var topic1 = "topic1";
            var topic2 = "topic2";
            var message1 = "message1";
            var message2 = "message2";

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .Returns(Task.CompletedTask);

            // Act
            await _producerWrapper.SendMessageAsync(topic1, message1);
            await _producerWrapper.SendMessageAsync(topic2, message2);

            // Assert
            _mockServiceBusClient.Verify(x => x.CreateSender(topic1), Times.Once);
            _mockServiceBusClient.Verify(x => x.CreateSender(topic2), Times.Once);
            _mockServiceBusSender.Verify(
                x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default),
                Times.Exactly(2));
        }
    }
}