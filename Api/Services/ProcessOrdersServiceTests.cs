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
            var orderJson = "{"productname":"TestProduct","status":0}";

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
            statusValues.Should().Contain(OrderStatus.IN_PROGRESS);
            statusValues.Should().Contain(OrderStatus.COMPLETED);
            statusValues.Should().Contain(OrderStatus.REJECTED);
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
        public void OrderRequest_ShouldAllowStatusChange()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct", status = OrderStatus.IN_PROGRESS };

            // Act
            order.status = OrderStatus.COMPLETED;

            // Assert
            order.status.Should().Be(OrderStatus.COMPLETED);
        }

        [Fact]
        public void ProcessOrdersService_LogsStartupMessage()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig { GroupId = "test-group" };
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act & Assert
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                var service = new ProcessOrdersService(consumerConfig, producerConfig);

                writer.ToString().Should().Contain("OrderProcessing Service Started");
            }
        }

        [Fact]
        public void OrderRequest_DefaultStatusIsInProgress()
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct" };

            // Assert
            order.status.Should().Be(OrderStatus.IN_PROGRESS);
        }
    }
}