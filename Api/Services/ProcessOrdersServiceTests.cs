using System;
using Xunit;
using Moq;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Confluent.Kafka;
using Api.Services;
using Api.Models;
using Api;

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
            var consumerWrapperMock = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            var producerWrapperMock = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");

            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct",
                status = OrderStatus.PENDING
            };

            consumerWrapperMock
                .Setup(x => x.readMessage())
                .Returns(JsonConvert.SerializeObject(orderRequest));

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            producerWrapperMock
                .Verify(x => x.writeMessage(It.Is<string>(msg => 
                    JsonConvert.DeserializeObject<OrderRequest>(msg).status == OrderStatus.COMPLETED)), 
                Times.Once);
        }

        [Fact]
        public void Constructor_ValidConfigs_InitializesSuccessfully()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_CancellationRequested_StopsProcessing()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await service.StartAsync(cancellationTokenSource.Token);
        }

        [Theory]
        [InlineData("Product1")]
        [InlineData("Product2")]
        public async Task ExecuteAsync_MultipleProducts_ProcessesCorrectly(string productName)
        {
            // Arrange
            var consumerWrapperMock = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            var producerWrapperMock = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");

            var orderRequest = new OrderRequest 
            { 
                productname = productName,
                status = OrderStatus.PENDING
            };

            consumerWrapperMock
                .Setup(x => x.readMessage())
                .Returns(JsonConvert.SerializeObject(orderRequest));

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            producerWrapperMock
                .Verify(x => x.writeMessage(It.Is<string>(msg => 
                    JsonConvert.DeserializeObject<OrderRequest>(msg).status == OrderStatus.COMPLETED)), 
                Times.Once);
        }
    }
}