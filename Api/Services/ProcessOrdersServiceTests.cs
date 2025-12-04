using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Api.Services;
using Api.Models;
using Newtonsoft.Json;

namespace Api.Tests
{
    public class ProcessOrdersServiceTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithValidConfigs()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig { GroupId = "test-group" };
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void DeserializeOrderRequest_ValidJson_ShouldReturnOrderRequest()
        {
            // Arrange
            var orderJson = "{"productname":"TestProduct", "status":0}";

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(orderJson);

            // Assert
            order.Should().NotBeNull();
            order.productname.Should().Be("TestProduct");
            order.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void DeserializeOrderRequest_InvalidJson_ShouldReturnNull()
        {
            // Arrange
            var invalidJson = "{ invalid json }";

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
            statusValues.Should().Contain(new[] { OrderStatus.IN_PROGRESS, OrderStatus.COMPLETED, OrderStatus.REJECTED });
            statusValues.Length.Should().Be(3);
        }

        [Fact]
        public void SerializeOrderRequest_ShouldProduceValidJson()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct", status = OrderStatus.COMPLETED };

            // Act
            var json = JsonConvert.SerializeObject(order);

            // Assert
            json.Should().Contain("TestProduct");
            json.Should().Contain("COMPLETED");
        }

        [Fact]
        public void ProcessOrdersService_NullOrEmptyMessage_ShouldContinue()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig { GroupId = "test-group" };
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Act & Assert
            service.Invoking(s => s.ExecuteAsync(default)).Should().NotThrow();
        }

        [Fact]
        public void OrderRequest_DefaultStatus_ShouldBeInProgress()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct" };

            // Assert
            order.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void OrderRequest_UpdateStatus_ShouldChangeCorrectly()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct" };

            // Act
            order.status = OrderStatus.COMPLETED;

            // Assert
            order.status.Should().Be(OrderStatus.COMPLETED);
        }
    }
}