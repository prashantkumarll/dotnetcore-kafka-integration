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
using Newtonsoft.Json;

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
            Func<Task> act = async () => await _service.StartAsync(cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_ShouldStopServiceGracefully()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

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
            cts.Cancel();

            // Act
            Func<Task> act = async () => await _service.StopAsync(cts.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }
    }

    /// <summary>
    /// Integration tests for ProcessOrdersService that test the service behavior
    /// in a more realistic scenario with proper Kafka configurations
    /// </summary>
    public class ProcessOrdersServiceIntegrationTests
    {
        [Fact]
        public void ProcessOrdersService_WithRealConfigs_ShouldCreateSuccessfully()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
            service.Should().BeAssignableTo<BackgroundService>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("localhost:9092")]
        [InlineData("broker1:9092,broker2:9092")]
        public void ProcessOrdersService_WithDifferentBootstrapServers_ShouldCreateSuccessfully(string bootstrapServers)
        {
            // Arrange
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = "test-group"
            };

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void ProcessOrdersService_InheritsFromBackgroundService_ShouldHaveCorrectBaseType()
        {
            // Arrange
            var consumerConfig = new ConsumerConfig();
            var producerConfig = new ProducerConfig();

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().BeAssignableTo<BackgroundService>();
            service.Should().BeAssignableTo<IHostedService>();
        }
    }
}