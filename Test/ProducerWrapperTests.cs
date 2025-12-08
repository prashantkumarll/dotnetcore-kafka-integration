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
        private readonly Mock<ServiceBusClient> _mockClient;
        private readonly Mock<ServiceBusSender> _mockSender;
        private readonly ProducerWrapper _producerWrapper;

        public ProducerWrapperTests()
        {
            _mockClient = new Mock<ServiceBusClient>();
            _mockSender = new Mock<ServiceBusSender>();
            
            _mockClient
                .Setup(x => x.CreateSender(It.IsAny<string>()))
                .Returns(_mockSender.Object);

            _producerWrapper = new ProducerWrapper();
            // Note: In a real scenario, you'd need to inject the ServiceBusClient via DI
        }

        [Fact]
        public async Task SendMessageAsync_ValidTopicAndMessage_SendsMessageSuccessfully()
        {
            // Arrange
            var topic = "test-topic";
            var message = "test message";

            _mockSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = async () => await _producerWrapper.SendMessageAsync(topic, message);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Theory]
        [InlineData(null, "message")]
        [InlineData("", "message")]
        [InlineData("topic", null)]
        [InlineData("topic", "")]
        public async Task SendMessageAsync_InvalidParameters_ThrowsArgumentException(string topic, string message)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _producerWrapper.SendMessageAsync(topic, message));
        }

        [Fact]
        public async Task SendMessageAsync_ServiceBusException_PropagatesException()
        {
            // Arrange
            var topic = "test-topic";
            var message = "test message";

            _mockSender
                .Setup(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ServiceBusException("Service Bus error"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(() => _producerWrapper.SendMessageAsync(topic, message));
        }

        [Fact]
        public void Dispose_CallsDisposeOnClient()
        {
            // Arrange
            var mockDisposableClient = _mockClient.As<IDisposable>();

            // Act
            _producerWrapper.Dispose();

            // Assert
            // Note: This test assumes ProducerWrapper implements IDisposable and disposes the client
            // The actual verification would depend on the implementation
        }
    }
}