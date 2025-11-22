using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Models;
using Api.Services;

namespace Api.Tests.Services
{
    public class ProcessOrdersServiceTests : IDisposable
    {
        private readonly Mock<ConsumerConfig> _mockConsumerConfig;
        private readonly Mock<ProducerConfig> _mockProducerConfig;
        private readonly Mock<ConsumerWrapper> _mockConsumerWrapper;
        private readonly Mock<ProducerWrapper> _mockProducerWrapper;

        public ProcessOrdersServiceTests()
        {
            _mockConsumerConfig = new Mock<ConsumerConfig>();
            _mockProducerConfig = new Mock<ProducerConfig>();
            _mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            _mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");
        }

        [Fact]
        public async Task ExecuteAsync_ValidOrderRequest_ProcessesOrderSuccessfully()
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.PENDING 
            };
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            _mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);
            _mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            _mockConsumerWrapper.Verify(x => x.readMessage(), Times.AtLeastOnce());
            _mockProducerWrapper.Verify(x => x.writeMessage(It.Is<string>(s => 
                JsonConvert.DeserializeObject<OrderRequest>(s).status == OrderStatus.COMPLETED)), Times.AtLeastOnce());
        }

        [Fact]
        public async Task ExecuteAsync_NullOrderRequest_HandlesGracefully()
        {
            // Arrange
            _mockConsumerWrapper.Setup(x => x.readMessage()).Returns((string)null);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(() => service.StartAsync(cancellationTokenSource.Token));
        }

        [Fact]
        public void Constructor_ValidConfigs_InitializesCorrectly()
        {
            // Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Electronics")]
        [InlineData("Clothing")]
        public async Task ExecuteAsync_MultipleProductTypes_ProcessesCorrectly(string productName)
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                productname = productName, 
                status = OrderStatus.PENDING 
            };
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            _mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);
            _mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            _mockProducerWrapper.Verify(x => x.writeMessage(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task ExecuteAsync_CancellationRequested_StopsProcessing()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            _mockConsumerWrapper.Verify(x => x.readMessage(), Times.Never());
        }

        [Fact]
        public void Constructor_NullConfigs_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProcessOrdersService(null, null));
        }

        [Fact]
        public async Task ExecuteAsync_ProducerWriteFailure_HandlesException()
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.PENDING 
            };
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            _mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);
            _mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).ThrowsAsync(new Exception("Producer write failed"));

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.StartAsync(cancellationTokenSource.Token));
        }

        public void Dispose()
        {
            // Cleanup resources if needed
        }
    }
}