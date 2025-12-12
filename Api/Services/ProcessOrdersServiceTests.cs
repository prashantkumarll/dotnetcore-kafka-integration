using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Services;
using Api.Models;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;

        public ProcessOrdersServiceTests()
        {
            // Arrange - Setup test configurations
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
        public void Constructor_WithValidConfigs_ShouldCreateInstance()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            var action = () => new ProcessOrdersService(null, _producerConfig);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            var action = () => new ProcessOrdersService(_consumerConfig, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithBothNullConfigs_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            var action = () => new ProcessOrdersService(null, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAsync_WithValidService_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var action = async () => await service.StartAsync(cancellationToken);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithValidService_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var action = async () => await service.StopAsync(cancellationToken);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartAsync_WithCancelledToken_ShouldHandleGracefully()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            var action = async () => await service.StartAsync(cts.Token);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_AfterStart_ShouldCompleteSuccessfully()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = CancellationToken.None;

            // Act
            await service.StartAsync(cancellationToken);
            
            // Assert
            var action = async () => await service.StopAsync(cancellationToken);
            await action.Should().NotThrowAsync();
        }
    }
}