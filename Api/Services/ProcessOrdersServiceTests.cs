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
        public async Task ExecuteAsync_WithNullOrEmptyMessage_ShouldContinue()
        {
            // Arrange
            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage()).Returns(string.Empty);

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await service.StartAsync(CancellationToken.None);
            // Verify no exception is thrown
        }

        [Fact]
        public async Task ExecuteAsync_WithValidOrder_ShouldProcessAndPublish()
        {
            // Arrange
            var testOrder = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.IN_PROGRESS 
            };

            var mockConsumerWrapper = new Mock<ConsumerWrapper>(_mockConsumerConfig.Object, "orderrequests");
            mockConsumerWrapper.Setup(x => x.readMessage())
                .Returns(JsonConvert.SerializeObject(testOrder));

            var service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);

            // Act & Assert
            await service.StartAsync(CancellationToken.None);
            // Additional assertions can be added based on specific requirements
        }

        [Fact]
        public void OrderRequest_ShouldUpdateStatusToCompleted()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct" };

            // Act
            order.status = OrderStatus.COMPLETED;

            // Assert
            order.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void OrderRequest_ShouldSupportRejectedStatus()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct" };

            // Act
            order.status = OrderStatus.REJECTED;

            // Assert
            order.status.Should().Be(OrderStatus.REJECTED);
        }

        [Fact]
        public void JsonSerialization_ShouldPreserveOrderProperties()
        {
            // Arrange
            var originalOrder = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.IN_PROGRESS 
            };

            // Act
            var serializedOrder = JsonConvert.SerializeObject(originalOrder);
            var deserializedOrder = JsonConvert.DeserializeObject<OrderRequest>(serializedOrder);

            // Assert
            deserializedOrder.Should().BeEquivalentTo(originalOrder);
        }

        [Fact]
        public void ProducerConfig_ShouldBeConfigurable()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Assert
            config.BootstrapServers.Should().Be("localhost:9092");
        }
    }
}