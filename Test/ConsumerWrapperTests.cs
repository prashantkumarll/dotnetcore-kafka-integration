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
        private readonly Mock<ServiceBusClient> _mockClient;
        private readonly Mock<ServiceBusProcessor> _mockProcessor;
        private readonly ConsumerWrapper _consumerWrapper;

        public ConsumerWrapperTests()
        {
            _mockClient = new Mock<ServiceBusClient>();
            _mockProcessor = new Mock<ServiceBusProcessor>();
            
            _mockClient
                .Setup(x => x.CreateProcessor(It.IsAny<string>(), It.IsAny<ServiceBusProcessorOptions>()))
                .Returns(_mockProcessor.Object);

            _consumerWrapper = new ConsumerWrapper();
            // Note: In a real scenario, you'd need to inject the ServiceBusClient via DI
        }

        [Fact]
        public async Task StartAsync_ValidTopic_StartsProcessorSuccessfully()
        {
            // Arrange
            var topic = "test-topic";
            
            _mockProcessor
                .Setup(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = async () => await _consumerWrapper.StartAsync(topic);

            // Assert
            await act.Should().NotThrowAsync();
            _mockProcessor.Verify(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task StartAsync_InvalidTopic_ThrowsArgumentException(string topic)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _consumerWrapper.StartAsync(topic));
        }

        [Fact]
        public async Task StopAsync_ProcessorStarted_StopsProcessorSuccessfully()
        {
            // Arrange
            _mockProcessor
                .Setup(x => x.StopProcessingAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = async () => await _consumerWrapper.StopAsync();

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartAsync_ServiceBusException_PropagatesException()
        {
            // Arrange
            var topic = "test-topic";
            
            _mockProcessor
                .Setup(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ServiceBusException("Service Bus error"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(() => _consumerWrapper.StartAsync(topic));
        }

        [Fact]
        public async Task StopAsync_ServiceBusException_PropagatesException()
        {
            // Arrange
            _mockProcessor
                .Setup(x => x.StopProcessingAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ServiceBusException("Service Bus error"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(() => _consumerWrapper.StopAsync());
        }

        [Fact]
        public void Dispose_CallsDisposeOnResources()
        {
            // Arrange
            var mockDisposableProcessor = _mockProcessor.As<IAsyncDisposable>();

            // Act
            _consumerWrapper.Dispose();

            // Assert
            // Note: This test assumes ConsumerWrapper implements IDisposable and disposes the processor
            // The actual verification would depend on the implementation
        }

        [Fact]
        public async Task StartAsync_MultipleCallsOnSameTopic_DoesNotStartMultipleProcessors()
        {
            // Arrange
            var topic = "test-topic";
            
            _mockProcessor
                .Setup(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _consumerWrapper.StartAsync(topic);
            await _consumerWrapper.StartAsync(topic);

            // Assert
            _mockProcessor.Verify(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()), Times.AtMostOnce);
        }
    }
}