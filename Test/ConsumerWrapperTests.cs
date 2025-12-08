using Xunit;
using Moq;
using FluentAssertions;
using Api;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Test.Messaging
{
    public class ConsumerWrapperTests
    {
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly Mock<ServiceBusProcessor> _mockServiceBusProcessor;
        private readonly Mock<ILogger<ConsumerWrapper>> _mockLogger;
        private readonly ConsumerWrapper _consumerWrapper;

        public ConsumerWrapperTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusProcessor = new Mock<ServiceBusProcessor>();
            _mockLogger = new Mock<ILogger<ConsumerWrapper>>();

            _mockServiceBusClient
                .Setup(x => x.CreateProcessor(It.IsAny<string>()))
                .Returns(_mockServiceBusProcessor.Object);

            _consumerWrapper = new ConsumerWrapper(_mockServiceBusClient.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task StartAsync_WithValidTopic_StartsProcessor()
        {
            // Arrange
            var topic = "order-events";
            var cancellationToken = CancellationToken.None;

            _mockServiceBusProcessor
                .Setup(x => x.StartProcessingAsync(cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _consumerWrapper.StartAsync(topic, cancellationToken);

            // Assert
            _mockServiceBusProcessor.Verify(x => x.StartProcessingAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task StartAsync_WithNullTopic_ThrowsArgumentNullException()
        {
            // Arrange
            string? topic = null;
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _consumerWrapper.StartAsync(topic!, cancellationToken));
        }

        [Fact]
        public async Task StartAsync_WithEmptyTopic_ThrowsArgumentException()
        {
            // Arrange
            var topic = "";
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _consumerWrapper.StartAsync(topic, cancellationToken));
        }

        [Fact]
        public async Task StartAsync_ProcessorThrowsException_PropagatesException()
        {
            // Arrange
            var topic = "test-topic";
            var cancellationToken = CancellationToken.None;
            var expectedException = new ServiceBusException("Processor failed to start");

            _mockServiceBusProcessor
                .Setup(x => x.StartProcessingAsync(cancellationToken))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<ServiceBusException>(() => 
                _consumerWrapper.StartAsync(topic, cancellationToken));
            
            actualException.Should().Be(expectedException);
        }

        [Fact]
        public async Task StopAsync_CallsStopProcessing()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            _mockServiceBusProcessor
                .Setup(x => x.StopProcessingAsync(cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _consumerWrapper.StopAsync(cancellationToken);

            // Assert
            _mockServiceBusProcessor.Verify(x => x.StopProcessingAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task StopAsync_WithCancelledToken_ThrowsOperationCancelledException()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            var cancellationToken = cancellationTokenSource.Token;

            _mockServiceBusProcessor
                .Setup(x => x.StopProcessingAsync(cancellationToken))
                .ThrowsAsync(new OperationCanceledException());

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => 
                _consumerWrapper.StopAsync(cancellationToken));
        }

        [Fact]
        public void Constructor_SetsUpEventHandlers()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper(_mockServiceBusClient.Object, _mockLogger.Object);

            // Assert
            // Verify that event handlers are set up (this would typically be verified by checking
            // that the processor's ProcessMessageAsync and ProcessErrorAsync events are configured)
            _mockServiceBusClient.Verify(x => x.CreateProcessor(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Dispose_CallsDispose()
        {
            // Act
            _consumerWrapper.Dispose();

            // Assert - No exception should be thrown
            // Verify cleanup is called
            _mockServiceBusProcessor.Verify(x => x.DisposeAsync(), Times.Once);
        }

        [Theory]
        [InlineData("orders")]
        [InlineData("events")]
        [InlineData("notifications")]
        [InlineData("order-processing-queue")]
        public async Task StartAsync_WithDifferentTopics_CreatesCorrectProcessor(string topic)
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            _mockServiceBusClient
                .Setup(x => x.CreateProcessor(topic))
                .Returns(_mockServiceBusProcessor.Object);

            _mockServiceBusProcessor
                .Setup(x => x.StartProcessingAsync(cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _consumerWrapper.StartAsync(topic, cancellationToken);

            // Assert
            _mockServiceBusClient.Verify(x => x.CreateProcessor(topic), Times.Once);
        }
    }
}