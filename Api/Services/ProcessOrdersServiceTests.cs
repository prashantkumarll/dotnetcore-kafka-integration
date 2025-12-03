using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Services;
using Api.Models;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Api;

namespace Test
{
    public class ProcessOrdersServiceTests
    {
        private readonly Mock<ServiceBusProcessorOptions> _mockServiceBusProcessorOptions;
        private readonly Mock<ServiceBusClient> _mockServiceBusClient;
        private readonly ProcessOrdersService _service;

        public ProcessOrdersServiceTests()
        {
            // Arrange - Setup mock configurations
            _mockServiceBusProcessorOptions = new Mock<ServiceBusProcessorOptions>();
            _mockServiceBusClient = new Mock<ServiceBusClient>();
            _service = new ProcessOrdersService(_mockServiceBusProcessorOptions.Object, _mockServiceBusClient.Object);
        }

        [Fact]
        public void Constructor_WithValidConfigs_ShouldCreateInstance()
        {
            // Arrange
            var consumerConfig = new ServiceBusProcessorOptions();
            var producerConfig = new ServiceBusClient(connectionString);

            // Act
            var service = new ProcessOrdersService(consumerConfig, producerConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullServiceBusProcessorOptions_ShouldThrowArgumentNullException()
        {
            // Arrange
            var producerConfig = new ServiceBusClient(connectionString);

            // Act & Assert
            Action act = () => new ProcessOrdersService(default!, producerConfig);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullServiceBusClient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var consumerConfig = new ServiceBusProcessorOptions();

            // Act & Assert
            Action act = () => new ProcessOrdersService(consumerConfig, default!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task StartAsync_ShouldInitializeService()
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
        public async Task StopAsync_ShouldStopService()
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
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act
            await _service.StartAsync(cancellationTokenSource.Token);

            // Assert
            // Service should handle cancellation gracefully
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task StopAsync_WithCancelledToken_ShouldHandleCancellation()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act
            await _service.StopAsync(cancellationTokenSource.Token);

            // Assert
            // Service should handle cancellation gracefully
            _service.Should().NotBeNull();
        }

        [Fact]
        public void Service_ShouldImplementBackgroundService()
        {
            // Arrange & Act
            var service = _service;

            // Assert
            service.Should().BeAssignableTo<Microsoft.Extensions.Hosting.BackgroundService>();
        }
    }
}