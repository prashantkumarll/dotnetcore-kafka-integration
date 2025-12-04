using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NSubstitute;
using Confluent.Kafka;
using Api.Services;
using Api.Models;

namespace Api.Tests.Services
{
    public class ProcessOrdersServiceTests
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;
        private readonly ProcessOrdersService _service;

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

            _service = new ProcessOrdersService(_consumerConfig, _producerConfig);
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
        public void Constructor_WithNullConsumerConfig_ShouldNotThrow()
        {
            // Arrange & Act
            Action act = () => new ProcessOrdersService(null, _producerConfig);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public async Task StartAsync_ShouldNotThrow()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(100));

            // Act
            Func<Task> act = async () => await _service.StartAsync(cts.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task StopAsync_ShouldNotThrow()
        {
            // Arrange
            using var cts = new CancellationTokenSource();

            // Act
            Func<Task> act = async () => await _service.StopAsync(cts.Token);

            // Assert
            await act.Should().NotThrowAsync();
        }
    }
}