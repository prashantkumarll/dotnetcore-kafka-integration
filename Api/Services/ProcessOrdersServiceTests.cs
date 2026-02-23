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
using Microsoft.Extensions.Hosting;

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
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            var producerConfig = new ProducerConfig();

            // Act & Assert
            Action act = () => new ProcessOrdersService(null, producerConfig);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig();

            // Act & Assert
            Action act = () => new ProcessOrdersService(consumerConfig, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithBothNullConfigs_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Action act = () => new ProcessOrdersService(null, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAsync_ShouldInitializeServiceSuccessfully()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act
            await _service.StartAsync(cancellationToken);

            // Assert
            // Service should start without throwing exceptions
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task StopAsync_ShouldStopServiceGracefully()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            await _service.StartAsync(cancellationToken);

            // Act
            await _service.StopAsync(cancellationToken);

            // Assert
            // Service should stop without throwing exceptions
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task StartAsync_WithCancelledToken_ShouldHandleCancellation()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            Func<Task> act = async () => await _service.StartAsync(cts.Token);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithCancelledToken_ShouldHandleCancellation()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            Func<Task> act = async () => await _service.StopAsync(cts.Token);
            await act.Should().NotThrowAsync();
        }
    }
}