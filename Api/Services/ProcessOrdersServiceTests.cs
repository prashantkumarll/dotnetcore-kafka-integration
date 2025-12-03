using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api.Services;
using Api.Models;
using Microsoft.Extensions.Hosting;

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
            service.Should().BeAssignableTo<BackgroundService>();
        }

        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(null, _producerConfig);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(_consumerConfig, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithBothConfigsNull_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(null, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAsync_WithValidService_ShouldStartSuccessfully()
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
        public async Task StopAsync_WithRunningService_ShouldStopSuccessfully()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = CancellationToken.None;
            await service.StartAsync(cancellationToken);

            // Act
            Func<Task> act = async () => await service.StopAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithCancelledToken_ShouldHandleCancellation()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationTokenSource = new CancellationTokenSource();
            await service.StartAsync(CancellationToken.None);
            cancellationTokenSource.Cancel();

            // Act
            Func<Task> act = async () => await service.StopAsync(cancellationTokenSource.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("t")]
        [InlineData("n")]
        public void JsonDeserialization_WithEmptyOrWhitespaceString_ShouldReturnNull(string input)
        {
            // Arrange & Act
            var result = JsonConvert.DeserializeObject<OrderRequest>(input);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void JsonDeserialization_WithValidOrderJson_ShouldDeserializeCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct",
                status = OrderStatus.IN_PROGRESS
            };
            var json = JsonConvert.SerializeObject(orderRequest);

            // Act
            var result = JsonConvert.DeserializeObject<OrderRequest>(json);

            // Assert
            result.Should().NotBeNull();
            result.productname.Should().Be("TestProduct");
            result.status.Should().Be(OrderStatus.IN_PROGRESS);
        }

        [Fact]
        public void JsonDeserialization_WithInvalidJson_ShouldThrowJsonException()
        {
            // Arrange
            var invalidJson = "{ invalid json }";

            // Act & Assert
            Action act = () => JsonConvert.DeserializeObject<OrderRequest>(invalidJson);
            act.Should().Throw<JsonReaderException>();
        }

        [Fact]
        public void JsonSerialization_WithOrderRequest_ShouldSerializeCorrectly()
        {
            // Arrange
            var orderRequest = new OrderRequest 
            { 
                productname = "TestProduct",
                status = OrderStatus.COMPLETED
            };

            // Act
            var json = JsonConvert.SerializeObject(orderRequest);

            // Assert
            json.Should().NotBeNullOrEmpty();
            json.Should().Contain("TestProduct");
            json.Should().Contain("COMPLETED");
        }
    }
}