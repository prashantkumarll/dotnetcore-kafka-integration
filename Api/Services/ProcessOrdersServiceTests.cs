using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Api.Services;
using Api.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<ConsumerConfig> _mockConsumerConfig;
        private readonly Mock<ProducerConfig> _mockProducerConfig;

        public ProcessOrdersServiceTests()
        {
            _mockConsumerConfig = new Mock<ConsumerConfig>();
            _mockProducerConfig = new Mock<ProducerConfig>();
        }

        [Fact]
        public async Task ExecuteAsync_ValidOrderRequest_ProcessesOrderSuccessfully()
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.IN_PROGRESS 
            };
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");
            mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            mockConsumerWrapper.Verify(x => x.readMessage(), Times.AtLeastOnce());
            mockProducerWrapper.Verify(x => x.writeMessage(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Fact]
        public void Constructor_ValidConfigs_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_CancellationRequested_ShouldStopProcessing()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await service.StartAsync(cancellationTokenSource.Token);
        }

        [Fact]
        public async Task ExecuteAsync_NullOrderRequest_ShouldHandleGracefully()
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns((string)null);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await service.StartAsync(cancellationTokenSource.Token);
        }

        [Fact]
        public void Constructor_NullConfigs_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProcessOrdersService(null, null));
        }
    }
}