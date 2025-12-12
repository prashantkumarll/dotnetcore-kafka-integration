using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Services;
using Api.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

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
            service.Should().BeOfType<ProcessOrdersService>();
        }

        [Fact]
        public void Constructor_WithNullConsumerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ProcessOrdersService(null, _producerConfig);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ProcessOrdersService(_consumerConfig, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithBothConfigsNull_ShouldThrowArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ProcessOrdersService(null, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ProcessOrdersService_ShouldInheritFromBackgroundService()
        {
            // Arrange & Act
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);

            // Assert
            service.Should().BeAssignableTo<BackgroundService>();
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
        public async Task ServiceLifecycle_StartAndStop_ShouldWorkCorrectly()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = CancellationToken.None;

            // Act & Assert - Start service
            Func<Task> startAct = async () => await service.StartAsync(cancellationToken);
            await startAct.Should().NotThrowAsync();

            // Act & Assert - Stop service
            Func<Task> stopAct = async () => await service.StopAsync(cancellationToken);
            await stopAct.Should().NotThrowAsync();
        }
    }
}