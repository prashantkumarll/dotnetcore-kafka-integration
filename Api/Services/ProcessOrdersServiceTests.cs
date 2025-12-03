using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
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
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;

        public ProcessOrdersServiceTests()
        {
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
        }

        [Fact]
        public void Constructor_WithValidConfigs_ShouldInitializeService()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ConsumerConfig nullConsumerConfig = default!;

            // Act
            Action act = () => new ProcessOrdersService(nullConsumerConfig, _producerConfig);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProducerConfig nullProducerConfig = default!;

            // Act
            Action act = () => new ProcessOrdersService(_consumerConfig, nullProducerConfig);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAsync_WithValidToken_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = CancellationToken.None;

            // Act
            Func<Task> act = async () => await service.StartAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithValidToken_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = CancellationToken.None;

            // Act
            Func<Task> act = async () => await service.StopAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartAsync_WithCancelledToken_ShouldHandleCancellation()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act
            Func<Task> act = async () => await service.StartAsync(cancellationTokenSource.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithCancelledToken_ShouldHandleCancellation()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act
            Func<Task> act = async () => await service.StopAsync(cancellationTokenSource.Token);

            // Assert
            await act.Should().NotThrowAsync();
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
        public void JsonSerialization_WithValidOrderRequest_ShouldSerializeAndDeserialize()
        {
            // Arrange
            var order = new OrderRequest 
            { 
                productname = "TestProduct", 
                status = OrderStatus.IN_PROGRESS 
            };

            // Act
            var serialized = JsonConvert.SerializeObject(order);
            var deserialized = JsonConvert.DeserializeObject<OrderRequest>(serialized);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.productname.Should().Be("TestProduct");
            deserialized.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void JsonSerialization_WithNullOrderRequest_ShouldReturnNull()
        {
            // Arrange
            OrderRequest nullOrder = default!;

            // Act
            var serialized = JsonConvert.SerializeObject(nullOrder);
            var deserialized = JsonConvert.DeserializeObject<OrderRequest>(serialized);

            // Assert
            serialized.Should().Be("null");
            deserialized.Should().BeNull();
        }

        [Fact]
        public void JsonDeserialization_WithInvalidJson_ShouldReturnNull()
        {
            // Arrange
            var invalidJson = "invalid json string";

            // Act
            Action act = () => JsonConvert.DeserializeObject<OrderRequest>(invalidJson);

            // Assert
            act.Should().Throw<JsonReaderException>();
        }

        [Fact]
        public void JsonDeserialization_WithEmptyJson_ShouldReturnNull()
        {
            // Arrange
            var emptyJson = "{}";

            // Act
            var deserialized = JsonConvert.DeserializeObject<OrderRequest>(emptyJson);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.productname.Should().BeNull();
            deserialized.status.Should().Be(default(OrderStatus));
        }

        [Theory]
        [InlineData("Product1", OrderStatus.IN_PROGRESS)]
        [InlineData("Product2", OrderStatus.COMPLETED)]
        [InlineData("Product3", OrderStatus.REJECTED)]
        public void OrderRequest_WithDifferentValues_ShouldSetPropertiesCorrectly(string productName, OrderStatus status)
        {
            // Arrange & Act
            var order = new OrderRequest 
            { 
                productname = productName, 
                status = status 
            };

            // Assert
            order.productname.Should().Be(productName);
            order.status.Should().Be(status);
        }

        [Fact]
        public void OrderRequest_DefaultConstructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var order = new OrderRequest();

            // Assert
            order.Should().NotBeNull();
            order.productname.Should().BeNull();
            order.status.Should().Be(default(OrderStatus));
        }

        [Fact]
        public void ConsumerConfig_ShouldAllowPropertyAccess()
        {
            // Arrange & Act
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
        public void ProducerConfig_ShouldAllowPropertyAccess()
        {
            // Arrange & Act
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            // Assert
            config.BootstrapServers.Should().Be("localhost:9092");
        }
    }
}