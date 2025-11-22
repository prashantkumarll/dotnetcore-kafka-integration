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
        private readonly Mock<ConsumerWrapper> _mockConsumerWrapper;
        private readonly Mock<ProducerWrapper> _mockProducerWrapper;

        public ProcessOrdersServiceTests()
        {
            _mockConsumerConfig = new Mock<ConsumerConfig>();
            _mockProducerConfig = new Mock<ProducerConfig>();
            _mockConsumerWrapper = new Mock<ConsumerWrapper>();
            _mockProducerWrapper = new Mock<ProducerWrapper>();
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

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");
            mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            mockConsumerWrapper.Verify(x => x.readMessage(), Times.AtLeastOnce());
            mockProducerWrapper.Verify(x => x.writeMessage(It.IsAny<string>()), Times.AtLeastOnce());
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
        public async Task ExecuteAsync_CancellationRequested_StopsProcessing()
        {
            // Arrange
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            cancellationTokenSource.Token.IsCancellationRequested.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ExecuteAsync_InvalidOrderRequest_HandlesNullOrEmptyInput(string invalidInput)
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(invalidInput);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(() => service.StartAsync(cancellationTokenSource.Token));
        }

        [Fact]
        public async Task ExecuteAsync_OrderProcessing_UpdatesOrderStatus()
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.PENDING 
            };
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");
            mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            var deserializedOrder = JsonConvert.DeserializeObject<OrderRequest>(serializedOrder);
            deserializedOrder.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}