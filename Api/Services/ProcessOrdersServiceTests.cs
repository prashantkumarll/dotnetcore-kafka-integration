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
        public void Constructor_WithBothNullConfigs_ShouldThrowArgumentNullException()
        {
            // Arrange
            ConsumerConfig nullConsumerConfig = default!;
            ProducerConfig nullProducerConfig = default!;

            // Act
            Action act = () => new ProcessOrdersService(nullConsumerConfig, nullProducerConfig);

            // Assert
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

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("t")]
        [InlineData("n")]
        public void JsonConvert_WithEmptyOrWhitespaceString_ShouldReturnNull(string input)
        {
            // Arrange & Act
            var result = JsonConvert.DeserializeObject<OrderRequest>(input);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void JsonConvert_WithValidOrderJson_ShouldDeserializeCorrectly()
        {
            // Arrange
            var testOrder = new OrderRequest { productname = "TestProduct" };
            var serializedOrder = JsonConvert.SerializeObject(testOrder);

            // Act
            var deserializedOrder = JsonConvert.DeserializeObject<OrderRequest>(serializedOrder);

            // Assert
            deserializedOrder.Should().NotBeNull();
            deserializedOrder!.productname.Should().Be("TestProduct");
        }

        [Fact]
        public void JsonConvert_WithInvalidJson_ShouldThrowJsonException()
        {
            // Arrange
            var invalidJson = "{ invalid json }";

            // Act
            Action act = () => JsonConvert.DeserializeObject<OrderRequest>(invalidJson);

            // Assert
            act.Should().Throw<JsonReaderException>();
        }

        [Fact]
        public void OrderStatus_ShouldHaveCompletedValue()
        {
            // Arrange & Act
            var completedStatus = OrderStatus.COMPLETED;

            // Assert
            completedStatus.Should().Be(OrderStatus.COMPLETED);
            Enum.IsDefined(typeof(OrderStatus), completedStatus).Should().BeTrue();
        }
    }
}