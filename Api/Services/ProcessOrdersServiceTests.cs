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
        private readonly Mock<ConsumerConfig> _mockConsumerConfig;
        private readonly Mock<ProducerConfig> _mockProducerConfig;
        private readonly ProcessOrdersService _service;

        public ProcessOrdersServiceTests()
        {
            // Arrange - Setup mock configurations
            _mockConsumerConfig = new Mock<ConsumerConfig>();
            _mockProducerConfig = new Mock<ProducerConfig>();
            _service = new ProcessOrdersService(_mockConsumerConfig.Object, _mockProducerConfig.Object);
        }

        [Fact]
        public void Constructor_WithValidConfigs_ShouldCreateInstance()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig();
            var producerConfig = new ProducerConfig();

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldCreateInstance()
        {
            // Arrange
            ConsumerConfig consumerConfig = default!;
            var producerConfig = new ProducerConfig();

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldCreateInstance()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig();
            ProducerConfig producerConfig = default!;

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public async Task StartAsync_ShouldInitializeService()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act
            var action = async () => await _service.StartAsync(cancellationToken);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_ShouldStopService()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act
            var action = async () => await _service.StopAsync(cancellationToken);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartAsync_WithCancelledToken_ShouldNotThrow()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            var action = async () => await _service.StartAsync(cts.Token);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithCancelledToken_ShouldNotThrow()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act
            var action = async () => await _service.StopAsync(cts.Token);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void ProcessOrdersService_ShouldInheritFromBackgroundService()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(new ConsumerConfig(), new ProducerConfig());

            // Assert
            service.Should().BeAssignableTo<Microsoft.Extensions.Hosting.BackgroundService>();
        }
    }
}