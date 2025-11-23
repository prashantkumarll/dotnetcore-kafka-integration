using System;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Services;
using Api.Models;

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
        public void Constructor_ShouldInitializeWithConfigs()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessOrder_WhenMessageReceived()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

            var orderRequest = new OrderRequest { productname = "TestProduct" };
            var serializedOrder = JsonConvert.SerializeObject(orderRequest);

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(serializedOrder);

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");
            mockProducerWrapper.Setup(x => x.writeMessage(It.IsAny<string>())).Returns(Task.CompletedTask);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act
            await service.ExecuteAsync(cancellationTokenSource.Token);

            // Assert
            mockConsumerWrapper.Verify(x => x.readMessage(), Times.AtLeastOnce());
            mockProducerWrapper.Verify(x => x.writeMessage(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Fact]
        public void DeserializeOrder_ShouldConvertJsonToOrderRequest()
        {
            // Arrange
            var jsonOrder = "{"productname":"TestProduct", "status":0}";

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(jsonOrder);

            // Assert
            order.Should().NotBeNull();
            order.productname.Should().Be("TestProduct");
            order.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderStatus_ShouldHaveExpectedValues()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus));

            // Assert
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleCancellation()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await service.ExecuteAsync(cancellationTokenSource.Token);
        }
    }
}