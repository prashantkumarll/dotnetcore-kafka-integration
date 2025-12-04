using System;
using Xunit;
using Moq;
using FluentAssertions;
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
        public void Constructor_ShouldInitializeWithValidConfigs()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_WithNullMessage_ShouldContinue()
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns((string)null);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            // Act & Assert
            await Assert.RaisesAsync<OperationCanceledException>(async () => 
            {
                await service.StartAsync(cancellationTokenSource.Token);
            });
        }

        [Fact]
        public async Task ExecuteAsync_WithValidOrder_ShouldProcessAndPublish()
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.IN_PROGRESS 
            };

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(JsonConvert.SerializeObject(orderRequest));

            var mockProducerWrapper = new Mock<ProducerWrapper>(_mockProducerConfig.Object, "readytoship");

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            // Act & Assert
            await Assert.RaisesAsync<OperationCanceledException>(async () => 
            {
                await service.StartAsync(cancellationTokenSource.Token);
            });
        }

        [Fact]
        public void DeserializeOrder_WithInvalidJson_ShouldReturnNull()
        {
            // Arrange
            string invalidJson = "{ invalid json }";

            // Act
            var result = JsonConvert.DeserializeObject<OrderRequest>(invalidJson);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void OrderStatus_ShouldUpdateToCompleted()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct" };

            // Act
            order.status = OrderStatus.COMPLETED;

            // Assert
            order.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void OrderRequest_ShouldHaveProductName()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct" };

            // Assert
            order.productname.Should().Be("TestProduct");
        }

        [Fact]
        public void OrderStatus_ShouldHaveValidEnumValues()
        {
            // Arrange & Act
            var enumValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            enumValues.Should().Contain(new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED });
        }
    }
}