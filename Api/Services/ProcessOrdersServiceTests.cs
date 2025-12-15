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
        public void Constructor_WithNullConsumerConfig_ShouldThrow()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(null!, _producerConfig);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullProducerConfig_ShouldThrow()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(_consumerConfig, null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithBothConfigsNull_ShouldThrow()
        {
            // Arrange & Act & Assert
            Action act = () => new ProcessOrdersService(null!, null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAsync_WithValidToken_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await service.StartAsync(cancellationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_WithValidToken_ShouldNotThrow()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = new CancellationToken();

            // Act & Assert
            Func<Task> act = async () => await service.StopAsync(cancellationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StartAsync_ThenStopAsync_ShouldWorkCorrectly()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);
            var cancellationToken = new CancellationToken();

            // Act
            await service.StartAsync(cancellationToken);
            
            // Assert - Should not throw when stopping after starting
            Func<Task> act = async () => await service.StopAsync(cancellationToken);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void ProcessOrdersService_ShouldInheritFromBackgroundService()
        {
            // Arrange
            var service = new ProcessOrdersService(_consumerConfig, _producerConfig);

            // Act & Assert
            service.Should().BeAssignableTo<Microsoft.Extensions.Hosting.BackgroundService>();
        }
    }
}