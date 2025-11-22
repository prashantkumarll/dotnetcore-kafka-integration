using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Services.Tests
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
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");

            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.PENDING 
            };

            mockConsumerWrapper
                .Setup(x => x.readMessage())
                .Returns(JsonConvert.SerializeObject(orderRequest));

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            mockProducerWrapper.Verify(x => x.writeMessage(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Constructor_ValidConfigs_InitializesCorrectly()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_NullOrderRequest_HandlesGracefully()
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper
                .Setup(x => x.readMessage())
                .Returns(JsonConvert.SerializeObject(null));

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await service.StartAsync(cancellationTokenSource.Token);
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

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper
                .Setup(x => x.readMessage())
                .Returns(JsonConvert.SerializeObject(orderRequest));

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}