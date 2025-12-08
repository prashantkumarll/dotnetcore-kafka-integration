using Xunit;
using Moq;
using FluentAssertions;
using Api;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;
using System;

namespace Test.Messaging
{
    public class ProducerWrapperTests
    {
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusSender> _mockServiceBusSender;
        private readonly Mock<ILogger<ProducerWrapper>> _mockLogger;
        private readonly ProducerWrapper _producerWrapper;

        public ProducerWrapperTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusSender = new Mock<ServiceBusSender>();
            _mockLogger = new Mock<ILogger<ProducerWrapper>>();

            _mockServiceBusClient
                .Setup(x => x.CreateSender(It.IsAny<string>()))
                .Returns(_mockServiceBusSender.Object);

            _producerWrapper = new ProducerWrapper(_mockServiceBusClient.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ProduceAsync_WithValidTopicAndMessage_SendsMessage()
        {
            // Arrange
            var topic = "order-events";
            var message = "{\"orderId\": \"12345\", \"status\": \"created\"}";

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .Returns(Task.CompletedTask);

            // Act
            await _producerWrapper.ProduceAsync(topic, message);

            // Assert
            _mockServiceBusSender.Verify(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default), Times.Once);
        }

        [Fact]
        public async Task ProduceAsync_WithNullTopic_ThrowsArgumentNullException()
        {
            // Arrange
            string? topic = null;
            var message = "test message";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _producerWrapper.ProduceAsync(topic!, message));
        }

        [Fact]
        public async Task ProduceAsync_WithEmptyTopic_ThrowsArgumentException()
        {
            // Arrange
            var topic = "";
            var message = "test message";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _producerWrapper.ProduceAsync(topic, message));
        }

        [Fact]
        public async Task ProduceAsync_WithNullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            var topic = "test-topic";
            string? message = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _producerWrapper.ProduceAsync(topic, message!));
        }

        [Fact]
        public async Task ProduceAsync_ServiceBusThrowsException_PropagatesException()
        {
            // Arrange
            var topic = "test-topic";
            var message = "test message";
            var expectedException = new ServiceBusException("Service unavailable");

            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<ServiceBusException>(
                () => _producerWrapper.ProduceAsync(topic, message));
            
            actualException.Should().Be(expectedException);
        }

        [Theory]
        [InlineData("orders", "simple message")]
        [InlineData("events", "{\"complex\": {\"json\": \"message\", \"with\": [1, 2, 3]}}")]
        [InlineData("notifications", "Message with special chars: !@#$%^&*()")]
        public async Task ProduceAsync_WithVariousMessageFormats_SendsSuccessfully(string topic, string message)
        {
            // Arrange
            _mockServiceBusSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
                .Returns(Task.CompletedTask);

            // Act
            await _producerWrapper.ProduceAsync(topic, message);

            // Assert
            _mockServiceBusSender.Verify(x => x.SendMessageAsync(
                It.Is<ServiceBusMessage>(m => m.Body.ToString() == message), 
                default), Times.Once);
        }

        [Fact]
        public void Dispose_CallsDispose()
        {
            // Act
            _producerWrapper.Dispose();

            // Assert - No exception should be thrown
            // Verify that ServiceBusClient dispose is called
            _mockServiceBusClient.Verify(x => x.DisposeAsync(), Times.Once);
        }
    }
}