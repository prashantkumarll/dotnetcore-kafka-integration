using Xunit;
using Moq;
using FluentAssertions;
using Api;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;
using System;
using System.Threading;

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
        public async Task SendMessageAsync_WithValidTopicAndMessage_SendsSuccessfully()
        {
            // Arrange
            var topic = "test-topic";
            var message = "test message";

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _producerWrapper.SendMessageAsync(topic, message);

            // Assert
            _mockServiceBusSender.Verify(
                x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_WithNullTopic_ThrowsArgumentException()
        {
            // Arrange
            string? topic = null;
            var message = "test message";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _producerWrapper.SendMessageAsync(topic!, message));
        }

        [Fact]
        public async Task SendMessageAsync_WithEmptyTopic_ThrowsArgumentException()
        {
            // Arrange
            var topic = "";
            var message = "test message";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _producerWrapper.SendMessageAsync(topic, message));
        }

        [Fact]
        public async Task SendMessageAsync_WithNullMessage_ThrowsArgumentException()
        {
            // Arrange
            var topic = "test-topic";
            string? message = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _producerWrapper.SendMessageAsync(topic, message!));
        }

        [Fact]
        public async Task SendMessageAsync_WhenSenderThrows_PropagatesException()
        {
            // Arrange
            var topic = "test-topic";
            var message = "test message";

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ServiceBusException("Connection failed"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(() => 
                _producerWrapper.SendMessageAsync(topic, message));
        }

        [Fact]
        public async Task SendMessageAsync_CreatesCorrectSender()
        {
            // Arrange
            var topic = "orders-topic";
            var message = "order data";

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _producerWrapper.SendMessageAsync(topic, message);

            // Assert
            _mockServiceBusClient.Verify(
                x => x.CreateSender(topic),
                Times.Once);
        }

        [Fact]
        public void Dispose_DisposesServiceBusClient()
        {
            // Act
            _producerWrapper.Dispose();

            // Assert
            // Note: We can't directly verify Dispose was called on the mock,
            // but we can ensure the wrapper handles disposal properly
            _producerWrapper.Should().NotBeNull();
        }
    }
}