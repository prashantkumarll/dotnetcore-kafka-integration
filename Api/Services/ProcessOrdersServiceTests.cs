using System;
using Xunit;
using Moq;
using FluentAssertions;
using Api.Services;
using Api.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<ServiceBusProcessorOptions> _mockConsumerConfig;
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;

        public ProcessOrdersServiceTests()
        {
            _mockConsumerConfig = new Mock<ServiceBusProcessorOptions>();
            _mockServiceBusClient = new Mock<ServiceBusClient>();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithValidConfigs()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockServiceBusClient.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_WithValidOrder_ShouldProcessAndComplete()
        {
            // Arrange
            var orderRequest = new OrderRequest { productname = "TestProduct" };
            var cancellationTokenSource = new CancellationTokenSource();

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(JsonConvert.SerializeObject(orderRequest));

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockServiceBusClient.Object, "readytoship");

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockServiceBusClient.Object);

            // Act
            await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            orderRequest.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyMessage_ShouldContinue()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(string.Empty);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await service.StartAsync(cancellationTokenSource.Token);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullOrder_ShouldLogWarning()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns("null");

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockServiceBusClient.Object);

            // Act & Assert
            await service.StartAsync(cancellationTokenSource.Token);
        }
    }
}