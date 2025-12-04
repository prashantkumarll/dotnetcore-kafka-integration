using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Api.Services;
using Api.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace Api.Tests
{
    public class ProcessOrdersServiceTests
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;

        public ProcessOrdersServiceTests()
        {
            _consumerConfig = new ConsumerConfig { GroupId = "test-group" };
            _producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
        }

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
        public void Constructor_WithNullConsumerConfig_ShouldNotThrow()
        {
            // Arrange
            ConsumerConfig consumerConfig = default!;
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act & Assert
            var action = () => new ProcessOrdersService(consumerConfig, producerConfig);
            action.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldNotThrow()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig { GroupId = "test-group" };
            ProducerConfig producerConfig = default!;

            // Act & Assert
            var action = () => new ProcessOrdersService(consumerConfig, producerConfig);
            action.Should().NotThrow();
        }

        [Fact]
        public void Constructor_WithBothNullConfigs_ShouldNotThrow()
        {
            // Arrange
            ConsumerConfig consumerConfig = default!;
            ProducerConfig producerConfig = default!;

            // Act & Assert
            var action = () => new ProcessOrdersService(consumerConfig, producerConfig);
            action.Should().NotThrow();
        }

        [Fact]
        public async Task StartAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = CancellationToken.None;

            // Act
            var action = async () => await service.StartAsync(cancellationToken);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_ShouldCompleteSuccessfully()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = CancellationToken.None;

            // Act
            var action = async () => await service.StopAsync(cancellationToken);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartAsync_WithCancelledToken_ShouldHandleGracefully()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            var action = async () => await service.StartAsync(cts.Token);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithCancelledToken_ShouldHandleGracefully()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            var action = async () => await service.StopAsync(cts.Token);

            // Assert
            await action.Should().NotThrowAsync();
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
        public void DeserializeOrderRequest_EmptyJson_ShouldReturnNull()
        {
            // Arrange
            var emptyJson = "";

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(emptyJson);

            // Assert
            order.Should().BeNull();
        }

        [Fact]
        public void DeserializeOrderRequest_NullJson_ShouldReturnNull()
        {
            // Arrange
            string nullJson = default!;

            // Act
            var order = JsonConvert.DeserializeObject<OrderRequest>(nullJson);

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
            json.Should().NotBeNullOrEmpty();
            json.Should().Contain("TestProduct");
        }

        [Fact]
        public void SerializeOrderRequest_WithSpecialCharacters_ShouldProduceValidJson()
        {
            // Arrange
            var order = new OrderRequest { productname = "Test Product With Spaces", status = OrderStatus.COMPLETED };

            // Act
            var json = JsonConvert.SerializeObject(order);

            // Assert
            json.Should().NotBeNullOrEmpty();
            json.Should().Contain("Test Product With Spaces");
        }

        [Fact]
        public void SerializeOrderRequest_WithNullProductName_ShouldProduceValidJson()
        {
            // Arrange
            var order = new OrderRequest { productname = default!, status = OrderStatus.COMPLETED };

            // Act
            var json = JsonConvert.SerializeObject(order);

            // Assert
            json.Should().NotBeNullOrEmpty();
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

        [Theory]
        [InlineData(OrderStatus.IN_PROGRESS)]
        [InlineData(OrderStatus.COMPLETED)]
        [InlineData(OrderStatus.REJECTED)]
        public void OrderRequest_SetStatus_ShouldAcceptAllValidStatuses(OrderStatus status)
        {
            // Arrange
            var order = new OrderRequest { productname = "TestProduct" };

            // Act
            order.status = status;

            // Assert
            order.status.Should().Be(status);
        }

        [Fact]
        public void ProcessOrdersService_InheritsFromBackgroundService()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);

            // Act & Assert
            service.Should().BeAssignableTo<BackgroundService>();
        }

        [Fact]
        public void ProcessOrdersService_ImplementsIHostedService()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);

            // Act & Assert
            service.Should().BeAssignableTo<IHostedService>();
        }
    }
}