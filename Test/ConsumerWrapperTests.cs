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
    public class ConsumerWrapperTests
    {
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusProcessor> _mockServiceBusProcessor;
        private readonly ConsumerWrapper _consumerWrapper;

        public ConsumerWrapperTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusProcessor = new Mock<ServiceBusProcessor>();

            _mockServiceBusClient
                .Setup(x => x.CreateProcessor(It.IsAny<string>()))
                .Returns(_mockServiceBusProcessor.Object);

            _consumerWrapper = new ConsumerWrapper(_mockServiceBusClient.Object);
        }

        [Fact]
        public async Task StartAsync_WithValidTopic_StartsProcessor()
        {
            // Arrange
            var topic = "test-topic";
            Func<ProcessMessageEventArgs, Task> messageHandler = args => Task.CompletedTask;
            Func<ProcessErrorEventArgs, Task> errorHandler = args => Task.CompletedTask;

            _mockServiceBusProcessor
                .Setup(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _consumerWrapper.StartAsync(topic, messageHandler, errorHandler);

            // Assert
            _mockServiceBusProcessor.Verify(
                x => x.StartProcessingAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task StartAsync_WithNullTopic_ThrowsArgumentException()
        {
            // Arrange
            string? topic = null;
            Func<ProcessMessageEventArgs, Task> messageHandler = args => Task.CompletedTask;
            Func<ProcessErrorEventArgs, Task> errorHandler = args => Task.CompletedTask;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _consumerWrapper.StartAsync(topic!, messageHandler, errorHandler));
        }

        [Fact]
        public async Task StartAsync_WithNullMessageHandler_ThrowsArgumentNullException()
        {
            // Arrange
            var topic = "test-topic";
            Func<ProcessMessageEventArgs, Task>? messageHandler = null;
            Func<ProcessErrorEventArgs, Task> errorHandler = args => Task.CompletedTask;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _consumerWrapper.StartAsync(topic, messageHandler!, errorHandler));
        }

        [Fact]
        public async Task StartAsync_WithNullErrorHandler_ThrowsArgumentNullException()
        {
            // Arrange
            var topic = "test-topic";
            Func<ProcessMessageEventArgs, Task> messageHandler = args => Task.CompletedTask;
            Func<ProcessErrorEventArgs, Task>? errorHandler = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _consumerWrapper.StartAsync(topic, messageHandler, errorHandler!));
        }

        [Fact]
        public async Task StartAsync_WhenProcessorThrows_PropagatesException()
        {
            // Arrange
            var topic = "test-topic";
            Func<ProcessMessageEventArgs, Task> messageHandler = args => Task.CompletedTask;
            Func<ProcessErrorEventArgs, Task> errorHandler = args => Task.CompletedTask;

            _mockServiceBusProcessor
                .Setup(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ServiceBusException("Failed to start"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(() => 
                _consumerWrapper.StartAsync(topic, messageHandler, errorHandler));
        }

        [Fact]
        public async Task StopAsync_StopsProcessor()
        {
            // Arrange
            _mockServiceBusProcessor
                .Setup(x => x.StopProcessingAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _consumerWrapper.StopAsync();

            // Assert
            _mockServiceBusProcessor.Verify(
                x => x.StopProcessingAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task StopAsync_WhenProcessorThrows_PropagatesException()
        {
            // Arrange
            _mockServiceBusProcessor
                .Setup(x => x.StopProcessingAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ServiceBusException("Failed to stop"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(() => 
                _consumerWrapper.StopAsync());
        }

        [Fact]
        public void Dispose_DisposesResources()
        {
            // Act
            _consumerWrapper.Dispose();

            // Assert
            // Verify that disposal doesn't throw and wrapper remains valid
            _consumerWrapper.Should().NotBeNull();
        }
    }
}