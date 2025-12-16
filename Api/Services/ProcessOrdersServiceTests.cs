using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Api.Services;
using Api.Models;
using Confluent.Kafka;
using Newtonsoft.Json;
using Api;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;
        private readonly ProcessOrdersService _service;

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

            _service = new ProcessOrdersService(_consumerConfig, _producerConfig);
        }

        [Fact]
        public void Constructor_WithValidConfigs_ShouldCreateInstance()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ConsumerConfig consumerConfig = default!;
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act & Assert
            Action act = () => new ProcessOrdersService(consumerConfig, producerConfig);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig { BootstrapServers = "localhost:9092" };
            ProducerConfig producerConfig = default!;

            // Act & Assert
            Action act = () => new ProcessOrdersService(consumerConfig, producerConfig);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAsync_ShouldInitializeServiceSuccessfully()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act
            Func<Task> act = async () => await _service.StartAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_ShouldStopServiceSuccessfully()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            await _service.StartAsync(cancellationToken);

            // Act
            Func<Task> act = async () => await _service.StopAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartAsync_WithCancelledToken_ShouldHandleCancellation()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            Func<Task> act = async () => await _service.StartAsync(cts.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithCancelledToken_ShouldHandleCancellation()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            await _service.StartAsync(CancellationToken.None);
            cts.Cancel();

            // Act
            Func<Task> act = async () => await _service.StopAsync(cts.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void Service_ShouldImplementIDisposable()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);

            // Assert
            service.Should().BeAssignableTo<IDisposable>();

            // Cleanup
            service.Dispose();
        }
    }
}