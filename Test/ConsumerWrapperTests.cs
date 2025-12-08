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
        private readonly string _testTopic = "test-topic";

        public ConsumerWrapperTests()
        {
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _mockServiceBusProcessor = new Mock<ServiceBusProcessor>();
            
            _mockServiceBusClient
                .Setup(x => x.CreateProcessor(_testTopic, It.IsAny<ServiceBusProcessorOptions>()))
                .Returns(_mockServiceBusProcessor.Object);

            _consumerWrapper = new ConsumerWrapper(_mockServiceBusClient.Object, _testTopic);
        }

        [Fact]
        public async Task StartProcessingAsync_StartsProcessor()
        {
            // Arrange
            _mockServiceBusProcessor
                .Setup(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _consumerWrapper.StartProcessingAsync();

            // Assert
            _mockServiceBusProcessor.Verify(
                x => x.StartProcessingAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task StopProcessingAsync_StopsProcessor()
        {
            // Arrange
            _mockServiceBusProcessor
                .Setup(x => x.StopProcessingAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _consumerWrapper.StopProcessingAsync();

            // Assert
            _mockServiceBusProcessor.Verify(
                x => x.StopProcessingAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void Constructor_WithNullClient_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new ConsumerWrapper(null, _testTopic));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithInvalidTopic_ThrowsArgumentException(string topic)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new ConsumerWrapper(_mockServiceBusClient.Object, topic));
        }

        [Fact]
        public async Task StartProcessingAsync_WhenProcessorThrows_PropagatesException()
        {
            // Arrange
            _mockServiceBusProcessor
                .Setup(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ServiceBusException("Start failed"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(
                () => _consumerWrapper.StartProcessingAsync());
        }

        [Fact]
        public async Task StopProcessingAsync_WhenProcessorThrows_PropagatesException()
        {
            // Arrange
            _mockServiceBusProcessor
                .Setup(x => x.StopProcessingAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ServiceBusException("Stop failed"));

            // Act & Assert
            await Assert.ThrowsAsync<ServiceBusException>(
                () => _consumerWrapper.StopProcessingAsync());
        }

        [Fact]
        public void Constructor_SetsUpEventHandlers()
        {
            // Arrange & Act
            var consumer = new ConsumerWrapper(_mockServiceBusClient.Object, _testTopic);

            // Assert
            _mockServiceBusClient.Verify(
                x => x.CreateProcessor(_testTopic, It.IsAny<ServiceBusProcessorOptions>()),
                Times.Once);
        }

        [Fact]
        public async Task StartAndStopProcessing_WorksCorrectly()
        {
            // Arrange
            _mockServiceBusProcessor
                .Setup(x => x.StartProcessingAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            
            _mockServiceBusProcessor
                .Setup(x => x.StopProcessingAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _consumerWrapper.StartProcessingAsync();
            await _consumerWrapper.StopProcessingAsync();

            // Assert
            _mockServiceBusProcessor.Verify(
                x => x.StartProcessingAsync(It.IsAny<CancellationToken>()),
                Times.Once);
            _mockServiceBusProcessor.Verify(
                x => x.StopProcessingAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void Dispose_CallsDisposeOnProcessor()
        {
            // Act
            _consumerWrapper.Dispose();

            // Assert
            _mockServiceBusProcessor.Verify(x => x.DisposeAsync(), Times.Once);
        }

        [Fact]
        public void Constructor_WithValidParameters_CreatesProcessorWithCorrectTopic()
        {
            // Arrange
            var customTopic = "custom-topic";

            // Act
            var consumer = new ConsumerWrapper(_mockServiceBusClient.Object, customTopic);

            // Assert
            _mockServiceBusClient.Verify(
                x => x.CreateProcessor(customTopic, It.IsAny<ServiceBusProcessorOptions>()),
                Times.Once);
        }
    }
}