using System;
using Xunit;
using Moq;
using FluentAssertions;
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

            // Act & Assert
            await Assert.RaisesAsync<OperationCanceledException>(async () => 
            {
                await service.ExecuteAsync(new System.Threading.CancellationToken(true));
            });
        }

        [Fact]
        public void DeserializeOrder_WithValidJson_ShouldSucceed()
        {
            // Arrange
            var orderJson = "{"productname":"TestProduct", "status":0}";

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(orderJson);

            // Assert
            order.Should().NotBeNull();
            order.productname.Should().Be("TestProduct");
            order.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void DeserializeOrder_WithInvalidJson_ShouldReturnNull()
        {
            // Arrange
            var invalidJson = "invalid json";

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(invalidJson);

            // Assert
            order.Should().BeNull();
        }

        [Fact]
        public void OrderStatus_ShouldHaveExpectedValues()
        {
            // Arrange & Act
            var statusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToArray();

            // Assert
            statusValues.Should().Contain(OrderStatus.COMPLETED);
        }

        [Fact]
        public void ProducerConfig_ShouldAllowConfiguration()
        {
            // Arrange
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            // Assert
            config.BootstrapServers.Should().Be("localhost:9092");
        }

        [Fact]
        public void ConsumerConfig_ShouldAllowConfiguration()
        {
            // Arrange
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group"
            };

            // Assert
            config.BootstrapServers.Should().Be("localhost:9092");
            config.GroupId.Should().Be("test-group");
        }

        [Fact]
        public void OrderRequest_ShouldSupportStatusChange()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct", status = OrderStatus.COMPLETED };

            // Assert
            order.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}